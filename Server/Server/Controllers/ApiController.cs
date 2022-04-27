using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class ApiController : ControllerBase
    {

        private string IMAGES_FOLDER_PATH;
        private string CARDS_FILE_PATH;
        private string CARDS_FILE_NAME;

        public ApiController (IConfiguration config)
        {
            IMAGES_FOLDER_PATH = config["ImagesFolderPath"];
            CARDS_FILE_PATH = config["Cards:CardsFilePath"];
            CARDS_FILE_NAME = config["Cards:CardsFileName"];

            if (!Directory.Exists (IMAGES_FOLDER_PATH))
                Directory.CreateDirectory (IMAGES_FOLDER_PATH);

            if (!Directory.Exists (CARDS_FILE_PATH))
                Directory.CreateDirectory (CARDS_FILE_PATH);

            if (!System.IO.File.Exists (CARDS_FILE_PATH + CARDS_FILE_NAME))
                using (FileStream fs = System.IO.File.Create (CARDS_FILE_PATH + CARDS_FILE_NAME));
        }

        [HttpPost ("card")]
        public void Add (string title, IFormFile image)
        {

            if (title.Length > 19)
                title = title.Substring (0, 19);

            // WORK WITH IMAGE
            string fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString () + ".jpg";

            using (var ms = new MemoryStream ())
            {
                image.CopyTo (ms);
                var fileBytes = ms.ToArray ();
                System.IO.File.WriteAllBytes (IMAGES_FOLDER_PATH + fileName, fileBytes);
            }

            // CREATING NEW CARD
            Card card = new Card (){ Id = Guid.NewGuid ().ToString (), Title = title, ImageUri = fileName };

            // GETTING CARDS LIST FROM THE FILE
            StreamReader reader = new StreamReader (CARDS_FILE_PATH + CARDS_FILE_NAME);
            string cardsStr = reader.ReadToEnd ();
            reader.Close ();

            List<Card> cards;
            if (cardsStr == "")
                cards = new List<Card> ();
            else
                cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);

            // ADDING NEW CARD
            cards.Add (card);

            // WRITING UPDATED LIST TO THE FILE
            using StreamWriter writer = new StreamWriter (CARDS_FILE_PATH + CARDS_FILE_NAME);
            writer.Write (JsonSerializer.Serialize<List<Card> > (cards));
        }

        [HttpGet ("card/{Id}/image")]
        public ActionResult GetCardImage (string Id)
        {
            using StreamReader reader = new StreamReader (CARDS_FILE_PATH + CARDS_FILE_NAME);
            string cardsStr = reader.ReadToEnd ();
            List<Card> cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);
            Card card = cards.FirstOrDefault (c => c.Id == Id);

            if (card != null)
                return File (System.IO.File.ReadAllBytes (IMAGES_FOLDER_PATH + card.ImageUri), "image/*",
                             card.ImageUri);
            else
                return Content ("");
        }

        [HttpGet ("card/{Id}")]
        public string GetCard (string Id)
        {
            using StreamReader reader = new StreamReader (CARDS_FILE_PATH + CARDS_FILE_NAME);
            string cardsStr = reader.ReadToEnd ();
            List<Card> cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);
            Card card = cards.FirstOrDefault (c => c.Id == Id);

            if (card != null)
                return JsonSerializer.Serialize<Card> (card);
            else
                return "";
        }

        [HttpGet ("card")]
        public string GetAllCards ()
        {
            using StreamReader reader = new StreamReader (CARDS_FILE_PATH + CARDS_FILE_NAME);
            return reader.ReadToEnd ();
        }

        public static byte[] CopyImageToByteArray (Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream ())
            {
                image.SaveAsJpeg (memoryStream);
                return memoryStream.ToArray ();
            }
        }

        public static Image GetImageFromByteArray (byte[] byteArray) => Image.Load<Rgba32> (byteArray);

        [HttpDelete ("card/delete")]
        public void DeleteCards ([ FromQuery ] string[] ids)
        {

            string cardsStr = GetAllCards ();
            List<Card> cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);
            for (int i = 0; i < ids.Length; i++)
                {
                    for (int j = 0; j < cards.Count; j++)
                        {
                            if (ids[i] == cards[j].Id)
                                {
                                    System.IO.File.Delete (IMAGES_FOLDER_PATH + cards[j].ImageUri);
                                    cards.RemoveAt (j);
                                    break;
                                }
                        }
                }
            using StreamWriter writer = new StreamWriter (CARDS_FILE_PATH + CARDS_FILE_NAME);
            writer.Write (JsonSerializer.Serialize<List<Card> > (cards));
        }

        [HttpPut ("card/fullupdate")]
        public void FullUpdate (string id, string title, IFormFile image)
        {
            if (title.Length > 19)
                title = title.Substring (0, 19);

            string cardsStr = GetAllCards ();
            List<Card> cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);
            string fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString () + ".jpg";

            using (var ms = new MemoryStream ())
            {
                image.CopyTo (ms);
                var fileBytes = ms.ToArray ();
                System.IO.File.WriteAllBytes (IMAGES_FOLDER_PATH + fileName, fileBytes);
            }

            Card card = cards.FirstOrDefault (c => c.Id == id);
            System.IO.File.Delete (IMAGES_FOLDER_PATH + card.ImageUri);
            card.Title = title;
            card.ImageUri = fileName;

            using StreamWriter writer = new StreamWriter (CARDS_FILE_PATH + CARDS_FILE_NAME);
            writer.Write (JsonSerializer.Serialize<List<Card> > (cards));
        }

        [HttpPut ("card/simpleupdate")]
        public void SimpleUpdate (string id, string title)
        {
            if (title.Length > 19)
                title = title.Substring (0, 19);

            string cardsStr = GetAllCards ();
            List<Card> cards = JsonSerializer.Deserialize<List<Card> > (cardsStr);

            Card card = cards.FirstOrDefault (c => c.Id == id);
            card.Title = title;

            using StreamWriter writer = new StreamWriter (CARDS_FILE_PATH + CARDS_FILE_NAME);
            writer.Write (JsonSerializer.Serialize<List<Card> > (cards));
        }
    }
}

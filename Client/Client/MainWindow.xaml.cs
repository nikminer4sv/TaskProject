using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int numberOfCards = 0;
        private int numberOfColumns = 1;
        private string cardsJSON = "";

        public static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateCardBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateCardWindow createCardWindow = new CreateCardWindow();
            createCardWindow.ShowDialog();
        }

        private string GetCardsFromServer()
        {

            WebRequest request = WebRequest.Create("http://localhost:5000/api/card");

            request.Method = "GET";
            request.ContentType = "text/plain";
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

        }

        public async void DownloadImage(Client.Models.Card card)
        {

            if (File.Exists("./Images/" + card.ImageUri))
                return;

            string url = "http://localhost:5000/api/card/" + card.Id + "/image";

            HttpClient client = new HttpClient();

            var response = client.GetAsync(url);

            using (var ms = await response.Result.Content.ReadAsStreamAsync())
            {

                using (var fs = File.Create("./Images/" + card.ImageUri))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }

        }

        public void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {

            string cardsJSON = GetCardsFromServer();

            if (cardsJSON == this.cardsJSON)
                return;

            this.cardsJSON = cardsJSON;

            int temp = numberOfCards;
            for (int i = 0; i < temp; i++)
            {
                var element = CardsGrid.Children.OfType<CustomCard>().FirstOrDefault(c => c.Name == "Card"+i);
                UnregisterName("Card"+i);
                if (element != null)
                {
                    CardsGrid.Children.Remove(element);
                    numberOfCards -= 1;
                }
            }


            if (cardsJSON != "")
            {
                List<Client.Models.Card> cards = JsonConvert.DeserializeObject<List<Client.Models.Card>>(cardsJSON);

                foreach (Client.Models.Card card in cards)
                {
                    DownloadImage(card);
                    CreateCardInGrid(card.Id, card.Title, "/Images/" + card.ImageUri);
                }
            }

        }

        private void CreateCardInGrid(string id, string title, string imagePath)
        {

            imagePath = Environment.CurrentDirectory + imagePath;

            CustomCard card = new CustomCard(id, title, imagePath);
            card.Name = "Card" + numberOfCards;
            CardsGrid.Children.Add(card);
            RegisterName(card.Name, card);

            numberOfCards += 1;

            if (_numberOfColumns * 2 < numberOfCards)
            {
                ColumnDefinition cDefenition = new ColumnDefinition();
                cDefenition.Width = GridLength.Auto;
                CardsGrid.ColumnDefinitions.Add(cDefenition);
            }

            int row, col;
            row = numberOfCards % 2 == 0 ? 1 : 0;
            col = (numberOfCards + 1) / 2;

            Grid.SetColumn(card, col);
            Grid.SetRow(card, row);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (instance == null)
                instance = this;

            RefreshBtn_Click(null, null);
            List<Client.Models.Card> cards = JsonConvert.DeserializeObject<List<Client.Models.Card>>(cardsJSON);
            foreach(string path in Directory.GetFiles("./Images/"))
            {
                string temp = path.Substring(path.LastIndexOf('/') + 1);
                if (!cards.Any(u => u.ImageUri == temp))
                {
                    File.Delete(path);
                }
            }

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

            List<string> ids = new List<string>();

            int temp = numberOfCards;
            for (int i = 0; i < temp; i++)
            {
                var element = CardsGrid.Children.OfType<CustomCard>().FirstOrDefault(c => c.Name == "Card"+i);
                if (element.isChecked)
                {
                    ids.Add(element.id);
                }
            }

            if (ids.Count > 0)
            {
                string url = "http://localhost:5000/api/card/delete?ids=";

                foreach (string id in ids)
                    url += id + "&ids=";


                url = url.Substring(0, url.Length - 5);
                System.Diagnostics.Trace.WriteLine(url);

                HttpClient client = new HttpClient();

                var response = client.DeleteAsync(url);

                Thread.Sleep(100);
                RefreshBtn_Click(sender, e);

            }

        }
    }
}

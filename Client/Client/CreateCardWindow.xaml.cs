using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для CreateCardWindow.xaml
    /// </summary>
    public partial class CreateCardWindow : Window
    {

        private bool isUpdating = false;
        private string startPath;
        private string id;

        public CreateCardWindow()
        {
            InitializeComponent();
        }

        public CreateCardWindow(string id, string title, string path)
        {
            InitializeComponent();
            this.id = id;
            TitleTextBox.Text = title;
            PathTextBox.Text = path;
            startPath = path;
            isUpdating = true;
            CreateBtn.Content = "Update";
            this.Title = "Update";
        }

        private void ChooseBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Image Files(jpg, jpeg, png, gif)|*.jpg;*.jpeg;*.png;*.gif;";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                PathTextBox.Text = filename;
            }

        }

        private async void CreateBtn_Click(object sender, RoutedEventArgs e)
        {

            if (TitleTextBox.Text != "" && PathTextBox.Text != "")
            {
                if (!File.Exists(PathTextBox.Text))
                {
                    MessageBox.Show("File is not exist!");
                    return;
                }

                if (_IsUpdating == false)
                {

                    using (var httpClient = new HttpClient())
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://localhost:5000/api/card?Title=" + TitleTextBox.Text))
                        {
                            request.Headers.TryAddWithoutValidation("accept", "*/*");

                            var multipartContent = new MultipartFormDataContent();
                            multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(PathTextBox.Text)), "image", System.IO.Path.GetFileName(PathTextBox.Text));
                            request.Content = multipartContent;

                            var response = await httpClient.SendAsync(request);
                        }
                    }

                    this.Close();
                } else
                {

                    using (var httpClient = new HttpClient())
                    {
                        if (startPath != PathTextBox.Text)
                        {
                            using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "http://localhost:5000/api/card/fullupdate?id=" + id + "&title=" + TitleTextBox.Text))
                            {
                                request.Headers.TryAddWithoutValidation("accept", "*/*");

                                var multipartContent = new MultipartFormDataContent();
                                multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(PathTextBox.Text)), "image", System.IO.Path.GetFileName(PathTextBox.Text));
                                request.Content = multipartContent;

                                var response = await httpClient.SendAsync(request);
                            }


                        }
                        else
                        {
                            using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "http://localhost:5000/api/card/simpleupdate?id=" + id + "&title=" + TitleTextBox.Text))
                            {
                                request.Headers.TryAddWithoutValidation("accept", "*/*");
                                var response = await httpClient.SendAsync(request);
                            }

                        }
                    }
                   
                    this.Close();
                }

            } else
            {
                if (!File.Exists(PathTextBox.Text))
                {
                    MessageBox.Show("Form is not valid!");
                    return;
                }
            }

            MainWindow.instance.RefreshBtn_Click(null, null);

        }

    }
}

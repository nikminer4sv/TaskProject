using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для CustomCard.xaml
    /// </summary>
    public partial class CustomCard : UserControl
    {

        public bool IsChecked = false;
        public string id;
        public string imagePath;

        public CustomCard(string id, string title, string imagePath)
        {
            InitializeComponent();
            this.id = id;
            container.Header = title;
            this.imagePath = imagePath;
            BitmapImage img = new BitmapImage(new Uri(@imagePath, UriKind.Absolute));
            cardImage.Source = img;
        }

        private void CardCheckBox_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = IsChecked == false ? true : false;
        }

        public string GetTitle()
        {
            return container.Header.ToString();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateCardWindow window = new CreateCardWindow(this.id, this.GetTitle(), this.imagePath);
            window.ShowDialog();
        }
    }
}

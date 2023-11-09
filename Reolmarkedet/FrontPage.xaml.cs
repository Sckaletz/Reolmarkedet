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

namespace Reolmarkedet
{
    /// <summary>
    /// Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : Page
    {
        public FrontPage()
        {
            InitializeComponent();
        }

        private void Admin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Admin());
        }

        private void Renter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Renter());
        }
    }
}

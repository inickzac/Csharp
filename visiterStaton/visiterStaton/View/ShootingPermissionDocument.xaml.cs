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
using System.Windows.Shapes;
using visiterStaton.VM.stationDBContext;

namespace visiterStaton.View
{
    /// <summary>
    /// Логика взаимодействия для ShootingPermissionDocument.xaml
    /// </summary>
    public partial class ShootingPermissionDocument : Window
    {
        public ShootingPermissionDocument()
        {
            InitializeComponent();
        }

        private void WebBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            int i = 0;
            //((WebBrowser)sender);
        }
    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

        private void TextBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            int i = 0;
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((WebBrowser)sender).NavigateToString(((DocumentPrint)DataContext).Html);
        }
    }
}

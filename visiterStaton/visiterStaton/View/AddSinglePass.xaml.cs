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

namespace visiterStaton.View
{
    /// <summary>
    /// Логика взаимодействия для AddSinglePass.xaml
    /// </summary>
    public partial class AddSinglePass : Window
    {
        public AddSinglePass()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SinglePass t = (SinglePass)((Button)sender).DataContext;
            t.OnPropertyChanged(nameof(t.Accompanying.Name));
            //t.OnPropertyChanged(nameof(t.Accompanying.Sel));
            t.OnPropertyChanged(nameof(t.Accompanying));
        }
    }
}

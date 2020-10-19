using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using visiterStaton.VM;
using visiterStaton.VM.stationDBContext;

namespace visiterStaton
{
    
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        MainWindowViewModel mainWindowViewModel;

        public App()
        {
            Initialization initialization = new Initialization();
            displayRootRegistry.RegisterWindowType<MainWindowViewModel, View.MainWindow>();
            displayRootRegistry.RegisterWindowType<DialogWindowViewModel, DialogWindow>();
            displayRootRegistry.RegisterWindowType<Organization, AddElements>();
            displayRootRegistry.RegisterWindowType<DocumentType, AddElements>();
            displayRootRegistry.RegisterWindowType<IssuingAuthority, AddElements>();
            displayRootRegistry.RegisterWindowType<Employee, View.AddAmployee>();
            displayRootRegistry.RegisterWindowType<Department, AddElements>();
            displayRootRegistry.RegisterWindowType<ShootingPermission, View.AddSP>();
            displayRootRegistry.RegisterWindowType<StationFacility, AddElements>();
            displayRootRegistry.RegisterWindowType<TemporarPass, View.AddTemporaryPas>();
            displayRootRegistry.RegisterWindowType<SinglePass, View.AddSinglePass>();
            displayRootRegistry.RegisterWindowType<Visitor, VisiterWindow>();
            displayRootRegistry.RegisterWindowType<DocumentPrint, View.ShootingPermissionDocument>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mainWindowViewModel = new MainWindowViewModel();

            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

            Shutdown();
        }
        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {

            CollectionView itemsOriginal = (CollectionView)CollectionViewSource.GetDefaultView(((ComboBox)sender).ItemsSource);
            itemsOriginal.Filter = o =>
            {
                if (string.IsNullOrEmpty(((ComboBox)sender).Text)) return true;
                else
                {
                    if (((IStantionDBTable)o).Name.ToLower().Contains(((ComboBox)sender).Text.ToLower())) return true;
                    else return false;
                };
            };
            ((ComboBox)sender).IsDropDownOpen = true;
            itemsOriginal.Refresh();
        }
        private void CBdocktype_GotFocus(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

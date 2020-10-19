using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;


namespace visiterStaton.VM
{
    class MainWindowViewModel : VMContext
    {
        // Закрытые поля команд
        private ICommand _openChildWindow;
        private RelayCommand addNewItem;
        private ICommand _openDialogWindow;

        private ObservableCollection<Visitor> visitors;
        private Visitor editableVisitor;
        private Visitor visitor;
        private string nameFilter;

        public ObservableCollection<Visitor> Visitors { get => visitors; set { visitors = value; OnPropertyChanged(); } }
        public Visitor EditableVisitor { get => editableVisitor; set { editableVisitor = value; OnPropertyChanged(); } }
        public Visitor Visitor { get => visitor; set { visitor = value; OnPropertyChanged(); } }
        Dictionary<string, Func<Visitor, bool>> filter = new Dictionary<string, Func<Visitor, bool>>();

        private bool conteinsActualShootingPer;
        private bool conteinsActualSinglePass;
        private bool conteinsActualTemporyPas;

        public string NameFilter
        {
            get => nameFilter; set
            {
                nameFilter = value;
                string NameOfFilter = "FilterName";
                if (String.IsNullOrEmpty(value) && filter.ContainsKey(NameOfFilter))
                {
                    filter.Remove(NameOfFilter);
                }
                else
                {
                    if (!filter.ContainsKey(NameOfFilter))
                    {
                        filter.Add("FilterName", new Func<Visitor, bool>(v =>
                       {
                           if ((v.Name + v.LastName).ToLower().Contains(nameFilter))
                               return true;
                           else return false;
                       })); 
                    }
                }
                initVisitors();
                OnPropertyChanged();
            }
        }

        public static MainWindowViewModel _mainWindowViewModel;

        public MainWindowViewModel()
        {
            _mainWindowViewModel = this;
            Visitor = new Visitor(this);
            EditableVisitor = new Visitor(this);
            Visitors = new ObservableCollection<Visitor>();
            initVisitors();
        }

        public void initVisitors()
        {
            Visitors.Clear();
            Visitor.GetALL().ForEach(o =>
            {
                o.Parent = this;
                o.LoadSP();
                o.LoadTP();
                o.LoadSingleP();
                if (filter.All(func => func.Value.Invoke(o)))
                {
                    Visitors.Add(o);                 
                }
            });
            OnPropertyChanged(nameof(Visitors));
        }

        public static MainWindowViewModel GetMainWindowViewModel()
        {
            return _mainWindowViewModel;
        }

        public ICommand OpenChildWindow
        {
            get
            {
                if (_openChildWindow == null)
                {
                    _openChildWindow = new OpenChildWindowCommand(this);
                }
                return _openChildWindow;
            }
        }
        public ICommand OpenDialogWindow
        {
            get
            {
                if (_openDialogWindow == null)
                {
                    _openDialogWindow = new OpenDialogWindowCommand(this);
                }
                return _openDialogWindow;
            }
        }
        public RelayCommand AddNewItem
        {
            get
            {
                return addNewItem ??
                    (addNewItem = new RelayCommand(obj =>
                    {
                        if (obj is IStantionDBTable)
                        {
                            MainWindowViewModel.GetMainWindowViewModel().OpenChildWindow.Execute(obj);
                            ((IStantionDBTable)obj).GetEmptyEditableTable();
                        }
                    },
                    obj =>
                    {
                        return true;
                    }));
            }
        }

        public bool ConteinsActualShootingPer { get => conteinsActualShootingPer; set {
                permissionFilter(value, "CurrentSP");              
                conteinsActualShootingPer = value;
                OnPropertyChanged();
                initVisitors();
            } }

        public bool ConteinsActualSinglePass { get => conteinsActualSinglePass; set {
                permissionFilter(value, "CurrentSingleP");
                conteinsActualSinglePass = value;
                OnPropertyChanged();
                initVisitors();
            } }

        public bool ConteinsActualTemporyPas { get => conteinsActualTemporyPas; set {
                conteinsActualTemporyPas = value;
                permissionFilter(value, "CurrentTP");
                OnPropertyChanged();
                initVisitors();
            } }

        public void permissionFilter(bool value, string NameOfFilterInMap)
        {
            var pi = typeof(Visitor).GetProperty(NameOfFilterInMap);
            if (value && !filter.ContainsKey(NameOfFilterInMap))
            {
                filter.Add(NameOfFilterInMap, new Func<Visitor, bool>(v =>
                {                    
                    if (!string.IsNullOrEmpty((string)pi.GetValue(v))) return true;
                    return false;
                }));
            }
            if (!value && filter.ContainsKey(NameOfFilterInMap))
            {
                filter.Remove(NameOfFilterInMap);
            }
        }
    }
 
    
    abstract class MyCommand : ICommand
    {
        protected MainWindowViewModel _mainWindowVeiwModel;

        public MyCommand(MainWindowViewModel mainWindowVeiwModel)
        {
            _mainWindowVeiwModel = mainWindowVeiwModel;
        }

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }

    class OpenChildWindowCommand : MyCommand
    {
        public OpenChildWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
        {
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override async void Execute(object VM)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            await displayRootRegistry.ShowModalPresentation(VM);
        }
    }

    class OpenDialogWindowCommand : MyCommand
    {
        public OpenDialogWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
        {
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override async void Execute(object VM)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            await displayRootRegistry.ShowModalPresentation(VM);

        }
    }
}


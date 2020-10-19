using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using visiterStaton.VM;
using visiterStaton.VM.stationDBContext;

namespace visiterStaton
{
    class StationDBContext: VMContext
    {
        private RelayCommand addNewItem;
        private RelayCommand saveCommand;
        private RelayCommand saveLocalCommand;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand documentGenerate;

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand(obj => {
                        IStantionDBTable dBTable = obj as IStantionDBTable;
                        dBTable.initEditableTable();
                        try
                        {
                            dBTable?.Delete();
                        }
                        catch (MySqlException e)
                        {
                            MainWindowViewModel.GetMainWindowViewModel().OpenDialogWindow.
                            Execute(new DialogWindowViewModel( e.Message, "Ошибка удаления", "#FFEEA3A3")); 
                        }
                    },
                    obj => {
                        if (obj == null) return false;
                        else return ((IStantionDBTable)obj).IsTableSelected;
                    }));
            }
        }
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj => {
                        IStantionDBTable dBTable = obj as IStantionDBTable;
                        try
                        {
                            dBTable?.Update();
                            MainWindowViewModel.GetMainWindowViewModel().OpenDialogWindow.
                            Execute(new DialogWindowViewModel("Успешное сохранение", "Успешное сохранение", "#FFB0E892"));
                        }
                        catch (Exception e)
                        {
                            MainWindowViewModel.GetMainWindowViewModel().OpenDialogWindow.
                            Execute(new DialogWindowViewModel(e.Message, "Ошибка сохранения", "#FFEEA3A3"));
                        
                        }
                        
                    },                  
                    obj => {
                        if (obj == null) return false;
                      return  ((IStantionDBTable)obj).Check();                    
                    }));
            }
        }

        public RelayCommand SaveLocalCommand
        {
            get
            {
                return saveLocalCommand ??
                    (saveLocalCommand = new RelayCommand(obj => {
                        ISaveLocal dBTable = obj as ISaveLocal;
                        dBTable.SaveLocal();
                    },
                    obj => {
                        if (obj == null) return false;
                        return ((IStantionDBTable)obj).Check();
                    }));
            }
        }

        public RelayCommand DocumentGenerate
        {
            get
            {
                return documentGenerate ??
                    (documentGenerate = new RelayCommand(obj => {
                        
                        MainWindowViewModel.GetMainWindowViewModel().OpenChildWindow.Execute(obj);
                    }                  
                    ));
            }
        }

        public RelayCommand AddNewItem
        {
            get
            {
                return addNewItem ??
                    (addNewItem = new RelayCommand(obj => {
                        if (obj is IStantionDBTable)
                        {
                            MainWindowViewModel.GetMainWindowViewModel().OpenChildWindow.Execute(obj);
                            //((IStantionDBTable)obj).GetEmptyEditableTable();

                        }
                    },
                    obj => {
                        return true;
                    }));
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(obj => {
                        if (obj is IStantionDBTable)
                        {
                            MainWindowViewModel.GetMainWindowViewModel().OpenDialogWindow.Execute(obj);
                            ((IStantionDBTable)obj).initEditableTable();
                        }
                    },
                    obj => {
                        if (obj == null) return false;
                        else return ((IStantionDBTable)obj).IsTableSelected;
                    }));
            }
        }
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static List<object> GetFromReader(Query query)
        {
            var tableList = new List<object>();
            using (MySqlCommand command = GetNewcommand())
            {
                var selected = new List<object>();
                command.CommandText = query.CommandString; ;
                command.Parameters.AddRange(query.ParametersList.ToArray());
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            query.Elector(reader, tableList);
                        }
                    }
                }
                command.Connection.Close();
                return tableList;               
            }
        }
        public static MySqlCommand GetNewcommand()
        {
            var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["mySqlConnectionDBSatation"].ConnectionString);
            var command = new MySqlCommand
            {
                Connection = connection
            };
            connection.Open();
            return command;
        }
        public class Query
        {
            string _commandString;
            List<MySqlParameter> _parametersList;
            Action<MySqlDataReader, List<object>> _elector;

            public Query(string commandString, List<MySqlParameter> parametersList, Action<MySqlDataReader, List<object>> elector)
            {
                _commandString = commandString;
                _parametersList = parametersList;
                _elector = elector;
            }
            public string CommandString { get => _commandString; set => _commandString = value; }
            public List<MySqlParameter> ParametersList { get => _parametersList; set => _parametersList = value; }
            public Action<MySqlDataReader, List<object>> Elector { get => _elector; set => _elector = value; }
        }
        protected class NotAllDataEnteredInTableException : Exception
        {
            public NotAllDataEnteredInTableException(string table) 
            {
                string _table = table;
                string message = string.Format("Введены не все данные в таблицу {0}", table);
               
            }
        }

     
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    
    class DocumentType : StationDBContext, IStantionDBTable, INotifyPropertyChanged
    {
        int? _id;
        string _type;
        int? _initId;
        string _initType;
        string _name = "DocumentType";
        public Document parent;
        private int _sel;
        
         
    
        private DocumentType(int id, string type) {
            _id = _initId = id;
            _type = _initType = type;
        }

        public DocumentType(Document parentTable)
        {
            parent = parentTable;
            Id = null;
        }

        public DocumentType()
        {
            Id = null;
        }

        public int? Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string Name { get => _type; 
        set { _type = value; OnPropertyChanged();  } }

        public bool IsTableSelected { get { if (parent != null) { return parent.DocumentType != null; } return false; } }

        public int Sel { get => _sel; set => _sel = value; }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                        {
                            error = "НЕ должно быть пустое значение";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }



        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new DocumentType(sqlReader.GetInt32("id"), sqlReader.GetString("type")));
        };

        public void FillBYId(int? id)
        {
            var oldDT = GetTableById((int)id);
            this.Id = oldDT.Id;
            this.Name = oldDT.Name;
        }

        public static DocumentType GetTableById(int id)
        {
            var commandString = "SELECT * FROM document_type where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<DocumentType>().FirstOrDefault();
        }
       
        public static List<DocumentType> FillterOn(string substring)
        {
            var commandString = " SELECT * FROM document_type WHERE type LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<DocumentType>().ToList();
        }
        
        public static List<DocumentType> GetALL()
        {
            var commandString = " SELECT * FROM document_type ";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<DocumentType>().ToList();
        }

        public void Update()
        {
            if (Check())
            {
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = " UPDATE document_type SET type=@Type  WHERE id = @id; ";
                        command.Parameters.Add(new MySqlParameter("@Type", Name));
                        command.Parameters.Add(new MySqlParameter("@id", Id));
                        command.ExecuteNonQuery();
                        _initType = Name;
                    }
                   if(parent!=null) UpdateParent();
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = " INSERT INTO `station`.`document_type` (`type`) VALUES(@doc);" +
                            "SELECT * FROM `station`.`document_type` WHERE id = LAST_INSERT_ID();";
                        command.Parameters.Add(new MySqlParameter("@doc", Name));
                        _initId=Id = (int)command.ExecuteScalar();
                        _initType = Name;
                        parent?.initDocumentTypes();
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_name); }
        }

        public bool Check()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public void Delete()
        {
            if (Id!=null)
            {
                using (MySqlCommand command = GetNewcommand())
                {
                    command.CommandText = " DELETE FROM document_type WHERE id=@id; ";
                    command.Parameters.Add(new MySqlParameter("@id", Id));
                    command.ExecuteNonQuery();
                }
                parent.initDocumentTypes();
            }
        }

        public void Cancel()
        {
            Name = _initType;
            Id = _initId;
        }

        public void GetEmptyEditableTable()
        {
            parent.NewdocumentType.Id = null;
            parent.NewdocumentType.Name= "";
        }
        
        public void UpdateParent()
        {
            parent.DocumentType.FillBYId((int)this.Id);              
        }
        public void initEditableTable()
        {
            if(parent.DocumentType !=null)
            {
                parent.NewdocumentType.FillBYId((int)parent.DocumentType.Id);
            }
        }
                         
        public override string ToString()
        {
            return Name;
        }

    }
}

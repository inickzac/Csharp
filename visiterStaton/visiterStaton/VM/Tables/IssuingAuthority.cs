using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    class IssuingAuthority : StationDBContext, IStantionDBTable
    {
        int? _id;
        string _name;
        int? _initId;
        string _initName;
        static string _nameTable= "issuing_authority";
        Document parent;
        private int _sel=-1;
        
        public IssuingAuthority(Document document)
        {
            parent = document;
        }
         IssuingAuthority(int? id, string name)
        {
            _initId = Id = id;
            _initName = Name = name;        
        }

        public int? Id { get => _id;
            set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        public bool IsTableSelected { get { if (parent != null) { return parent.IssuingAuthority != null; } return false; } }

        public int Sel { get => _sel; set { _sel = value; OnPropertyChanged(); } }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new IssuingAuthority(sqlReader.GetInt32("id"), sqlReader.GetString("name")));
        };
        public static IssuingAuthority GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<IssuingAuthority>().FirstOrDefault();
        }
        public static List<IssuingAuthority> FillterOn(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE name LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<IssuingAuthority>().ToList();
        }
        public static List<IssuingAuthority> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<IssuingAuthority>().ToList();
        }
        public void Update()
        {
            if (Check())
            {
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET name=@name  WHERE id = @id; ";
                        command.Parameters.Add(new MySqlParameter("@name", Name));
                        command.Parameters.Add(new MySqlParameter("@id", Id));
                        command.ExecuteNonQuery();
                      _initName= Name;
                        UpdateParent();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO `station`.`{_nameTable}` (`name`) VALUES(@name);" +
                            $"SELECT * FROM `station`.`{_nameTable}` WHERE id = LAST_INSERT_ID();";
                        command.Parameters.Add(new MySqlParameter("@name", Name));
                        _initId = Id = (int)command.ExecuteScalar();
                        _initName = Name;
                        parent?.initIssuingAuthority();
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }

        public bool Check()
        {
            return !string.IsNullOrEmpty(Name);
        }
        public void Delete()
        {         
                using (MySqlCommand command = GetNewcommand())
                {
                    command.CommandText = $" DELETE FROM {_nameTable} WHERE id=@id; ";
                    command.Parameters.Add(new MySqlParameter("@id", Id));
                    command.ExecuteNonQuery();
                }
                parent.initIssuingAuthority();           
        }
        public void Cancel()
        {
            Name = _initName;
            Id = _initId;
        }

        public void UpdateParent()
        {
            parent?.IssuingAuthority.FillBYId((int)this.Id);
        }

        public override string ToString()
        {
            return Name;
        }

       public void FillBYId(int? id)
        {
            if(id!=null)
            {
              var t=  GetTableById((int)id);
              Name = t.Name;
              Id = t.Id;
            }
        }
       
        public void initEditableTable()
        {
            parent.EditableIssuingAuthority.FillBYId((int)parent.IssuingAuthority.Id);
        }

        public void GetEmptyEditableTable()
        {
            parent.EditableIssuingAuthority.Name = "";
            parent.EditableIssuingAuthority.Id = null;
        }

      
    }
}

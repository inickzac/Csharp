using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    class Organization : StationDBContext, IStantionDBTable
    {
        int? _initId;
        string _initName;
        int? _id;
        string _name;
        static string _nameTable = "organization";
        static string _nameId = "id";
        static string _nameName = "name";


        private int _sel;
        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public Visitor Parent { get; set; }

        public bool IsTableSelected { get { if (Parent != null) { return Parent.Organization != null; } return false; } }

        public int Sel { get => _sel; set { _sel = value; OnPropertyChanged(); } }

        public Organization(int? id, string name)
        {
           _initId= Id = id;
           _initName=  Name = name;
        }

        public Organization(Visitor parent)
        {
            Parent = parent;
        }

        public Organization()
        {
        }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Organization(sqlReader.GetInt32($"{_nameId}"), sqlReader.GetString($"{_nameName}")));
        };

       
        public static Organization GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where {_nameId}=@{_nameId}";
            var param = (new MySqlParameter($"@{_nameId}", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Organization>().FirstOrDefault();
        }

        public static List<Organization> FillterOn(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE {_nameName} LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Organization>().ToList();
        }
        public static List<Organization> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Organization>().ToList();
        }
        public void Update()
        {
            if (Check())
            {
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_nameName}=@{_nameName}  WHERE {_nameId} = @{_nameId}; ";
                        command.Parameters.Add(new MySqlParameter($"@{_nameName}", Name));
                        command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                        command.ExecuteNonQuery();
                        _initName = Name;
                        if (Parent != null) UpdateParent();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} ({_nameName}) VALUES(@{_nameName});" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                        command.Parameters.Add(new MySqlParameter($"@{_nameName}", Name));
                        _initId = Id = (int)command.ExecuteScalar();
                        _initName = Name;
                        Parent?.InitOrganization();
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
                command.CommandText = $" DELETE FROM {_nameTable} WHERE {_nameId}=@{_nameId}; ";
                command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                command.ExecuteNonQuery();
                Parent.InitOrganization();
            }
        }
        public void Cancel()
        {
            Name = _initName;
            Id = _initId;
        }

      public void FillBYId(int? id)
        {
            if (id!=null)
            {
                Organization t = GetTableById((int)id);
                Id = t.Id;
                Name = t.Name; 
            }
        }

        public void UpdateParent()
        {
            Parent.Organization.FillBYId((int)this.Id);
        }
        public void initEditableTable()
        {
            if(Parent!=null)
            {
                Parent.EditableOrganization.FillBYId(Parent.Organization.Id);
            }
        }

        public void GetEmptyEditableTable()
        {
            Parent.EditableOrganization.Id = null;
            Parent.EditableOrganization.Name = null;
        }
        public override string ToString()
        {
            return Name;
        }

    }
}

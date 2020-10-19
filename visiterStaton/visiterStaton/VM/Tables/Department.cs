using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    class Department : StationDBContext, IStantionDBTable
    {
        int? _initId;
        string _initName;
        int? _id;
        string _name;
        static string _nameTable = "department";
        Employee parent;

        public Department(Employee parent)
        {
            this.parent = parent;
            Id = null;
        }

        Department(int? initId, string initName)
        {
            Id = _initId = initId;
            Name = _initName = initName;
        }

        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public int Sel { get; set; }
        
        public bool IsTableSelected { get { if (parent != null) { return parent.Department != null; } return false; } }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Department(sqlReader.GetInt32("id"), sqlReader.GetString("name")));
        };

        public static Department GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Department>().FirstOrDefault();
        }
        public static List<Department> FillterOn(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE name LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Department>().ToList();
        }
        public static List<Department> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Department>().ToList();
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
                        _initName = Name;
                        if (parent != null) UpdateParent();
                        parent?.initAllDepartment();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} (`name`) VALUES(@name);" +
                            $"SELECT * FROM {_nameTable} WHERE id = LAST_INSERT_ID();";
                        command.Parameters.Add(new MySqlParameter("@name", Name));
                        _initId = Id = (int)command.ExecuteScalar();
                        _initName = Name;
                        parent?.initAllDepartment();
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
            parent.initAllDepartment();
        }
        public void Cancel()
        {
            Name = _initName;
            Id = _initId;
        }

        public override string ToString()
        {
            return Name;
        }

        public void initEditableTable()
        {
            if (parent.Department != null)
            {
                parent.EditableDepartment.FillBYId((int)parent.Department.Id);
            }
        }

        public void FillBYId(int? id)
        {
            var oldDT = Department.GetTableById((int)id);
            this.Id = oldDT.Id;
            this.Name = oldDT.Name;
        }
        public void GetEmptyEditableTable()
        {
            parent.EditableDepartment.Id = null;
            parent.EditableDepartment.Name = "";
        }
        public void UpdateParent()
        {
            parent.Department.FillBYId((int)this.Id);
        }
    }

}


using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{ 
    class StationFacility : StationDBContext, IStantionDBTable
    {
        int? _initId;
        string _initName;
        int? _id;
        string _name;
        static string _nameTable = "station_facility";
        ShootingPermission parent;
        
        public StationFacility(IStantionDBTable parent)
        {
            this.parent = (ShootingPermission)parent;
            Id = null;
        }

         StationFacility(int? initId, string initName)
        {
           Id= _initId = initId;
           Name= _initName = initName;
        }

        public int Sel { get; set; }
        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        public bool IsTableSelected { get { if (parent != null) { return parent.SubjectOfShooting != null; } return false; } }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new StationFacility(sqlReader.GetInt32("id"), sqlReader.GetString("name")));
        };

        public static StationFacility GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<StationFacility>().FirstOrDefault();
        }
        public static List<StationFacility> FillterOn(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE name LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<StationFacility>().ToList();
        }
        public static List<StationFacility> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<StationFacility>().ToList();
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
                        parent?.initEStationFacility();
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
                        parent?.initEStationFacility();
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
            parent?.initEStationFacility();
        }
        public void Cancel()
        {
            Name = _initName;
            Id = _initId;
        }

        public void FillBYId(int? id)
        {
            var oldDT = GetTableById((int)id);
            this.Id = oldDT.Id;
            this.Name = oldDT.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public void initEditableTable()
        {
            if (parent.SubjectOfShooting != null)
            {
                parent.EditStationFacility.FillBYId((int)parent.SubjectOfShooting.Id);
            }
        }

        public void GetEmptyEditableTable()
        {
            if (parent!=null)
            {
                parent.EditStationFacility.Id = null;
                parent.EditStationFacility.Name = ""; 
            }
        }
        public void UpdateParent()
        {
            parent.SubjectOfShooting.FillBYId((int)this.Id);
        }
    }
}

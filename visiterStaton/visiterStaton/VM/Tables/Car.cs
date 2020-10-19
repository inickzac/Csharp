using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    class Car : StationDBContext//, IStantionDBTable
    {
        int? _id;
        string _model;
        string _number;

        int? _initId;
        string _initModel;
        string _initNumber;

        static string _nameTable = "car";
        static string  _nameId="id";
       static string _nameModel= "model";
       static string _nameNumber= "number";

        public int? Id { get => _id; set => _id = value; }
        public string Model { get => _model; set => _model = value; }
        public string Number { get => _number; set => _number = value; }

        Car( string model, string number)
        {
           _initModel= Model = model;
           _initNumber=  Number = number;
        }
        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Car(sqlReader.GetString(_nameModel),
                sqlReader.GetString(_nameNumber)));
        };
        public static Car GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where {_nameId}=@{_nameId}";
            var param = (new MySqlParameter($"@{_nameId}", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Car>().FirstOrDefault();
        }
        public static List<Car> FillterOn(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE {_nameNumber} LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Car>().ToList();
        }
        public static List<Car> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Car>().ToList();
        }
        public bool Check()
        {
            return string.IsNullOrEmpty(Model) && string.IsNullOrEmpty(Number);
        }
        public void Update()
        {
            if (Check())
            {            
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_nameModel}=@{_nameModel}, {_nameNumber}=@{_nameNumber}" +
                            $"  WHERE {_nameId} = @{_nameId}; ";
                        InitParam(command);
                        command.ExecuteNonQuery();
                        NewInitialization();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} ({_nameModel}, {_nameNumber}" +
                            $" VALUES(@{_nameModel}, @{_nameNumber} ;" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                        InitParam(command);
                        NewInitialization();
                        _initId = Id = (int)command.ExecuteScalar();
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }

        private void NewInitialization()
        {
            _initId = Id;
            _initModel = Model;
            _initNumber = Number;
        }

        private void InitParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter($"@{_nameModel}", Model));
            command.Parameters.Add(new MySqlParameter($"@{_nameNumber}", Number));
            command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
        }
        public void Delete()
        {
            using (MySqlCommand command = GetNewcommand())
            {
                command.CommandText = $" DELETE FROM {_nameTable} WHERE id=@id; ";
                command.Parameters.Add(new MySqlParameter("@id", Id));
                command.ExecuteNonQuery();
            }
        }
        public void Cancel()
        {
            Id = _initId;
            Model = _initModel;
            Number = _initNumber;           
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace visiterStaton
{
    class Employee:StationDBContext , IStantionDBTable
    {
        int? _id;
        string _name;
        string _lastName;
        Department _department;
        string _position;

        int? _initId;
        string _initName;
        string _initLastName;
        Department _initDepartment;
        string _initPosition;
        public int Sel { get; set; }
        private string findName;

        static string _nameTable = "employee";
        private Department editableDepartment;
        private ObservableCollection<Department> departments = new ObservableCollection<Department>();

        IUpdateEmployee Parent;
        public int? Id { get => _id; set => _id = value; }
        public string FirstName { get => _name; set { _name = value; OnPropertyChanged(); OnPropertyChanged(nameof(Name)); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(LastName)); } }
        public Department Department { get => _department; set { _department = value; OnPropertyChanged(); OnPropertyChanged(nameof(Department)); } }
        public string Position { get => _position; set { _position = value; OnPropertyChanged(); OnPropertyChanged(nameof(Position)); } }

        public bool IsTableSelected { get { if (Parent != null) { return Parent.EmployeeIsSelected(this); ; } return false; } }

        public Department EditableDepartment { get => editableDepartment; set { editableDepartment = value; OnPropertyChanged(); } }
        public ObservableCollection<Department> Departments { get => departments; set { departments = value; OnPropertyChanged(); } }

        public string Name { get { return FirstName + " " + LastName + " " + Department.Name; }
            set { findName = value; OnPropertyChanged(); } }

        Employee(int? initId, string initName, string initLastName, Department initDepartment, string initPosition)
        {
           Id= _initId = initId;
            FirstName = _initName = initName;
           LastName= _initLastName = initLastName;
           Department= _initDepartment = initDepartment;
           Position= _initPosition = initPosition;
            EditableDepartment = new Department(this);
            initAllDepartment();
        }

        public Employee(IUpdateEmployee parent)
        {
            EditableDepartment = new Department(this);
            initAllDepartment();
            this.Parent = parent;
        }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Employee(sqlReader.GetInt32("id"), sqlReader.GetString("name"), sqlReader.GetString("lastName"),
            Department.GetTableById(sqlReader.GetInt32("department")), sqlReader.GetString("position")));
        };
        public static Employee GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Employee>().FirstOrDefault();
        }
        public static List<Employee> FillterOnName(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE name LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Employee>().ToList();
        }
        public static List<Employee> FillterOnLastName(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE lastName LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Employee>().ToList();
        }
        public static List<Employee> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Employee>().ToList();
        }
        public void Update()
        {
            if (Check())
            {
                Department.Update();
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET name=@name, lastName=@lastName, department=@department," +
                            $"position=@position  WHERE id = @id; ";
                        command.Parameters.Add(new MySqlParameter("@id", Id));
                        InitParam(command);
                        command.ExecuteNonQuery();
                        if (Parent != null) UpdateParent();
                        NewInitialization();
                        Parent?.InitEmployees(this);
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} (`name`, `lastName`, `department`, `position`)" +
                            $" VALUES(@name, @lastName, @department, @position );" +
                            $"SELECT * FROM {_nameTable} WHERE id = LAST_INSERT_ID();";
                        InitParam(command);
                        _initId = Id = (int)command.ExecuteScalar();
                        NewInitialization();
                        Parent?.InitEmployees(this);
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }
        private void InitParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter("@name", FirstName));
            command.Parameters.Add(new MySqlParameter("@lastName", LastName));
            command.Parameters.Add(new MySqlParameter("@department", Department.Id));
            command.Parameters.Add(new MySqlParameter("@position", Position));
        }
        private void NewInitialization()
        {
            _initName = FirstName;
            _lastName = LastName;
            _initPosition = Position;
            _department = Department;
        }
        public bool Check()
        {      
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && 
                !string.IsNullOrEmpty(Position) && Department!=null && Department.Check();
        }
        public void Delete()
        {
            using (MySqlCommand command = GetNewcommand())
            {
                command.CommandText = $" DELETE FROM {_nameTable} WHERE id=@id; ";
                command.Parameters.Add(new MySqlParameter("@id", Id));
                command.ExecuteNonQuery();
            }
            Parent?.InitEmployees(this);
        }
        public void Cancel()
        {
            Id = _initId;
            FirstName = _initName;
            LastName = _initLastName;
            Department = _initDepartment;
            Position = _initPosition;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Department.Name ;
        }
        public void FillBYId(int? id)
        {
            FillByOtherEmployee(GetTableById((int)id));         
        }
       
        public void FillByOtherEmployee(Employee employee)
        {
            this.Id = employee.Id;
            this.FirstName = employee.FirstName;
            this.LastName = employee.LastName;
            this.Position = employee.Position;
            this.Department = employee.Department;
        }
        
        public void initEditableTable()
        {
           Parent.InitEditableEmployeeTable(this);
        }

        public void GetEmptyEditableTable()
        {
            Parent.GetEmptyEmployeeEditableTable(this);

        }
        public void UpdateParent()
        {
            Parent.FillEmployeeById(this.Id, this);
        }
        public void initAllDepartment()
        {
            int? id = Department?.Id;
           Departments.Clear();
            Department.GetALL().ForEach(o => Departments.Add(o));

            if (id != null)
            {
                Department = Departments.Where(o => { return o.Id == id; }).FirstOrDefault();
            }
        }
    
    }
}

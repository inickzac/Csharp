using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using visiterStaton.VM;

namespace visiterStaton
{
    class Visitor : StationDBContext, IStantionDBTable,IDataErrorInfo
    {
        int? _id;
        string _name;
        string _lastName;
        Document _document;
        Organization _organization;
        string _position;
        ShootingPermission _shootingPermission;
        int countOfInitProperty = 0;

        int? _initId;
        string _initName;
        string _initLastName;
        Document _initDocument;
        Organization _initOrganization;
        string _initPosition;
        ShootingPermission _initShootingPermission;

        Organization _editableOrganization;
        private ObservableCollection<Organization> _organizations = new ObservableCollection<Organization>();

        private ObservableCollection<ShootingPermission> shootingPermissions  = new ObservableCollection<ShootingPermission>();
        private ShootingPermission editableShootingPermission;       

        private ObservableCollection<TemporarPass> temporarPasses = new ObservableCollection<TemporarPass>();
        private TemporarPass editabletemporarPass;
        private TemporarPass temporarPass;

        private ObservableCollection<SinglePass> singlePasses  = new ObservableCollection<SinglePass>();
        private SinglePass  editableSinglePass;
        private SinglePass  singlePass;

        static string _nameTable = "visitor";
        static string _nameId = "id";
        static string _nameName = "name";
        static string _nameLastName= "lastname";
        static string _nameDocument = "document";
        static string _nameOrganization = "place_of_work";
        static string _namePosition = "position";

        public MainWindowViewModel Parent; 

        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public Document Document { get => _document; set { _document = value; OnPropertyChanged(); } }
        public Visitor EditableVisitor { get => Parent.EditableVisitor; set { Parent.EditableVisitor = value; OnPropertyChanged(); } }

        public Organization Organization { get => _organization; set { _organization = value; OnPropertyChanged(); } }
        public ObservableCollection<Organization> Organizations { get => _organizations; set { _organizations = value; OnPropertyChanged(); } }
        public Organization EditableOrganization { get => _editableOrganization; set { _editableOrganization = value; OnPropertyChanged(); } }
        public string CurrentSP {get {
                string dates=""; 
                DateTime currentDate = DateTime.Now;
                shootingPermissions.Where(o => o.Start <= currentDate && o.End > currentDate).ToList().ForEach(o => dates +="Дo "+ o.End + "\n");
                return dates;
            } 
        }

        public string CurrentSingleP
        {
            get
            {
                string dates = "";
                DateTime currentDate = DateTime.Now;
                SinglePasses.Where(o => o.PassIssued <= currentDate && o.ValidUntil > currentDate).ToList().ForEach(o => dates += "Дo " + o.ValidUntil + "\n");
                return dates;
            }
        }

        public string CurrentTP
        {
            get
            {
                string dates = "";
                DateTime currentDate = DateTime.Now;
                TemporarPasses.Where(o => o.ValidWith <= currentDate && o.ValidUntil > currentDate).ToList().ForEach(o => dates += "Дo " + o.ValidUntil + "\n");
                return dates;
            }
        }

        public string Position { get => _position;  set { _position = value; OnPropertyChanged(); } }
        public ShootingPermission ShootingPermission { get => _shootingPermission; set { _shootingPermission = value; OnPropertyChanged(); } }

        public bool IsTableSelected => true;

        public ObservableCollection<ShootingPermission> ShootingPermissions { get => shootingPermissions; set { shootingPermissions = value; OnPropertyChanged(); } }

        public ShootingPermission EditableShootingPermission { get => editableShootingPermission; set { editableShootingPermission = value; OnPropertyChanged(); } }

        public ObservableCollection<TemporarPass> TemporarPasses { get => temporarPasses; set { temporarPasses = value; OnPropertyChanged(); } }
        public TemporarPass EditabletemporarPass { get => editabletemporarPass; set { editabletemporarPass = value; OnPropertyChanged(); } }
        public TemporarPass TemporarPass { get => temporarPass; set { temporarPass = value; OnPropertyChanged(); } }
        public ObservableCollection<SinglePass> SinglePasses { get => singlePasses; set { singlePasses = value; OnPropertyChanged(); } }
        public SinglePass EditableSinglePass { get => editableSinglePass; set { editableSinglePass = value; OnPropertyChanged(); } }
        public SinglePass SinglePass { get => singlePass; set { singlePass = value; OnPropertyChanged(); } }

        public Visitor(MainWindowViewModel parent)
        {
            this.Parent = parent;
            InitOrganization();
            Document = new Document();
            EditableOrganization = new Organization(this);
            InitShootingPermission();
            EditableShootingPermission = new ShootingPermission(this);
            EditabletemporarPass = new TemporarPass(this);
            EditableSinglePass = new SinglePass(this);
           
        }

        Visitor(int id,string name, string lastName, Document document, Organization organization, 
            string position)
        {
            _initId = Id = id;
            _initName = Name = name;
          _initLastName=  LastName = lastName;
          _initDocument=  Document = document;
          _initOrganization= Organization = organization;
          _initPosition =Position = position;
            InitOrganization();
            EditableOrganization = new Organization(this);
            InitShootingPermission();
            EditableShootingPermission = new ShootingPermission(this);
            EditabletemporarPass = new TemporarPass(this);
            EditableSinglePass = new SinglePass(this);
          
        }

        public string this[string columnName]
        {
            get
            {
                string NoEmptyItem = "НЕ должно быть пустое значение";

                string error = String.Empty;
                switch (columnName)
                {
                    case "Organization":
                        if (Organization==null && countOfInitProperty>2)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "Position":
                        if (string.IsNullOrEmpty(Position) && countOfInitProperty>2)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "Name":
                        if (string.IsNullOrEmpty(Position) && countOfInitProperty > 2)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "LastName":
                        if (string.IsNullOrEmpty(Position) && countOfInitProperty > 2)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                }
                countOfInitProperty++;
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
      

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Visitor(sqlReader.GetInt32(_nameId), 
                sqlReader.GetString(_nameName), 
                sqlReader.GetString(_nameLastName),
                Document.GetTableById(sqlReader.GetInt32(_nameDocument)), 
                Organization.GetTableById(sqlReader.GetInt32(_nameOrganization)),
                sqlReader.GetString(_namePosition) 
                ));
        };
        public static Visitor GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where {_nameId}=@{_nameId}";
            var param = (new MySqlParameter($"{_nameId}", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Visitor>().FirstOrDefault();
        }
        public static List<Employee> FillterOnName(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE {_nameName} LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Employee>().ToList();
        }
        public static List<Visitor> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Visitor>().ToList();
        }      

        public bool Check()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(LastName) && Document.Check() &&
                Organization != null && !string.IsNullOrEmpty(Position);

        }
        public void Update()
        {
            if (Check())
            {
                ShootingPermissions.ToList().ForEach(o => o.Update());
                TemporarPasses.ToList().ForEach(o => o.Update());
                SinglePasses.ToList().ForEach(o => o.Update());
                Document.Update();
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_nameName}=@{_nameName}, {_nameLastName}=@{_nameLastName}," +
                            $" {_nameDocument}=@{_nameDocument}, {_nameOrganization}=@{_nameOrganization},{_namePosition}=@{_namePosition} " +
                            $" WHERE {_nameId} = @{_nameId}; ";
                        initParam(command);                      
                        NewInitialization();
                        command.ExecuteNonQuery();
                        Parent?.initVisitors();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} ({_nameName}, {_nameLastName}, {_nameDocument}" +
                            $",{_nameOrganization}, {_namePosition})" +
                            $" VALUES(@{_nameName}, @{_nameLastName}, @{_nameDocument}, @{_nameOrganization}, @{_namePosition} " +
                            $" );" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                            initParam(command);
                        NewInitialization();
                        _initId = Id = (int)command.ExecuteScalar();                   
                    }
                    Parent?.initVisitors();
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }

        private void NewInitialization()
        {
            _initName = Name;
            _initLastName = LastName;
            _initDocument = Document;
            _initOrganization = Organization;
            _initPosition = Position;
            _initShootingPermission = ShootingPermission;
        }

        private void initParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameName}", Name));
            command.Parameters.Add(new MySqlParameter($"@{_nameLastName}", LastName));
            command.Parameters.Add(new MySqlParameter($"@{_nameDocument}", Document.Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameOrganization}", Organization.Id));
            command.Parameters.Add(new MySqlParameter($"@{_namePosition}", Position));
        }
        public void Delete()
        {
            using (MySqlCommand command = GetNewcommand())
            {
                command.CommandText = $" DELETE FROM {_nameTable} WHERE {_nameId}=@{_nameId}; ";
                command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                command.ExecuteNonQuery();
            }
            Parent?.initVisitors();
        }

        public Visitor FillThisByOtherVisiter(Visitor Tempory)
        {
            this.Id = Tempory.Id;
            this.Position = Tempory.Position;
            this.Organization = Tempory.Organization;
            this.Name = Tempory.Name;
            this.LastName = Tempory.LastName;
            this.Document = Tempory.Document;
            this.Organization = Tempory.Organization;
            this.ShootingPermissions = Tempory.shootingPermissions;
            this.SinglePasses = Tempory.singlePasses;
            this.TemporarPasses = Tempory.TemporarPasses;
            this.Parent = Tempory.Parent;
            return this;
        }

        public void Cancel()
        {
            Document.Cancel();
            Organization.Cancel();
            ShootingPermission.Cancel();
            Id = _initId;
            Name = _initName;
            LastName = _initLastName;         
            Position = _initPosition;
        }

        public void InitShootingPermission()
        {
            OnPropertyChanged(nameof(ShootingPermissions));               
        }
        public void InitOrganization()
        {
            var t = Organization.GetALL();
            int? id = Organization?.Id;
            Organizations.Clear();
            Organization.GetALL().ForEach(o => Organizations.Add(o));
            if (id != null)
            {
                Organization = Organizations.Where(o => o.Id == id).FirstOrDefault();
            }
        }
        public void initEditableTable()
        {
            FillThisByOtherVisiter(Parent.Visitor);
        }

        public void GetEmptyEditableTable()
        {
            FillThisByOtherVisiter(new Visitor(this.Parent));
        }

        public void FillBYId(int? id)
        {
            if (id != null)
            {
                FillThisByOtherVisiter(GetTableById((int)id));
            }
        }
   public void LoadSP()
        {
            ShootingPermission.FillterOnParent(Id.ToString()).ForEach(o => { o.parent = Parent.EditableVisitor; ShootingPermissions.Add(o); });
        }
    public void LoadTP()
        {
           TemporarPass.FillterOnVisiter(this).ForEach(o => { o.Parent = Parent.EditableVisitor; TemporarPasses.Add(o); });
        }
        public void LoadSingleP()
        {
           SinglePass.FillterOnVisiter(this).ForEach(o => { o.Parent = Parent.EditableVisitor; SinglePasses.Add(o); });
        }

        public void CurrentValidPpass()
        {
            DateTime currentDate = DateTime.Now;
            var currentSP= shootingPermissions.Where(o => o.Start <= currentDate && o.End > currentDate);
            var currentSingleP=SinglePasses.Where(o => o.PassIssued <= currentDate && o.ValidUntil > currentDate);
            var currentTP=TemporarPasses.Where(o => o.ValidWith <= currentDate && o.ValidUntil > currentDate);             
        }
    }
  
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using visiterStaton.VM.stationDBContext;

namespace visiterStaton
{
    class ShootingPermission : StationDBContext, IStantionDBTable, ISaveLocal, ICloneable, IUpdateEmployee, IDocument
    {
        int? _id;
        string _cameraType;
        Employee _shootingAllowed;
        string _shootingPurpose;
        DateTime _start;
        DateTime _end;
        StationFacility _subjectOfShooting;
        static string _nameTable = "shooting_permission";

        int? _initId;
        string _initCameraType;
        Employee _initShootingAllowed;
        string _initshootingPurpose;
        DateTime _initStart;
        DateTime _initEnd;
        StationFacility _initSubjectOfShooting;
        string name = "";
        public DocumentPrint DocumentTP { get; set; } = new DocumentPrint(null);


        static string _nameId = "id";
        static string _nameCameraType = "camera_type";
        static string _nameShootingAllowed = "shooting_allowed";
        static string _nameShootingPurpose = "shooting_purpose";
        static string _nameStart = "start";
        static string _nameEnd = "end";
        static string _nameSubjectOfShooting = "subject_of_shooting";
        static string _nameVisitor = "visitor";

        public Visitor parent;

        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private Employee initEmployee;

        private ObservableCollection<StationFacility> stationFacilities = new ObservableCollection<StationFacility>();
        private StationFacility editStationFacility;
        public ShootingPermission EditableShootingPermission { get => parent.EditableShootingPermission; set { parent.EditableShootingPermission = value; OnPropertyChanged(); } }


        public ShootingPermission(Visitor parent)
        {
            this.parent = parent;
            initEStationFacility();
            InitEmployees(initEmployee);
            initEmployee = new Employee(this);
            EditStationFacility = new StationFacility(this);
            DocumentTP = new DocumentPrint(this);
        }

        ShootingPermission(int? id, string cameraType, Employee shootingAllowed, string shootingPurpose,
            DateTime start, DateTime end, StationFacility stationFacility, Visitor parent = null)
        {
            _initId = Id = id;
            _initCameraType = CameraType = cameraType;
            _initShootingAllowed = ShootingAllowed = shootingAllowed;
            _initshootingPurpose = ShootingPurpose = shootingPurpose;
            _initStart = Start = start;
            _initEnd = End = end;
            _initSubjectOfShooting = SubjectOfShooting = stationFacility;
            this.parent = parent;
            initEStationFacility();
            InitEmployees(initEmployee);
            initEmployee = new Employee(this);
            EditStationFacility = new StationFacility(this);
            DocumentTP = new DocumentPrint(this);
        }

        public int? Id { get => _id; set => _id = value; }
        public string CameraType { get => _cameraType; set { _cameraType = value; OnPropertyChanged(); } }
        public string ShootingPurpose { get => _shootingPurpose; set { _shootingPurpose = value; OnPropertyChanged(); } }
        public DateTime Start { get => _start; set { _start = value; OnPropertyChanged(); } }
        public DateTime End { get => _end; set { _end = value; OnPropertyChanged(); } }

        public Employee ShootingAllowed { get => _shootingAllowed; set { _shootingAllowed = value; OnPropertyChanged(); } }
        public StationFacility SubjectOfShooting { get => _subjectOfShooting; set { _subjectOfShooting = value; OnPropertyChanged(); } }
        public ObservableCollection<Employee> Employees { get => employees; set { employees = value; OnPropertyChanged(); } }

        public Employee InitEmployee { get => initEmployee; set { initEmployee = value; OnPropertyChanged(); } }

        public string Name { get => " "; set => name = " "; }

        public bool IsTableSelected => true;

        public ObservableCollection<StationFacility> StationFacilities { get => stationFacilities; set { stationFacilities = value; OnPropertyChanged(); } }
        public StationFacility EditStationFacility { get => editStationFacility; set { editStationFacility = value; OnPropertyChanged(); } }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new ShootingPermission(sqlReader.GetInt32(_nameId), sqlReader.GetString(_nameCameraType),
            Employee.GetTableById(sqlReader.GetInt32(_nameShootingAllowed)), sqlReader.GetString(_nameShootingPurpose),
            sqlReader.GetDateTime(_nameStart), sqlReader.GetDateTime(_nameEnd),
            StationFacility.GetTableById(sqlReader.GetInt32(_nameSubjectOfShooting))));
        };
        public static ShootingPermission GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<ShootingPermission>().FirstOrDefault();
        }
        public static List<ShootingPermission> FillterOnParent(string substring)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE visitor LIKE @substring";
            var param = new MySqlParameter("@substring", "%" + substring + "%");
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<ShootingPermission>().ToList();
        }
        public static List<ShootingPermission> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<ShootingPermission>().ToList();
        }
        public void Update()
        {
            if (Check())
            {
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_nameShootingAllowed}=@{_nameShootingAllowed}, {_nameCameraType}=@{_nameCameraType}," +
                            $" {_nameShootingPurpose}=@{_nameShootingPurpose}, {_nameSubjectOfShooting}=@{_nameSubjectOfShooting}, {_nameStart}=@{_nameStart} " +
                            $" ,{_nameEnd}=@{_nameEnd} ,{_nameVisitor}=@{_nameVisitor} WHERE {_nameId} = @{_nameId}; ";
                        InitParam(command);
                        command.ExecuteNonQuery();
                        NewInitialization();
                    }
                    if (parent != null) UpdateParent();
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} ({_nameShootingAllowed}, {_nameCameraType}, {_nameShootingPurpose}" +
                            $",{_nameSubjectOfShooting}, {_nameStart}, {_nameEnd}, {_nameVisitor})" +
                            $" VALUES(@{_nameShootingAllowed}, @{_nameCameraType}, @{_nameShootingPurpose}, @{_nameSubjectOfShooting}, @{_nameStart}, " +
                            $"@{_nameEnd}, @{_nameVisitor} );" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                        InitParam(command);
                        _initId = Id = (int)command.ExecuteScalar();
                        parent?.InitShootingPermission();
                        NewInitialization();
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }
        private void NewInitialization()
        {
            _initCameraType = CameraType;
            _initShootingAllowed = ShootingAllowed;
            _initshootingPurpose = ShootingPurpose;
            _initStart = Start;
            _initEnd = End;
            _initSubjectOfShooting = SubjectOfShooting;
        }
        private void InitParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameCameraType}", CameraType));
            command.Parameters.Add(new MySqlParameter($"@{_nameShootingAllowed}", ShootingAllowed.Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameShootingPurpose}", ShootingPurpose));
            command.Parameters.Add(new MySqlParameter($"@{_nameSubjectOfShooting}", SubjectOfShooting.Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameStart}", Start));
            command.Parameters.Add(new MySqlParameter($"@{_nameEnd}", End));
            command.Parameters.Add(new MySqlParameter($"@{_nameVisitor}", parent.Id));
        }
        public bool Check()
        {
            return !string.IsNullOrEmpty(CameraType) && !string.IsNullOrEmpty(ShootingPurpose) && ShootingAllowed != null &&
                 SubjectOfShooting != null && Start != null && End != null;

        }
        public void Delete()
        {
            if (Id!=null)
            {
                using (MySqlCommand command = GetNewcommand())
                {
                    command.CommandText = $" DELETE FROM {_nameTable} WHERE {_nameId}=@{_nameId}; ";
                    command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                    command.ExecuteNonQuery();
                } 
            }

            if (parent.ShootingPermissions.Contains(parent.ShootingPermission))
            { 
                parent.ShootingPermissions.Remove(parent.ShootingPermission); 
            }
            parent?.InitShootingPermission();
        }
        public void Cancel()
        {
            Id = _initId;
            CameraType = _initCameraType;
            ShootingAllowed = _initShootingAllowed;
            ShootingPurpose = _initshootingPurpose;
            Start = _initStart;
            End = _initEnd;
            SubjectOfShooting = _initSubjectOfShooting;
        }
        public void InitEmployees(Employee employee)
        {
            int? id = ShootingAllowed?.Id;
            Employees.Clear();
            Employee.GetALL().ForEach(o => Employees.Add(o));
            if (id != null)
            {
                ShootingAllowed = employees.Where(o => o.Id == id).FirstOrDefault();
            }
        }

        public void initEStationFacility()
        {
            int? id = SubjectOfShooting?.Id;
            StationFacilities.Clear();
            StationFacility.GetALL().ForEach(o => StationFacilities.Add(o));
            if (id != null)
            {
                SubjectOfShooting = stationFacilities.Where(o => o.Id == id).FirstOrDefault();
            }
        }
        public void FillBYId(int? id)
        {
            var oldDT = GetTableById((int)id);
            this.Id = oldDT.Id;
            this.CameraType = oldDT.CameraType;
            this.End = oldDT.End;
            this.Start = oldDT.Start;
            this.ShootingAllowed = oldDT.ShootingAllowed;
            this.ShootingPurpose = oldDT.ShootingPurpose;
            this.SubjectOfShooting = oldDT.SubjectOfShooting;
        }

        public void initEditableTable()
        {

            if (parent.ShootingPermission!=null)
            {
                this.Id = parent.ShootingPermission.Id;
                this.CameraType = parent.ShootingPermission.CameraType;
                this.End = parent.ShootingPermission.End;
                this.Start = parent.ShootingPermission.Start;
                this.ShootingAllowed = parent.ShootingPermission.ShootingAllowed;
                this.ShootingPurpose = parent.ShootingPermission.ShootingPurpose;
                this.SubjectOfShooting = parent.ShootingPermission.SubjectOfShooting; 
            }
        }

        public void GetEmptyEditableTable()
        {
            parent.EditableShootingPermission.Id = null;
            parent.EditableShootingPermission.ShootingPurpose = null;
            parent.EditableShootingPermission.ShootingAllowed = null;
            parent.EditableShootingPermission.Start = new DateTime();
            parent.EditableShootingPermission.End = new DateTime();
            parent.EditableShootingPermission.CameraType = "";
            parent.ShootingPermission = null;
        }



        public void UpdateParent()
        {
            parent.ShootingPermission?.FillBYId((int)this.Id);
        }

        public void SaveLocal()
        {
            if (parent.ShootingPermissions.Contains(parent.ShootingPermission)) 
                parent.ShootingPermissions.Remove(parent.ShootingPermission);
                parent.ShootingPermissions.Add((ShootingPermission)this.Clone());
        }
    

        public object Clone()
        {
            var newSHP = new ShootingPermission(parent);

            newSHP.Id = this.Id;
            newSHP.CameraType = this.CameraType;
            newSHP.End = this.End;
            newSHP.Start = this.Start;
            newSHP.ShootingAllowed = this.ShootingAllowed;
            newSHP.ShootingPurpose = this.ShootingPurpose;
            newSHP.SubjectOfShooting = this.SubjectOfShooting;
            return newSHP;
        }

        public bool EmployeeIsSelected(Employee employee)
        {
            return ShootingAllowed != null;
        }

        public void InitEditableEmployeeTable(Employee employee)
        {
            if (ShootingAllowed != null)
            {
                InitEmployee.FillBYId((int)ShootingAllowed.Id);
            }
        }

        public void GetEmptyEmployeeEditableTable(Employee employee)
        {
            InitEmployee.FirstName = null;
            InitEmployee.Id = null;
            InitEmployee.LastName = null;
            InitEmployee.Department = null;
            InitEmployee.Position = null;
        }

        public void FillEmployeeById(int? id, Employee employee)
        {
            ShootingAllowed.FillBYId((int)id);
            OnPropertyChanged(nameof(ShootingAllowed.Sel));
        }
        public string GetHtml()
        {
            return $@"<!doctype html>
<html>
<head>
<meta charset=""utf-8"">
<title>new</title>
<style type=""text/css"">
	html body {{
   size: 7in 9.25in;
		margin: 27mm 16mm 27mm 16mm;
	}}
	span span {{
        margin - left: 12pt;
	}}
	.photo {{
        border: 1pt #C42C2E;
		width: 150pt;
		height: 150pt;
		vertical-align: middle;
		padding: 5pt;
	}}
	
	</style>
	</head>

<body>
<h1 style=""text-align: center"">Филиал ОАО</h1>
	<p style=""font-size: 13pt; padding-left: 20pt "">Разрешение на съемк <u>{parent.Id}</u></p>
	<span>Имя</span>
	<span style=""padding-left: 30pt""><u>{parent.Name}</u></span>
<p><span>Фамилия</span>
	<span style=""padding-left: 30pt""><u>{parent.LastName}</u></span></p>
	<table width=""100%"">
  <tr>
    <td >Место работы</td>
	   <td >ОАО ОРГАНИЗАЦИЯ</td>
  </tr>
 <tr>
    <td >Должность</td>
	   <td >{parent.Position}</td>
  </tr>	
		
		<tr>
    <td ><span>Пропуск выдан на основании</span><span style=""margin-left: 12pt""><u>{parent.Document.DocumentType}</u></span></td>
	   <td>&nbsp;</td>			
				<tr>
     <tr>
   <td ><span>серия </span>
		   <span style=""margin-left: 12pt""><u>{parent.Document.Series}</u></span></td>
	   <td ><span>номер документа </span>
		   <span style=""margin-left: 12pt""><u>{parent.Document.Number}</u></span></td>				
  </tr>	
										
  <tr>
    <td ><span>выданного </span>
		   <span style=""margin-left: 12pt""><u>{parent.Document.IssuingAuthority}</u></span></td>
	   <td><span>{parent.Document.Date_of_issue}</span>
		  </td>					
  </tr>	
		</tr>	
  </tr>	
	</table>
<p><span >Пропуск выдан</span><span style=""margin-left: 12pt""><u>{Start}</u></span></p>
<p><span >Срок действия до</span><span style=""margin-left: 12pt""><u>{End}</u></span></p>
<p><span >Срок действия продлен до</span><span style=""margin-left: 12pt"">____._____.20___ </span></p>
<p><span >Пропуск выдал</span><span style=""margin-left: 12pt"">{ShootingAllowed.Name + " " + ShootingAllowed.LastName}</span></p>
<table  width=""100%"" style=""margin-bottom: 20pt"">
	<tr>
	<td class=""photo"">
		<span>Фото</span></td>
		<td style=""vertical-align:middle; margin: 30pt""><div style=""margin-left: 100pt; margin-right:100pt;"">В случае отсутствия фотографии, временный пропуск  действителен только при наличии документа на  основании которого он выписан</div></td>
	</tr>
</table>
<div style="" margin-bottom: 20pt"">Дополнительная Информация</div>
<table width=""100%"" style=""margin-bottom: 40pt;"">
<tr style=""padding-top: 10pt;"" >
	<td>цель нахождения</td>
	<td>{ShootingPurpose}</td>
	</tr>
</table>
<div>{DateTime.Now.Date.Date}</div>
</body>
</html>
";
        }
        }
}

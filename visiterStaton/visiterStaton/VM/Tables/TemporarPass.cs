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
    class TemporarPass : StationDBContext, IStantionDBTable, IUpdateEmployee, ISaveLocal, IDocument
    {
        int? _id;
        DateTime _validWith;
        DateTime _validUntil;
        string _purposeOfIssuance;
        Employee _passIssued;
        Visitor _visitor;

        int? _initId;
        DateTime _initValidWith;
        DateTime _initValidUntil;
        string _initPurposeOfIssuance;
        Employee _initPassIssued;
        Visitor _initVisitor;

        static string _nameTable = "temporar_pass";
        public static string _nameId = "id";
        public static string _nameValidWith = "valid_with";
        public static string _nameValidUntil = "valid_until";
        public static string _namePurposeOfIssuance = "purpose_of_issuance";
        public static string _namePassIssued = "pass_issued";
        public static string _nameVisitor = "visitor";       
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private Employee initEmployee;
        public DocumentPrint DocumentTP { get; set; } = new DocumentPrint(null);
        TemporarPass(int? id, DateTime validWith, DateTime validUntil, 
            string purposeOfIssuance, Employee passIssued, Visitor visitor = null)
        {
          _initId= _id = id;
          _initValidWith=  _validWith = validWith;
          _initValidUntil= _validUntil = validUntil;
         _initPurposeOfIssuance=   _purposeOfIssuance = purposeOfIssuance;
          _initPassIssued= _passIssued = passIssued;
          _initVisitor= _visitor = visitor;
            DocumentTP = new DocumentPrint(this);

        }
        public TemporarPass(Visitor parent)
        {
            InitEmployee = new Employee(this);
            InitEmployees(InitEmployee);
            Parent = parent;
            DocumentTP = new DocumentPrint(this);
        }
        public TemporarPass EditabletemporarPass { get => Parent.EditabletemporarPass; set { Parent.EditabletemporarPass = value; OnPropertyChanged(); } }
        public Visitor Parent;
        public int? Id { get => _id; set => _id = value; }
        public DateTime ValidWith { get => _validWith; set { _validWith = value; OnPropertyChanged(); } }
        public DateTime ValidUntil { get => _validUntil; set { _validUntil = value; OnPropertyChanged(); } }
        public string PurposeOfIssuance { get => _purposeOfIssuance; set { _purposeOfIssuance = value; OnPropertyChanged(); } }
        public Employee PassIssued { get => _passIssued; set { _passIssued = value; OnPropertyChanged(); } }                    
        public Visitor Visitor { get => _visitor; set => _visitor = value; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsTableSelected => true;
        public ObservableCollection<Employee> Employees { get => employees; set { employees = value; OnPropertyChanged(); } }
        public Employee InitEmployee { get => initEmployee; set { initEmployee = value; OnPropertyChanged(); } }
        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new TemporarPass(
                sqlReader.GetInt32(_nameId),
                sqlReader.GetDateTime(_nameValidWith),
                sqlReader.GetDateTime(_nameValidUntil),
                sqlReader.GetString(_namePurposeOfIssuance),
                Employee.GetTableById(sqlReader.GetInt32(_namePassIssued))
               ));
        };
        public static TemporarPass GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where {_nameId}=@{_nameId}";
            var param = (new MySqlParameter($"@{_nameId}", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<TemporarPass>().FirstOrDefault();
        }
        public static List<TemporarPass> FillterOnVisiter(Visitor visitor)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE {_nameVisitor} = @substring";
            var param = new MySqlParameter("@substring", visitor.Id);
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<TemporarPass>().ToList();
        }
        public static List<TemporarPass> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<TemporarPass>().ToList();
        }
        public bool Check()
        {
            return ValidWith != null &&
               ValidUntil != null &&
               !string.IsNullOrEmpty(PurposeOfIssuance) &&
               PassIssued!=null && PassIssued.Check() 
               /*Visitor!=null && Visitor.Check()*/;      
        }
        public void Update()
        {
            if (Check())
            {
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_nameValidWith}=@{_nameValidWith}, {_nameValidUntil}=@{_nameValidUntil}," +
                            $" {_namePassIssued}=@{_namePassIssued}, {_namePurposeOfIssuance}=@{_namePurposeOfIssuance}," +
                            $" {_nameVisitor}=@{_nameVisitor} WHERE {_nameId} = @{_nameId}; ";
                        InitParam(command);
                        command.ExecuteNonQuery();
                        NewInitialization();
                    }
                }
                if (_id == null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" INSERT INTO {_nameTable} ({_namePassIssued}, {_nameValidUntil}, {_nameValidWith}" +
                            $",{_namePurposeOfIssuance},  {_nameVisitor})" +
                            $" VALUES(@{_namePassIssued}, @{_nameValidUntil}, @{_nameValidWith} , @{_namePurposeOfIssuance}, " +
                            $"@{_nameVisitor} );" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                        InitParam(command);
                        Debug.WriteLine(command.CommandText);
                        _initId = Id = (int)command.ExecuteScalar();
                        NewInitialization();
                    }
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }
        private void NewInitialization()
        {
            _initId = Id;
            _initPassIssued = PassIssued;
            _initValidUntil = ValidUntil;
            _initValidWith = ValidWith;
            _initPurposeOfIssuance = PurposeOfIssuance;
            _initVisitor = Visitor;
        }
        private void InitParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
            command.Parameters.Add(new MySqlParameter($"@{_namePassIssued}", PassIssued.Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameValidUntil}", ValidUntil));
            command.Parameters.Add(new MySqlParameter($"@{_nameValidWith}", ValidWith));
            command.Parameters.Add(new MySqlParameter($"@{_namePurposeOfIssuance}", PurposeOfIssuance));
            command.Parameters.Add(new MySqlParameter($"@{_nameVisitor}", Parent.Id));
        }
        public void Delete()
        {
            if (Id !=null)
            {
                using (MySqlCommand command = GetNewcommand())
                {
                    command.CommandText = $" DELETE FROM {_nameTable} WHERE {_nameId}=@{_nameId}; ";
                    command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                    command.ExecuteNonQuery();
                } 
            }
            if (Parent.TemporarPasses.Contains(Parent.TemporarPass))
            {
                Parent.TemporarPasses.Remove(Parent.TemporarPass);
            }
        }
        public void Cancel()
        {
            PassIssued?.Cancel();
            Visitor?.Cancel();
            Id = _initId;
            ValidWith = _initValidWith;
            PassIssued = _initPassIssued;
            ValidUntil = _initValidUntil;
            PurposeOfIssuance = _initPurposeOfIssuance;
        }
        public TemporarPass FillThisByOtherTemporarPass(TemporarPass Tempory)
        {
            this.Id = Tempory.Id;
            this.ValidUntil = Tempory.ValidUntil;
            this.ValidWith = Tempory._initValidWith;
            this.PurposeOfIssuance = Tempory.PurposeOfIssuance;
            this.PassIssued = Tempory.PassIssued;
            return this;
        }     
        public void initEmployees()
        {
            int? id = PassIssued?.Id;
            Employees.Clear();
            Employee.GetALL().ForEach(o => Employees.Add(o));
            if (id != null)
            {
                PassIssued = employees.Where(o => o.Id == id).FirstOrDefault();
            }
        }
        public void initEditableTable()
        {
            FillThisByOtherTemporarPass(Parent.TemporarPass);
        }
        public void GetEmptyEditableTable()
        {
            FillThisByOtherTemporarPass(new TemporarPass(Parent));
            Parent.TemporarPass = null;
        }
        public void FillBYId(int? id)
        {
            if (id!=null)
            {
                FillThisByOtherTemporarPass(GetTableById((int)id)); 
            }
        }
        public bool EmployeeIsSelected(Employee employee)
        {
            return PassIssued != null;
        }
        public void InitEmployees(Employee employee)
        {
            int? id = PassIssued?.Id;
            Employees.Clear();
            Employee.GetALL().ForEach(o => Employees.Add(o));
            if (id != null)
            {
                PassIssued = employees.Where(o => o.Id == id).FirstOrDefault();
            }
        }
        public void InitEditableEmployeeTable(Employee employee)
        {
            if (PassIssued != null)
            {
                InitEmployee.FillBYId((int)PassIssued.Id);
            }
        }
        public void GetEmptyEmployeeEditableTable(Employee employee)
        {
            InitEmployee.FillByOtherEmployee(new Employee(this));
        }
        public void FillEmployeeById(int? id, Employee employee)
        {
            PassIssued.FillBYId((int)id);
            OnPropertyChanged(nameof(PassIssued.Sel));
        }
        public void SaveLocal()
        {
            if (Parent.TemporarPasses.Contains(Parent.TemporarPass))
                Parent.TemporarPasses.Remove(Parent.TemporarPass);
            Parent.TemporarPasses.Add((new TemporarPass(Parent).FillThisByOtherTemporarPass(this)));
        }
        
        public string GetHtml()
        {
           return  $@"<!doctype html>
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
	<p style=""font-size: 13pt; padding-left: 20pt "">Временный пропуск <u>{Parent.Id}</u></p>
	<span>Имя</span>
	<span style=""padding-left: 30pt""><u>{Parent.Name}</u></span>
<p><span>Фамилия</span>
	<span style=""padding-left: 30pt""><u>{Parent.LastName}</u></span></p>
	<table width=""100%"">
  <tr>
    <td >Место работы</td>
	   <td >ОАО ОРГАНИЗАЦИЯ</td>
  </tr>
 <tr>
    <td >Должность</td>
	   <td >{Parent.Position}</td>
  </tr>	
		
		<tr>
    <td ><span>Пропуск выдан на основании</span><span style=""margin-left: 12pt""><u>{Parent.Document.DocumentType}</u></span></td>
	   <td>&nbsp;</td>			
				<tr>
     <tr>
   <td ><span>серия </span>
		   <span style=""margin-left: 12pt""><u>{Parent.Document.Series}</u></span></td>
	   <td ><span>номер документа </span>
		   <span style=""margin-left: 12pt""><u>{Parent.Document.Number}</u></span></td>				
  </tr>	
										
  <tr>
    <td ><span>выданного </span>
		   <span style=""margin-left: 12pt""><u>{Parent.Document.IssuingAuthority}</u></span></td>
	   <td><span>{Parent.Document.Date_of_issue}</span>
		  </td>					
  </tr>	
		</tr>	
  </tr>	
	</table>
<p><span >Пропуск выдан</span><span style=""margin-left: 12pt""><u>{ValidWith}</u></span></p>
<p><span >Срок действия до</span><span style=""margin-left: 12pt""><u>{ValidUntil}</u></span></p>
<p><span >Срок действия продлен до</span><span style=""margin-left: 12pt"">____._____.20___ </span></p>
<p><span >Пропуск выдал</span><span style=""margin-left: 12pt"">{PassIssued.Name +" "+ PassIssued.LastName}</span></p>
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
	<td>{PurposeOfIssuance}</td>
	</tr>
</table>
<div>{DateTime.Now.Date.Date}</div>
</body>
</html>
";
        }                       
     
    }
}

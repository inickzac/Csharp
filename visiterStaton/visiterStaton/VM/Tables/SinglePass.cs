using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using visiterStaton.VM.stationDBContext;

namespace visiterStaton
{ 
    class SinglePass : StationDBContext, IStantionDBTable, IUpdateEmployee, ISaveLocal, IDocument
    {
        int? _id;
        DateTime _passIssued;
        DateTime _validUntil;

        public DocumentPrint DocumentTP { get; set; } = new DocumentPrint(null);
        private Employee _singlePassIssued;
        private Employee _editableSinglePassIssued;
        private ObservableCollection<Employee> _singlePassIssueds = new ObservableCollection<Employee>();


        string _purposeOfIssuance;

        private Employee _accompanying;
        private Employee _editableAccompanying;
        private ObservableCollection<Employee> _accompanyings = new ObservableCollection<Employee>();

        Visitor _visitor;

        int? _initId;
        DateTime _initPassIssued;
        DateTime _initValidUntil;
        Employee _initSinglePassIssued;
        string _initPurposeOfIssuance;
        Employee _initAccompanying;
        Visitor _initVisitor;

        public Visitor Parent;

        static string _nameTable = "single_pass";
        public static string _nameId = "id";
        public static string _namePassIssued = "pass_issued";
        public static string _nameValidUntil= "valid_until";
        public static string _nameSinglePassIssued = "single_pass_issued";
        public static string _namePurposeOfIssuance = "purpose_of_issuance";
        public static string _nameAccompanying = "accompanying";
        public static string _nameVisitor = "visitor";

        public int? Id { get => _id; set => _id = value; }
        public DateTime PassIssued { get => _passIssued; set { _passIssued = value; OnPropertyChanged(); } }
        public DateTime ValidUntil { get => _validUntil; set { _validUntil = value; OnPropertyChanged(); } }
        public Employee SinglePassIssued { get => _singlePassIssued; set { _singlePassIssued = value; OnPropertyChanged(); } }


        public SinglePass EditableSinglePass { get => Parent.EditableSinglePass; set { Parent.EditableSinglePass = value; OnPropertyChanged(); } }

        public string PurposeOfIssuance { get => _purposeOfIssuance; set { _purposeOfIssuance = value; OnPropertyChanged(); } }
        public Employee Accompanying { get => _accompanying; set => 
                _accompanying = value; }
        internal Visitor Visitor { get => _visitor; set => _visitor = value; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsTableSelected => true;

        public Employee EditableSinglePassIssued { get => _editableSinglePassIssued; set { _editableSinglePassIssued = value; OnPropertyChanged(); } }
        public ObservableCollection<Employee> SinglePassIssueds { get => _singlePassIssueds; set { _singlePassIssueds = value; OnPropertyChanged(); } }

        public Employee EditableAccompanying { get => _editableAccompanying; set { _editableAccompanying = value; OnPropertyChanged(); } }
        public ObservableCollection<Employee> Accompanyings { get => _accompanyings; set { _accompanyings = value; OnPropertyChanged(); } }

        public SinglePass(int? id, DateTime passIssued, DateTime validUntil, Employee singlePassIssued, 
            string purposeOfIssuance, Employee accompanying, Visitor visitor=null)
        {
            this.Id = id;
            PassIssued = passIssued;
            ValidUntil = validUntil;
            SinglePassIssued = singlePassIssued;
            PurposeOfIssuance = purposeOfIssuance;
            Accompanying = accompanying;
            Visitor = visitor;
            InitEmployees(EditableAccompanying);
            InitEmployees(EditableSinglePassIssued);
            EditableAccompanying = new Employee(this);
            EditableSinglePassIssued = new Employee(this);
            DocumentTP = new DocumentPrint(this);
        }

        public SinglePass(Visitor Parent)
        {
            this.Parent = Parent;
            EditableAccompanying = new Employee(this);
            EditableSinglePassIssued = new Employee(this);
            InitEmployees(EditableAccompanying);
            InitEmployees(EditableSinglePassIssued);
            DocumentTP = new DocumentPrint(this);
        }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new SinglePass(
                sqlReader.GetInt32(_nameId),
                sqlReader.GetDateTime(_namePassIssued), 
                sqlReader.GetDateTime(_nameValidUntil),
                Employee.GetTableById(sqlReader.GetInt32(_nameSinglePassIssued)),
                sqlReader.GetString(_namePurposeOfIssuance),
                Employee.GetTableById(sqlReader.GetInt32(_nameAccompanying))));
        };
        public static SinglePass GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where {_nameId}=@{_nameId}";
            var param = (new MySqlParameter($"@{_nameId}", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<SinglePass>().FirstOrDefault();
        }
        public static List<SinglePass> FillterOnVisiter(Visitor visitor)
        {
            var commandString = $"SELECT * FROM {_nameTable} WHERE {_nameVisitor} = @substring";
            var param = new MySqlParameter("@substring", visitor.Id);
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<SinglePass>().ToList();
        }
        public static List<SinglePass> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<SinglePass>().ToList();
        }
        public bool Check()
        {
            return PassIssued != null
                && ValidUntil != null
                && SinglePassIssued != null && SinglePassIssued.Check()
                && !string.IsNullOrEmpty(PurposeOfIssuance)
                && Accompanying != null && Accompanying.Check();
        }
        public void Update()
        {
            if (Check())
            {
                SinglePassIssued.Update();
                Accompanying.Update();
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET {_namePassIssued}=@{_namePassIssued}, {_nameValidUntil}=@{_nameValidUntil}," +
                            $" {_nameSinglePassIssued}=@{_nameSinglePassIssued}, {_namePurposeOfIssuance}=@{_namePurposeOfIssuance},{_nameAccompanying}=@{_nameAccompanying}, " +
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
                        command.CommandText = $" INSERT INTO {_nameTable} ({_namePassIssued}, {_nameValidUntil}, {_nameSinglePassIssued}" +
                            $",{_namePurposeOfIssuance}, {_nameAccompanying}, {_nameVisitor})" +
                            $" VALUES(@{_namePassIssued}, @{_nameValidUntil}, @{_nameSinglePassIssued}, @{_namePurposeOfIssuance}, @{_nameAccompanying}, " +
                            $"@{_nameVisitor} );" +
                            $"SELECT * FROM {_nameTable} WHERE {_nameId} = LAST_INSERT_ID();";
                        InitParam(command);
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
            _initSinglePassIssued = SinglePassIssued;
            _initPurposeOfIssuance = PurposeOfIssuance;
            _initAccompanying = Accompanying;
            _initVisitor = Visitor;
        }

        private void InitParam(MySqlCommand command)
        {
            command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
            command.Parameters.Add(new MySqlParameter($"@{_namePassIssued}", PassIssued));
            command.Parameters.Add(new MySqlParameter($"@{_nameValidUntil}", ValidUntil));
            command.Parameters.Add(new MySqlParameter($"@{_nameSinglePassIssued}", SinglePassIssued.Id));
            command.Parameters.Add(new MySqlParameter($"@{_namePurposeOfIssuance}", PurposeOfIssuance));
            command.Parameters.Add(new MySqlParameter($"@{_nameAccompanying}", Accompanying.Id));
            command.Parameters.Add(new MySqlParameter($"@{_nameVisitor}", Parent.Id));
        }
        public void Delete()
        {
            using (MySqlCommand command = GetNewcommand())
            {
                command.CommandText = $" DELETE FROM {_nameTable} WHERE {_nameId}=@{_nameId}; ";
                command.Parameters.Add(new MySqlParameter($"@{_nameId}", Id));
                command.ExecuteNonQuery();
            }
            if (Parent.SinglePasses.Contains(Parent.SinglePass))
            {
                Parent.SinglePasses.Remove(Parent.SinglePass);
            }
        }
        public void Cancel()
        {
            SinglePassIssued?.Cancel();
            Accompanying?.Cancel();
            Visitor?.Cancel();
            Id = _initId;
            PassIssued = _initPassIssued;
            ValidUntil = _initValidUntil;
            PurposeOfIssuance = _initPurposeOfIssuance;
        }

        public SinglePass FillThisByOtherSinglePass(SinglePass source)
        {
            this.Accompanying = source.Accompanying;
            this.Id = source.Id;
            this.PassIssued = source.PassIssued;
            this.ValidUntil = source.ValidUntil;
            this.PurposeOfIssuance = source.PurposeOfIssuance;
            this.SinglePassIssued = source.SinglePassIssued;
            return this;
        }

        public void initEditableTable()
        {
            FillThisByOtherSinglePass(Parent.SinglePass);
        }

        public void GetEmptyEditableTable()
        {
            FillThisByOtherSinglePass(new SinglePass(Parent));
            Parent.ShootingPermission = null;
        }

        public void FillBYId(int? id)
        {
            if (id != null)
            {
                FillThisByOtherSinglePass(GetTableById((int)id));
            }
        }

        public void SaveLocal()
        {
            if (Parent.SinglePasses.Contains(Parent.SinglePass))
                Parent.SinglePasses.Remove(Parent.SinglePass);
            Parent.SinglePasses.Add((new SinglePass(Parent).FillThisByOtherSinglePass(this)));
        }

        public bool EmployeeIsSelected(Employee employee)
        {
           if(employee == EditableSinglePassIssued)
            {
                return SinglePassIssued != null;
            }
            if (employee == EditableAccompanying)
            {
                return Accompanying != null;
            }

            return false;
        }


        public void InitEmployees(Employee employee)
        {
            if (employee == EditableSinglePassIssued)
            {
                int? id = SinglePassIssued?.Id;
                SinglePassIssueds.Clear();
                Employee.GetALL().ForEach(o => SinglePassIssueds.Add(o));
                if (id != null)
                {
                    SinglePassIssued = SinglePassIssueds.Where(o => o.Id == id).FirstOrDefault();
                }
            }
            if (employee == EditableAccompanying)
            {
                int? id = Accompanying?.Id;
                Accompanyings.Clear();
                Employee.GetALL().ForEach(o => Accompanyings.Add(o));
                if (id != null)
                {
                    Accompanying = Accompanyings.Where(o => o.Id == id).FirstOrDefault();
                }
            }
         
        }

        public void InitEditableEmployeeTable(Employee employee)
        {
            if (employee == EditableSinglePassIssued)
            {
                if (PassIssued != null)
                {
                    employee.FillBYId((int)SinglePassIssued.Id);
                
}
            }
            if (employee == EditableAccompanying)
            {
                if (PassIssued != null)
                {
                    employee.FillBYId((int)Accompanying.Id);
                }               
            }
        }

        public void GetEmptyEmployeeEditableTable(Employee employee)
        {
            employee.FillByOtherEmployee(new Employee(this));
        }

        public void FillEmployeeById(int? id, Employee employee)
        {
            if (employee == EditableSinglePassIssued)
            {
                SinglePassIssued.FillBYId((int)id);

            }
            if (employee == EditableAccompanying)
            {
                InitEmployees(EditableAccompanying);
                Accompanying.FillBYId((int)id);
                OnPropertyChanged(nameof(Accompanying));
            }
            
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
	<p style=""font-size: 13pt; padding-left: 20pt ""><u>{Parent.Id}</u></p>
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
<p><span >Пропуск выдан</span><span style=""margin-left: 12pt""><u>{PassIssued}</u></span></p>
<p><span >Срок действия до</span><span style=""margin-left: 12pt""><u>{ValidUntil}</u></span></p>
<p><span >Срок действия продлен до</span><span style=""margin-left: 12pt"">____._____.20___ </span></p>
<p><span >Пропуск выдал</span><span style=""margin-left: 12pt"">{SinglePassIssued.Name + " " + SinglePassIssued.LastName}</span></p>
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

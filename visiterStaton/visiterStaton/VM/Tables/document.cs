using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;


namespace visiterStaton
{
    class Document : StationDBContext , IStantionDBTable, IDataErrorInfo
    {
        int? _initId;
        string _initSeries;
        string _initNumber;
        DateTime _initDate_of_issue;
        DocumentType _initDocumentType;
        IssuingAuthority _initIssuing_Authority;
        int? _id;
        string _series;
        string _number;
        DateTime _date_of_issue;
        DocumentType _documentType;

        IssuingAuthority _issuing_Authority;
        IssuingAuthority _editableIssuingAuthority;
        static readonly string _nameTable = "document";

        private DocumentType newdocumentType;

        private ObservableCollection<DocumentType> documentTypes = new ObservableCollection<DocumentType>();
        private ObservableCollection<IssuingAuthority> issuingAuthoritys = new ObservableCollection<IssuingAuthority>();
        public Document()
        {
            _initId = null;
            NewdocumentType = new DocumentType(this);
            EditableIssuingAuthority = new IssuingAuthority(this);
            initDocumentTypes();
            initIssuingAuthority();
        }

        Document(int? initId, string initSeries, string initNumber, DateTime initDate_of_issue, 
            DocumentType initDocumentType, IssuingAuthority initIssuing_Authority)
        {
            NewdocumentType = new DocumentType(this);
            EditableIssuingAuthority = new IssuingAuthority(this);
            Id = _initId = initId;
            Series= _initSeries = initSeries;
            Number= _initNumber = initNumber;
            Date_of_issue= _initDate_of_issue = initDate_of_issue;
            DocumentType=_initDocumentType = initDocumentType;
            IssuingAuthority=_initIssuing_Authority = initIssuing_Authority;
            initDocumentTypes();
            initIssuingAuthority();        }

        static Action<MySqlDataReader, List<object>> elector = (sqlReader, selectedList) =>
        {
            selectedList.Add(new Document(sqlReader.GetInt32("id"), 
                sqlReader.GetString("series"),
                sqlReader.GetString("number"),
                sqlReader.GetDateTime("date_of_issue"),
                DocumentType.GetTableById(sqlReader.GetInt32("document_type")), 
                IssuingAuthority.GetTableById(sqlReader.GetInt32("issuing_Authority"))));
        };


        public int? Id { get => _id; set => _id = value; }
        public string Series { get => _series; 
            set { _series = value; OnPropertyChanged(); } }
        public string Number { get => _number; set { _number = value; OnPropertyChanged(); } }
        public DateTime Date_of_issue { get => _date_of_issue;   set { _date_of_issue = value; OnPropertyChanged(); } }
        public DocumentType DocumentType { get => _documentType;  set { _documentType = value; OnPropertyChanged(); } }
        public IssuingAuthority IssuingAuthority { get => _issuing_Authority;  set { _issuing_Authority = value; OnPropertyChanged(); } }
        public IssuingAuthority EditableIssuingAuthority { get => _editableIssuingAuthority;   set { _editableIssuingAuthority = value; OnPropertyChanged(); } }
        public DocumentType NewdocumentType { get => newdocumentType;  set { newdocumentType = value; OnPropertyChanged(); } }
        public ObservableCollection<DocumentType> DocumentTypes { get => documentTypes; set { documentTypes = value; OnPropertyChanged(); } }
        public ObservableCollection<IssuingAuthority> IssuingAuthoritys { get => issuingAuthoritys; set {issuingAuthoritys = value; OnPropertyChanged(); } }

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsTableSelected => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string NoEmptyItem = "НЕ должно быть пустое значение";

                string error = String.Empty;
                switch (columnName)
                {
                    case "Series":
                        if (string.IsNullOrEmpty(Series))
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "DocumentType":
                        if (DocumentType==null)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "Number":
                        if (string.IsNullOrEmpty(Number))
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "IssuingAuthority":
                        if (IssuingAuthority==null)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                    case "Date_of_issue":
                        if (string.IsNullOrEmpty(Date_of_issue.ToString()) && Date_of_issue == DateTime.MinValue)
                        {
                            error = NoEmptyItem;
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public static Document GetTableById(int id)
        {
            var commandString = $"SELECT * FROM {_nameTable} where id=@id";
            var param = (new MySqlParameter("@id", id));
            var query = new Query(commandString, new List<MySqlParameter> { param }, elector);
            var dockType = GetFromReader(query);
            return dockType.Cast<Document>().FirstOrDefault();
        }
        public static List<Document> GetALL()
        {
            var commandString = $" SELECT * FROM {_nameTable}";
            var query = new Query(commandString, new List<MySqlParameter>(), elector);
            return GetFromReader(query).Cast<Document>().ToList();
        }
        public void Update()
        {
            if (Check())
            {
                DocumentType.Update();
                IssuingAuthority.Update();
                if (_id != null)
                {
                    using (MySqlCommand command = GetNewcommand())
                    {
                        command.CommandText = $" UPDATE {_nameTable} SET series=@series,number=@number, date_of_issue=@Date_of_issue,  " +
                            $"document_type=@documentType, issuing_authority=@Issuing_Authority " +
                            $"WHERE id = @id; ";
                        command.Parameters.Add(new MySqlParameter("@series", Series));
                        command.Parameters.Add(new MySqlParameter("@number", Number));
                        command.Parameters.Add(new MySqlParameter("@date_of_issue", Date_of_issue));
                        command.Parameters.Add(new MySqlParameter("@documentType", DocumentType.Id));
                        command.Parameters.Add(new MySqlParameter("@Issuing_Authority", IssuingAuthority.Id));
                        command.Parameters.Add(new MySqlParameter("@id", Id));
                        command.ExecuteNonQuery();
                        NewInitialisation();
                    }
                }
                if (_id == null)
                {
                    Create();
                    initDocumentTypes();
                }

            }

            else { throw new NotAllDataEnteredInTableException(_nameTable); }
        }

        private void Create()
        {
            using (MySqlCommand command = GetNewcommand())
            {
                command.CommandText = $" INSERT INTO {_nameTable} (series,number,date_of_issue,document_type,issuing_authority) " +
                    $"VALUES(@series,@number,@Date_of_issue,@documentType,@Issuing_Authority);" +
                    $"SELECT * FROM `station`.`{_nameTable}` WHERE id = LAST_INSERT_ID();";
                command.Parameters.Add(new MySqlParameter("@series", Series));
                command.Parameters.Add(new MySqlParameter("@number", Number));
                command.Parameters.Add(new MySqlParameter("@date_of_issue", Date_of_issue));
                command.Parameters.Add(new MySqlParameter("@documentType", DocumentType.Id));
                command.Parameters.Add(new MySqlParameter("@Issuing_Authority", IssuingAuthority.Id));
                _initId = Id = (int)command.ExecuteScalar();

            }
        }

        private void NewInitialisation()
        {
            _initSeries = Series;
            _initNumber = Number;
            _initDate_of_issue = Date_of_issue;
            _initDocumentType = DocumentType;
            _initIssuing_Authority = IssuingAuthority;
        }

        public bool Check()
        {                
            return DoAlldataExist() && DocumentType.Check() && IssuingAuthority.Check(); ;
        }

        private bool DoAlldataExist()
        {
            return !string.IsNullOrEmpty(Series) && !string.IsNullOrEmpty(Number) && Date_of_issue != null &&
                DocumentType != null && IssuingAuthority != null;
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
            Series = _initSeries;
            Number = _initNumber;
            Date_of_issue = _initDate_of_issue;
            DocumentType = _initDocumentType;
            IssuingAuthority = _initIssuing_Authority;

        }

    public void initDocumentTypes()
        {
            int? id = DocumentType?.Id;
            DocumentTypes.Clear();
            DocumentType.GetALL().ForEach(o => DocumentTypes.Add(o));
            if(id!=null)
            {
                DocumentType = DocumentTypes.Where(o => o.Id == id).FirstOrDefault();
            }
        }

        public void initIssuingAuthority()
        {
            int? id = IssuingAuthority?.Id;
            IssuingAuthoritys.Clear();
            IssuingAuthority.GetALL().ForEach(o => IssuingAuthoritys.Add(o));

            if (id != null)
            {
                IssuingAuthority = IssuingAuthoritys.Where(o => { return o.Id == id; }).FirstOrDefault();
            }
        }

        public void initEditableTable()
        {
            throw new NotImplementedException();
        }

        public void GetEmptyEditableTable()
        {
            throw new NotImplementedException();
        }

        public void FillBYId(int? id)
        {
            throw new NotImplementedException();
        }
    }
}

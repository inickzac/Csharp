using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    class Initialization
    {
        public Initialization()
        {
            DbMenager.DropDBStation();
            DbMenager.CreateDb();
            AddSomeDocumentType();
            AddSomeIA();
            AddSomeDocument();
            AddSomeOrganization();
            AddSomeDepartment();
            AddSomeEmployee();
            AddSomeStationFacility();
            AddSomeVister();
            AddSomeShootingPermission();
            AddSomeTP();
            AddSomeSingleP();
        }

        public void AddSomeDocumentType()
        {
            List<DocumentType> documentTypes = new List<DocumentType> {
            new DocumentType() { Name = "Паспорт" },
            new DocumentType() { Name = "Права" },
            new DocumentType() { Name = "Военный билет" }
        };

            documentTypes.ForEach(o => o.Update());

        }
        public void AddSomeIA()
        {
            List<IssuingAuthority> IA = new List<IssuingAuthority> {
            new IssuingAuthority(null) {Name = "РОВД"},
            new IssuingAuthority(null) {Name = "КГБ"},
            new IssuingAuthority(null) {Name = "МВД"}
        };

            IA.ForEach(o => o.Update());

        }

        public void AddSomeDocument()
        {
            var document = new Document();
            document.DocumentType = DocumentType.GetTableById(2);
            document.IssuingAuthority = IssuingAuthority.GetTableById(3);
            document.Series = "AB";
            document.Number = "34567894455688";
            document.Date_of_issue = new DateTime(2020, 08, 08);
            document.Update();

        }
        public void AddSomeOrganization()
        {
            List<Organization> IA = new List<Organization> {
            new Organization(null) {Name = "ооо сапр"},
            new Organization(null) {Name = "оао рол"},
            new Organization(null) {Name = "оло ппп"}
        };

            IA.ForEach(o => o.Update());

        }
        public void AddSomeDepartment()
        {
            List<Department> IA = new List<Department> {
            new Department(null) {Name = "отдел продаж"},
            new Department(null) {Name = "отдел сбыта"},
            new Department(null) {Name = "отдел планирования"}
        };

            IA.ForEach(o => o.Update());

        }
        public void AddSomeEmployee()
        {
            List<Employee> IA = new List<Employee> {
            new Employee(null) {FirstName = "Иван", Department= Department.GetTableById(1), LastName="Иванов", Position="Инженер" },
            new Employee(null) {FirstName = "вася", Department= Department.GetTableById(2), LastName="васильев", Position="халтурщик" },
        };

            IA.ForEach(o => o.Update());

        }
        public void AddSomeShootingPermission()
        {
            List<ShootingPermission> IA = new List<ShootingPermission> {
                new ShootingPermission(null) {CameraType="samsung" , Start= new DateTime(2018,12,23), End  = new DateTime(2020,12,23), ShootingAllowed= Employee.GetTableById(2),
                    SubjectOfShooting = StationFacility.GetTableById(1), ShootingPurpose = "Просто так", parent = Visitor.GetTableById(1) },
                 new ShootingPermission(null) {CameraType="lg" , Start= new DateTime(2015,09,23), End  = new DateTime(2019,12,31), ShootingAllowed= Employee.GetTableById(1),
                    SubjectOfShooting = StationFacility.GetTableById(2), ShootingPurpose = "Шпионские съемки", parent = Visitor.GetTableById(1)},
                  new ShootingPermission(null) {CameraType="nokia" , Start= new DateTime(2020,11,23), End  = new DateTime(2017,12,31), ShootingAllowed= Employee.GetTableById(2),
                    SubjectOfShooting = StationFacility.GetTableById(2), ShootingPurpose = "Проверить камеру", parent = Visitor.GetTableById(1) }
            };

            IA.ForEach(o => o.Update());

        }

        public void AddSomeTP()
        {
            List<TemporarPass> ltp = new List<TemporarPass>() {

            new TemporarPass(null)
            {
                ValidWith = new DateTime(2018, 03, 03),
                ValidUntil = new DateTime(2020, 12, 01),
                PurposeOfIssuance = "Посабирать грибы",
                Parent = Visitor.GetTableById(1),
                PassIssued = Employee.GetTableById(1)
            },
        new TemporarPass(null)
            {
                ValidWith = new DateTime(2018, 03, 03),
                ValidUntil = new DateTime(2020, 12, 01),
                PurposeOfIssuance = "Покармить бабушек",
                Parent = Visitor.GetTableById(1),
                PassIssued = Employee.GetTableById(1)
            }

            };
            ltp.ForEach(o => o.Update());
        }
        
        public void AddSomeSingleP()
        {
            List<SinglePass> sp = new List<SinglePass>()
            {
                new SinglePass(null) {Accompanying = Employee.GetTableById(1),
                SinglePassIssued = Employee.GetTableById(2),
                PassIssued= new DateTime(2020,01,01),
                ValidUntil = new DateTime(2020,02,02),
                PurposeOfIssuance ="Отстрел ежиков",
                Parent = Visitor.GetTableById(1)
                },
             new SinglePass(null) {Accompanying = Employee.GetTableById(1),
                SinglePassIssued = Employee.GetTableById(2),
                PassIssued= new DateTime(2020,01,01),
                ValidUntil = new DateTime(2020,02,02),
                PurposeOfIssuance ="Поиск приключений",
                Parent = Visitor.GetTableById(1)
             }
            };
           sp.ForEach(o => o.Update());
        }
        
        public void AddSomeStationFacility()
        {
            List<StationFacility> IA = new List<StationFacility>
            {
                 new StationFacility(null) {Name="ТУАЛЕТ" },
                 new StationFacility(null) {Name="насосная" },
                 new StationFacility(null) {Name="аппаратная" },
            };

            IA.ForEach(o => o.Update());

        }
        public void AddSomeVister()
        {
            List<Visitor> IA = new List<Visitor>
            {
                 new Visitor(new VM.MainWindowViewModel())
                 {   Name="Ванька" ,
                     LastName= "Огнев",
                     Document=  Document.GetTableById(1),
                     Organization = Organization.GetTableById(2),
                     Position = "Замиститель главного бездельника"
                 },
              new Visitor(new VM.MainWindowViewModel())
                 {   Name="Петька" ,
                     LastName= "Огнев",
                     Document=  Document.GetTableById(1),
                     Organization = Organization.GetTableById(1),
                     Position = "главный бездельник"
                 },

            };

            IA.ForEach(o => o.Update());

        }
    }


}

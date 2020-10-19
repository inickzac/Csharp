using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    interface IUpdateEmployee
    {
        bool EmployeeIsSelected(Employee employee);
        void InitEmployees(Employee employee);
        void InitEditableEmployeeTable(Employee employee);
        void GetEmptyEmployeeEditableTable(Employee employee);
        void FillEmployeeById(int? id, Employee employee);
    }
}

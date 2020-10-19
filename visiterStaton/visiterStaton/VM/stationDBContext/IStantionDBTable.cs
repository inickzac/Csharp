using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton
{
    interface IStantionDBTable
    {
        string Name { get; set; }
        bool IsTableSelected { get; }
        int? Id { get; }
        void initEditableTable();       
        void GetEmptyEditableTable();
        void Update();
        bool Check();
        void Delete();
        void Cancel();
        void FillBYId(int? id);
       
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap
{
   public class MenuItem
    {            
            public MenuItem()
            {
                this.Items = new ObservableCollection<MenuItem>();
               AnswersHeader = new List<string>();
            }
            public string Title { get; set; }
            public ObservableCollection<MenuItem> Items { get; set; }
            public List<string> AnswersHeader { get; set; }
            public string MessageText { get; set; }
            public string MessageHeader { get; set; }

    }
}

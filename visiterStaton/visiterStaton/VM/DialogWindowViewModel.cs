using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton.VM
{
    class DialogWindowViewModel : VMContext
    {
        private string _text;
        private string _header;
        private string _background;

        public DialogWindowViewModel()
        {
        }

        public DialogWindowViewModel(string text, string header, string background)
        {
            _text = text;
            _header = header;
            _background = background;
        }

        public string Text { get => _text; set { _text = value; OnPropertyChanged(); } }

        public string Header { get => _header; set { _header = value; OnPropertyChanged(); } }

        public string Background { get => _background; set { _background = value; OnPropertyChanged(); } }
    }
}

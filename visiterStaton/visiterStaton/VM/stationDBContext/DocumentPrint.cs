using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visiterStaton.VM.stationDBContext
{
    class DocumentPrint : VMContext
    {

        private IDocument document;
        public DocumentPrint(IDocument document)
        {
            this.document = document;

        }

        public string Html { get => document.GetHtml(); }
    }
}

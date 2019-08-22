using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTunnel
{
    class ComboItem
    {
        public ComboItem(string uid, string text)
        {
            UID = uid;
            Text = text;
        }

        public string UID
        { get; set; }

        public string Text
        { get; set; }

        public override string ToString()
        { return Text; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class ModbusSlave
    {
        public ModbusSlave() { }

        public string Name { get; set; }

        public ushort StartAdress { get; set; }
    }
}

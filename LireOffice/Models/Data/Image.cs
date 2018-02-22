using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class Image : EntityData
    {
        public string Name { get; set; }
        public byte[] Source { get; set; }
    }
}

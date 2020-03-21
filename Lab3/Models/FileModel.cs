using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models
{
    public class FileModel
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public double RazonDeComprecion { get; set; }
        public double FactorDeComprecion { get; set; }
        public double PorcentajeDeReduccion { get; set; }

    }
}

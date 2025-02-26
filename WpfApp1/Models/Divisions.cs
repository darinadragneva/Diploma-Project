using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    public class Divisions
    {
        public int Id { get; set; }
        public string DivisionName { get; set; }
        public double Total { get; set; }
        public double Interest { get; set; }
    }
}

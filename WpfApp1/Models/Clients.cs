using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    public class Clients
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public double CurrentCapital { get; set; }
        public double AutoPay { get; set; }
        public int DivisionId { get; set; }

        public Divisions Division { get; set; }
    }
}

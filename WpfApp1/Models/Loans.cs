using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    public class Loans
    {
        public int Id { get; set; }
        public int LoanNumber { get; set; }
        public DateTime LoanDate { get; set; }
        public double LoanSum { get; set; }
        public double CurrentInterest { get; set; }
        public double RealLoan { get; set; }
        public double LoanRemainder { get; set; }
        public int LoanMonths { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }

        public Clients Client { get; set; }

        public LoanStatus Status { get; set; }
    }
}
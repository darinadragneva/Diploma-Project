using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    public class Payments
    {
        public int Id { get; set; }
        public int OperationNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public double CapitalPay { get; set; }
        public double LoanPay { get; set; }
        public double PaymentCapital { get; set; }
        public string DocumentNumber { get; set; }
        public int ClientId { get; set; }
        public int TypeOperationId { get; set; }
        public int UserId { get; set; }

        public Clients Client { get; set; }

        public TypeOperations TypeOperation { get; set; }    

        public Users User { get; set; }
    }
}

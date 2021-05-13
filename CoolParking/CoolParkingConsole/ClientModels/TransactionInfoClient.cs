using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolParkingConsole.ClientModels
{
    public class TransactionInfoClient
    {
        public decimal Sum { get; set; }
        public string VehicleId { get; set; }
        public DateTime TimeTransaction { get; set; }
    }
}

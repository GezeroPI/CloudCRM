using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCRM.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Profession { get; set; }
        public int Vat { get; set; }
        public string TaxOffice { get; set; } //DOY
    }
}

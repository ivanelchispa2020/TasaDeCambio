using System;
using System.Collections.Generic;
using System.Text;

namespace TasaDeCambio.Models
{
   public class Tasa
    {

        public int RateId { get; set; }

        public string Code { get; set; }

        public double TaxRate { get; set; }

        public string Name { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs
{
    public class SaldoCuentaDto
    {
        public int CuentaId { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Saldo {  get; set; }
    }
}

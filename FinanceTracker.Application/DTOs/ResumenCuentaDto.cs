using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs
{
    public class ResumenCuentaDto
    {
        public int CuentaId { get; set; }
        public string NombreCuenta { get; set; } = string.Empty;
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Saldo => TotalIngresos - TotalGastos;
    }
}

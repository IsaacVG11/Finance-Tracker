using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs
{
    public class ResumenFinancieroUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal SaldoTotal {  get; set; }
        public IEnumerable<ResumenCuentaDto> Cuentas { get; set; } = [];
    }
}

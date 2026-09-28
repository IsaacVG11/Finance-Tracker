namespace FinanceTracker.Web.Models
{
    public class ResumenCuentaDto
    {
        public int CuentaId { get; set; }
        public string NombreCuenta { get; set; } = string.Empty;
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Saldo { get; set; }
    }
}

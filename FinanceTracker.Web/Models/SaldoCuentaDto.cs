namespace FinanceTracker.Web.Models
{
    public class SaldoCuentaDto
    {
        public int CuentaId { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Saldo { get; set; }
    }
}

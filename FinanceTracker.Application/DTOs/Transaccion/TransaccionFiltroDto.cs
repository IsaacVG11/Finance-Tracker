using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Application.DTOs.Transaccion
{
    public class TransaccionFiltroDto
    {
        public DateOnly? Desde { get; set; }
        public DateOnly? Hasta { get; set; }
        public TipoTransaccion? Tipo { get; set; }
        public int? CategoriaId { get; set; }
        public EstadoTransaccion? Estado { get; set; }
    }
}

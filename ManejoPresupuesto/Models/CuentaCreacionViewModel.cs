using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class CuentaCreacionViewModel: Cuenta
    {
        private IEnumerable<SelectListItem> myProperty;

        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}

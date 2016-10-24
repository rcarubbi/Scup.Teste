using ScupTel.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ScupTel.WebUI.Models
{
    public class QuoteViewModel 
    {
        public QuoteViewModel()
        {

        }
        public QuoteViewModel(List<Zone> zones)
        {
            _zones = zones;
        }

        private readonly List<Zone> _zones;
        public IEnumerable<SelectListItem> Zones
        {
            get { return new SelectList(_zones, "Code", "Name"); }
        }

        [Display(Name = "De")]
        [Required(ErrorMessage = "Selecione a origem da chamada")]
        public int SelectedSourceCode { get; set; }


        [Display(Name = "Para")]
        [Required(ErrorMessage = "Selecione o destino da chamada")]
        public int SelectedTargetCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe a duração da chamada em minutos")]
        [Display(Name = "Duração em minutos")]
        public int Minutes { get; set; }

       
    }
}

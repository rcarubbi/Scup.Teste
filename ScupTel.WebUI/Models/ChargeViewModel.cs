using ScupTel.DatatablesJS;
using ScupTel.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ScupTel.WebUI.Models
{
    public class ChargeViewModel
    {

        public ChargeViewModel()
        {

        }

        public ChargeViewModel(List<Zone> zones)
        {
            _zones = zones;
        }

        [DataTablesColumn(Order = 1, Header = "Origem", SortMap = "From.Name")]
        public string FromName { get; set; }

        [DataTablesColumn(Order = 2, Header = "Destino", SortMap = "To.Name")]
        public string ToName { get; set; }

        [Display(Name ="Origem")]
        public int SelectedSourceZoneCode { get; set; }

        [Display(Name = "Destino")]
        public int SelectedTargetZoneCode { get; set; }

        [DataTablesColumn(PrimaryKey = true, Hidden = true)]
        public int? Id { get; set; }


        [DataTablesColumn(Order = 3, Header = "Valor/Minuto", DataType = DataType.Currency)]
        [Display(Name = "Valor/Minuto")]
        [DataType(DataType.Currency, ErrorMessage = "Digite um valor monetário válido")]
        public decimal MinutePrice { get; set; }

        internal static ChargeViewModel FromEntity(Charge charge)
        {
            return FromEntity(charge, null);
        }
        internal static ChargeViewModel FromEntity(Charge charge, List<Zone> zones)
        {
            return new ChargeViewModel(zones)
            {
                FromName = charge.From.Name,
                ToName = charge.To.Name,
                SelectedSourceZoneCode = charge.From.Code,
                SelectedTargetZoneCode = charge.To.Code,
                Id = charge.Id,
                MinutePrice = charge.MinutePrice,
            };
        }

        internal static Charge ToEntity(ChargeViewModel viewModel, Zone from, Zone to, Charge currentCharge)
        {
            currentCharge.From = from;
            currentCharge.To = to;
            currentCharge.MinutePrice = viewModel.MinutePrice;
            return currentCharge;
        }

        internal static Charge ToEntity(ChargeViewModel viewModel, Zone from, Zone to)
        {
            return new Charge(from, to, viewModel.MinutePrice);
        }

        internal static IEnumerable<ChargeViewModel> FromEntityCollection(IEnumerable<Charge> entities)
        {
            foreach (var item in entities)
            {
                yield return ChargeViewModel.FromEntity(item);
            }
        }

        private readonly List<Zone> _zones;
        public IEnumerable<SelectListItem> Zones
        {
            get { return new SelectList(_zones ?? new List<Zone>(), "Code", "Name"); }
        }

    }
}

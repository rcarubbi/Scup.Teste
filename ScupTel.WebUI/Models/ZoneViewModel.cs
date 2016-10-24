using ScupTel.DatatablesJS;
using ScupTel.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScupTel.WebUI.Models
{
    public class ZoneViewModel
    {
        [Display(Name = "Nome")]
        [DataTablesColumn(Order = 1, Header = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Código")]
        [DataTablesColumn(Order = 2, Header = "Código")]
        public int Code { get; set; }

        [DataTablesColumn(PrimaryKey = true, Hidden = true)]
        public int? Id { get; set; }

        public static ZoneViewModel FromEntity(Zone entity)
        {
            return new ZoneViewModel
            {
                Code = entity.Code,
                Name = entity.Name,
                Id = entity.Id
            };
        }

        public static Zone ToEntity(ZoneViewModel viewModel)
        {
            return new Zone(viewModel.Name, viewModel.Code);
        }

        public static Zone ToEntity(ZoneViewModel viewModel, Zone zone)
        {
            zone.Code = viewModel.Code;
            zone.Name = viewModel.Name;
            return zone;
        }

        public static IEnumerable<ZoneViewModel> FromEntityCollection(IEnumerable<Zone> entities)
        {
            foreach (var item in entities)
            {
                yield return FromEntity(item);
            }
        }
    }
}

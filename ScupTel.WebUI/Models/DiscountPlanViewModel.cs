using System;
using System.Collections;
using System.Collections.Generic;
using ScupTel.DatatablesJS;
using ScupTel.Domain;

namespace ScupTel.WebUI.Models
{
    public class DiscountPlanViewModel
    {
        [DataTablesColumn(PrimaryKey = true, Hidden = true)]
        public int Id { get; set; }

        [DataTablesColumn(Order = 1, Header = "Nome")]
        public string Name { get; set; }

        [DataTablesColumn(Order = 2, Header = "Minutos disponíveis")]
        public int FreeMinutes { get; set; }

        internal static IEnumerable<DiscountPlanViewModel> FromEntityCollection(IEnumerable<DiscountPlan> entities)
        {
            foreach (var item in entities)
            {
                yield return FromEntity(item);
            }
        }

        private static DiscountPlanViewModel FromEntity(DiscountPlan plan)
        {
            return new DiscountPlanViewModel
            {
                Id = plan.Id,
                FreeMinutes = plan.FreeMinutes,
                Name = plan.Name
            };
        }
    }
}

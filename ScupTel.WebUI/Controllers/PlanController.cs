using DataTables.Mvc;
using ScupTel.DatatablesJS;
using ScupTel.Domain;
using ScupTel.Domain.Repositories.PagedResultsGenericRepository;
using ScupTel.WebUI.Models;
using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public class PlanController : BaseController
    {
        public PlanController(IDbContext context)
            : base(context)
        {

        }
        // GET: Charge
        public ActionResult Index()
        {
            ViewBag.Title = "Planos";
            ViewBag.SubTitle = "Consulta de Planos";
            return View();
        }

        public JsonResult Plans([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var repo = new GenericRepository<DiscountPlan>(_context);
            var query = new SearchQuery<DiscountPlan>();

            var sortedColumns = requestModel
                .Columns
                .GetSortedColumns()
                .ToDynamicExpression<DiscountPlanViewModel>();

            if (sortedColumns.Length > 0)
            {
                query.AddSortCriteria(new DynamicFieldSortCriteria<DiscountPlan>(sortedColumns));
            }

            query.Take = requestModel.Length;
            query.Skip = requestModel.Start;

            var result = repo.Search(query);

            return Json(new DataTablesResponse(requestModel.Draw, DiscountPlanViewModel.FromEntityCollection(result.Entities), result.Count, result.Count));
        }
    }
}

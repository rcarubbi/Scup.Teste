using DataTables.Mvc;
using ScupTel.DatatablesJS;
using ScupTel.Domain;
using ScupTel.Domain.Repositories.PagedResultsGenericRepository;
using ScupTel.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public class ChargeController : BaseController
    {
        public ChargeController(IDbContext context)
            : base(context)
        {

        }
        // GET: Charge
        public ActionResult Index()
        {
            ViewBag.Title = "Tarifas";
            ViewBag.SubTitle = "Manutenção de Tarifas";
            return View();
        }

        public ActionResult Edit(int? id)
        {
            ChargeViewModel viewModel = new ChargeViewModel(_context.Zones.ToList());

            if (id.HasValue)
            {
                var charge = _context.Charges.Find(id);
                viewModel = ChargeViewModel.FromEntity(charge, _context.Zones.ToList());
            }

            ModalFormViewModel modalViewModel = new ModalFormViewModel
            {
                Id = "EditChargeModalForm",
                PartialViewName = "_EditCharge",
                Title = string.Format("{0} de Tarifa", id.HasValue ? "Alteração" : "Inclusão"),
                ViewModel = viewModel
            };
            return PartialView("_ModalFormDialog", modalViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ChargeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Zone from = _context.Zones.First(x => x.Code == viewModel.SelectedSourceZoneCode);
                Zone to = _context.Zones.First(x => x.Code == viewModel.SelectedTargetZoneCode);

                if (viewModel.Id.HasValue)
                {
                    var currentCharge = _context.Charges.Find(viewModel.Id);
                    var updatedCharge = ChargeViewModel.ToEntity(viewModel, from, to, currentCharge);

                    _context.Update(currentCharge, updatedCharge);
                    _context.SaveChanges();
                }
                else
                {
                    var postedCharge = ChargeViewModel.ToEntity(viewModel, from, to);

                    _context.Charges.Add(postedCharge);
                    _context.SaveChanges();
                }

                return PartialView("_chargeGrid");
            }
            else
            {
                return PartialView("_validationSummary", viewModel);
            }
        }

        public ActionResult Delete(int id)
        {
            ChargeViewModel viewModel = new ChargeViewModel();

            Charge charge = _context.Charges.Find(id);

            viewModel = ChargeViewModel.FromEntity(charge);

            ModalQuestionViewModel modalViewModel = new ModalQuestionViewModel
            {
                Id = "DeleteChargeModal",
                Title = "Exclusão de Tarifa",
                Body = string.Format("Tem certeza que deseja excluir a tarifa de {0} para {1}?", viewModel.FromName, viewModel.ToName),
                YesButtonAction = Url.Action("Delete", "Charge"),
                CloseNoButton = true,
                KeyProperties = string.Format("Id={0}", id),
                YesButtonUpdateContainerId = "charge-grid-container",
                YesButtonNotification = new NotificationViewModel
                {
                    Type = NotificationType.Error,
                    Message = "Tarifa excluída com sucesso!",
                    Title = "Exclusão de Tarifa"
                }
            };
            return PartialView("_ModalQuestionDialog", modalViewModel);
        }

        [HttpPost]
        public ActionResult Delete(ChargeViewModel viewModel)
        {
            var charge = _context.Charges.Find(viewModel.Id.Value);

            _context.Charges.Remove(charge);
            _context.SaveChanges();

            return PartialView("_chargeGrid");
        }

        public JsonResult Charges([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var repo = new GenericRepository<Charge>(_context);
            var query = new SearchQuery<Charge>();

            var sortedColumns = requestModel
                .Columns
                .GetSortedColumns()
                .ToDynamicExpression<ChargeViewModel>();

            if (sortedColumns.Length > 0)
            {
                query.AddSortCriteria(new DynamicFieldSortCriteria<Charge>(sortedColumns));
            }

            query.Take = requestModel.Length;
            query.Skip = requestModel.Start;

            var result = repo.Search(query);

            return Json(new DataTablesResponse(requestModel.Draw, ChargeViewModel.FromEntityCollection(result.Entities), result.Count, result.Count));
        }
    }
}
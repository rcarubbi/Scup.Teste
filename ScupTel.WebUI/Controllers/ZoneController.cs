using DataTables.Mvc;
using ScupTel.DatatablesJS;
using ScupTel.Domain;
using ScupTel.Domain.Repositories.PagedResultsGenericRepository;
using ScupTel.WebUI.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public class ZoneController : BaseController
    {
        public ZoneController(IDbContext context)
              : base(context)
        {

        }

        // GET: Zone
        public ActionResult Index()
        {
            ViewBag.Title = "Regiões";
            ViewBag.SubTitle = "Manutenção de Regiões";

            return View();
        }

        public ActionResult Edit(int? id)
        {
            ZoneViewModel viewModel = new ZoneViewModel();

            if (id.HasValue)
            {
                var zone = _context.Zones.Find(id);
                viewModel = ZoneViewModel.FromEntity(zone);
            }

            ModalFormViewModel modalViewModel = new ModalFormViewModel
            {
                Id = "EditZoneModalForm",
                PartialViewName = "_EditZone",
                Title = string.Format("{0} de Região", id.HasValue ? "Alteração" : "Inclusão"),
                ViewModel = viewModel
            };
            return PartialView("_ModalFormDialog", modalViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ZoneViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
            
                if (viewModel.Id.HasValue)
                {
                    var currentZone = _context.Zones.Find(viewModel.Id);
                    var updatedZone = ZoneViewModel.ToEntity(viewModel, currentZone);

                    _context.Update(currentZone, updatedZone);
                    _context.SaveChanges();
                }
                else
                {
                    var postedZone = ZoneViewModel.ToEntity(viewModel);

                    _context.Zones.Add(postedZone);
                    _context.SaveChanges();
                }

                return PartialView("_zoneGrid");
            }
            else
            {
                return PartialView("_validationSummary", viewModel);
            }
        }

        public ActionResult Delete(int id)
        {
            ZoneViewModel viewModel = new ZoneViewModel();

            Zone zone = _context.Zones.Find(id);

            viewModel = ZoneViewModel.FromEntity(zone);

            ModalQuestionViewModel modalViewModel = new ModalQuestionViewModel
            {
                Id = "DeleteZoneModal",
                Title = "Exclusão de Região",
                Body = string.Format("Tem certeza que deseja excluir a região {0}?", viewModel.Name),
                YesButtonAction = Url.Action("Delete", "Zone"),
                CloseNoButton = true,
                KeyProperties = string.Format("Id={0}", id),
                YesButtonUpdateContainerId = "zone-grid-container",
                YesButtonNotification = new NotificationViewModel
                {
                    Type = NotificationType.Error,
                    Message = "Região excluída com sucesso!",
                    Title = "Exclusão de Região"
                }
            };
            return PartialView("_ModalQuestionDialog", modalViewModel);
        }

        [HttpPost]
        public ActionResult Delete(ZoneViewModel viewModel)
        {
            var zone = _context.Zones.Find(viewModel.Id.Value);

            _context.Zones.Remove(zone);
            try
            { 
                 _context.SaveChanges();
            }
            catch(DbUpdateException)
            {
                throw new ApplicationException("Esta região não pode ser excluída pois possui tarifas associadas a ela");
            }
         
            return PartialView("_zoneGrid");
        }

        public JsonResult Zones([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var repo = new GenericRepository<Zone>(_context);
            var query = new SearchQuery<Zone>();

            var sortedColumns = requestModel
                .Columns
                .GetSortedColumns()
                .ToDynamicExpression<ZoneViewModel>();

            if (sortedColumns.Length > 0)
            {
                query.AddSortCriteria(new DynamicFieldSortCriteria<Zone>(sortedColumns));
            }

            query.Take = requestModel.Length;
            query.Skip = requestModel.Start;

            var result = repo.Search(query);

            return Json(new DataTablesResponse(requestModel.Draw, ZoneViewModel.FromEntityCollection(result.Entities), result.Count, result.Count));
        }

    }
}
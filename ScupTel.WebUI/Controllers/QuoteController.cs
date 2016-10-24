using ScupTel.Domain;
using ScupTel.Domain.Repositories;
using ScupTel.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public class QuoteController : BaseController
    {
        public QuoteController(IDbContext context)
              : base(context)
        {

        }
        // GET: Quote
        public ActionResult Index()
        {
            ViewBag.Title = "Cotações";
            ViewBag.SubTitle = "Simule agora sua chamada";
            var viewModel = new QuoteViewModel(_context.Zones.ToList());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult QuoteCall(QuoteViewModel viewModel)
        {
            ChargeRepository repo = new ChargeRepository(_context);
            var charge = repo.FindByZones(viewModel.SelectedSourceCode, viewModel.SelectedTargetCode);
            Call call = new Call(charge, viewModel.Minutes);
            var callQuotes = call.Quote(_context.DiscountPlans.ToList());
            return PartialView("_quotesResult", callQuotes);
        }
    }
}
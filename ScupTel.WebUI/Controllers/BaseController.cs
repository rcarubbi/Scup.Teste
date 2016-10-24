using ScupTel.Domain;
using System.Web.Mvc;

namespace ScupTel.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IDbContext _context;

        public BaseController(IDbContext context)
        {
            _context = context;
        }
    }
}
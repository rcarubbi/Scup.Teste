using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScupTel.WebUI.Controllers;
using System.Web.Mvc;

namespace ScupTel.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
 
    }
}

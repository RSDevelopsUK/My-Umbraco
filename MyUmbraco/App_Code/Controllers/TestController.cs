using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class TestController : SurfaceController
  {
    [HttpPost]
    public ActionResult TestAnyMethod(string source, string[] values)
    {
      var test = values.Any(source.ToLower().Contains);
      return View();
    }
  }
}
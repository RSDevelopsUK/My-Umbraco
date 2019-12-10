using System.Web.Mvc;
using MyUmbraco.Filters;
using MyUmbraco.ViewModels;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class ContactFormController : SurfaceController
  {
    [HttpGet]
    [TranslateAttributeFilter]
    public ActionResult Index()
    {
      return PartialView("/Views/Partials/_ContactForm.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Submit(ContactFormViewModel model)
    {
      if (!ModelState.IsValid)
        return CurrentUmbracoPage();

      // Work with form data here

      return RedirectToCurrentUmbracoPage();
    }
  }
}
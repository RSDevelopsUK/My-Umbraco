using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class RegisterController : SurfaceController
  {
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public ActionResult HandleRegisterMember([Bind(Prefix = "registerModel")] RegisterModel model)
    {
      if (!ModelState.IsValid)
        return CurrentUmbracoPage();

      var firstName = model.MemberProperties.Find(x => x.Alias == "firstName").Value;
      var lastName = model.MemberProperties.Find(x => x.Alias == "lastName").Value;
      model.Name = $"{firstName} {lastName}";

      MembershipCreateStatus status;
      var rm = Members.CreateRegistrationModel("MyUmbracoMember");
      rm.MemberProperties = model.MemberProperties;

      Members.RegisterMember(model, out status, model.LoginOnSuccess);
      switch (status)
      {
        case MembershipCreateStatus.Success:
          this.TempData["FormSuccess"] = (object)true;
          if (!model.RedirectUrl.IsNullOrWhiteSpace())
            return (ActionResult)this.Redirect(model.RedirectUrl);
          return (ActionResult)this.RedirectToCurrentUmbracoPage();
        case MembershipCreateStatus.InvalidUserName:
          this.ModelState.AddModelError(model.UsernameIsEmail || model.Username == null ? "registerModel.Email" : "registerModel.Username", "Username is not valid");
          break;
        case MembershipCreateStatus.InvalidPassword:
          this.ModelState.AddModelError("registerModel.Password", "The password is not strong enough");
          break;
        case MembershipCreateStatus.InvalidQuestion:
        case MembershipCreateStatus.InvalidAnswer:
          throw new NotImplementedException(status.ToString());
        case MembershipCreateStatus.InvalidEmail:
          this.ModelState.AddModelError("registerModel.Email", "Email is invalid");
          break;
        case MembershipCreateStatus.DuplicateUserName:
          this.ModelState.AddModelError(model.UsernameIsEmail || model.Username == null ? "registerModel.Email" : "registerModel.Username", Umbraco.GetDictionaryValue("Validation.Email.InUse"));
          break;
        case MembershipCreateStatus.DuplicateEmail:
          this.ModelState.AddModelError("registerModel.Email", Umbraco.GetDictionaryValue("Validation.Email.InUse"));
          break;
        case MembershipCreateStatus.UserRejected:
        case MembershipCreateStatus.InvalidProviderUserKey:
        case MembershipCreateStatus.DuplicateProviderUserKey:
        case MembershipCreateStatus.ProviderError:
          this.ModelState.AddModelError("registerModel", "An error occurred creating the member: " + (object)status);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return (ActionResult)this.CurrentUmbracoPage();
    }
  }
}

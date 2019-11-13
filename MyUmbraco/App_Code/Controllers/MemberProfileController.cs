using System;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  [MemberAuthorize]
  public class MemberProfileController : SurfaceController
  {
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public ActionResult HandleUpdateProfile([Bind(Prefix = "profileModel")] ProfileModel model)
    {
      //if (!Umbraco.Core.Security.MembershipProviderExtensions.GetMembersMembershipProvider().IsUmbracoMembershipProvider())
      //  throw new NotSupportedException("Profile editing with the " + (object)typeof(UmbProfileController) + " is not supported when not using the default Umbraco membership provider");
      if (!ModelState.IsValid)
        return CurrentUmbracoPage();

      var avatarUpload = WebImage.GetImageFromRequest();
      // Check to see if the user has uploaded a new Avatar
      if (avatarUpload != null)
      {
        // Member avatar
        var mediaservices = Services.MediaService;
        var avatarsFolder = mediaservices.GetRootMedia().FirstOrDefault(x => x.Name.InvariantEquals("Member Avatars"));
        var member = Members.GetCurrentMember();
        var memberName = member.Name.Replace(" ", "-");
        var memberId = member.Id;
        var memberFolderName = $"Avatar-{memberName}-{memberId}";
        var memberImageName = $"Avatar-{memberName}";

        // Check Member Avatars folder exists
        if (avatarsFolder != null)
        {
          var memberFolderExists = Umbraco.Media(avatarsFolder.Id).Children().FirstOrDefault(x => x.Name.InvariantEquals(memberFolderName));
          Guid parentGuid;
          int parentId;
          if (memberFolderExists == null)
          {
            // Create member folder
            var memberFolder = mediaservices.CreateMedia(memberFolderName, avatarsFolder.Key, "Folder");
            mediaservices.Save(memberFolder);
            parentGuid = memberFolder.Key;
            parentId = memberFolder.Id;
          }
          else
          {
            parentGuid = memberFolderExists.Key;
            parentId = memberFolderExists.Id;
          }
          // Check to see if the avatar already exists
          var avatarExists = Umbraco.Media(parentId).Children().FirstOrDefault(x => x.Name.InvariantEquals(memberImageName));
          if (avatarExists != null)
          {
            var avatarToDelete = mediaservices.GetById(avatarExists.Id);
            mediaservices.Delete(avatarToDelete);
          }

          // Create image
          var avatar = mediaservices.CreateMedia(memberImageName, parentGuid, "Image");
          var contentTypeBaseServiceProvider = Services.ContentTypeBaseServices;

          var bytes = avatarUpload.GetBytes();
          var ms = new MemoryStream(bytes);

          using (var stream = ms)
          {
            avatar.SetValue(contentTypeBaseServiceProvider, "umbracoFile", memberFolderName + "." + avatarUpload.ImageFormat, stream);
          }
          mediaservices.Save(avatar);
          var avatarUrl = Umbraco.Media(parentId).Children().FirstOrDefault(x => x.Name.InvariantEquals(memberImageName));
          model.MemberProperties[2].Value = avatarUrl?.Url;
        }
      }
      else
      {
        // Populate the member props with the url of the avatar
        var member = Members.GetCurrentMemberProfileModel();
        var avatar = member.MemberProperties[2].Value;
        model.MemberProperties[2].Value = avatar;
      }

      // Update profile
      var attempt = Members.UpdateMemberProfile(model);
      if (!attempt.Success)
      {
        ModelState.AddModelError("profileModel", attempt.Exception.Message);
        return CurrentUmbracoPage();
      }
      TempData["ProfileUpdateSuccess"] = true;
      if (!model.RedirectUrl.IsNullOrWhiteSpace())
        return Redirect(model.RedirectUrl);
      return RedirectToCurrentUmbracoPage();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public ActionResult DeleteAvatar(string defaultAvatar)
    {
      var model = Members.GetCurrentMemberProfileModel();
      model.MemberProperties[2].Value = defaultAvatar;

      // Update profile
      var attempt = Members.UpdateMemberProfile(model);
      if (!attempt.Success)
      {
        ModelState.AddModelError("profileModel", attempt.Exception.Message);
        return CurrentUmbracoPage();
      }
      TempData["ProfileUpdateSuccess"] = true;
      if (!model.RedirectUrl.IsNullOrWhiteSpace())
        return Redirect(model.RedirectUrl);

      return RedirectToCurrentUmbracoPage();
    }
  }
}

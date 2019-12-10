using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyUmbraco.DataAnnotations;
using Umbraco.Core.Services;

namespace MyUmbraco.ViewModels
{
  public class BlogPostAddCommentViewModel
  {
    public int BlogPostId { get; set; }

    [UmbracoRequired("Validation.Required")]
    [AllowHtml]
    public string Comment { get; set; }
  }
}
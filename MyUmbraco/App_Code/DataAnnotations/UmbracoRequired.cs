using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using MyUmbraco.Helpers;

namespace MyUmbraco.DataAnnotations
{
  
  public class UmbracoRequiredAttribute : RequiredAttribute, IClientValidatable
  {
    private readonly ILanguageHelper _langHelper;

    private readonly string Key;

    public UmbracoRequiredAttribute(string key) : this(LanguageHelper.Instance)
    {
      Key = key;
    }

    public UmbracoRequiredAttribute(ILanguageHelper langHelper)
    {
      _langHelper = langHelper;
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
      var test = _langHelper.GetTranslatedKey(Key, "required");
      yield return new ModelClientValidationRequiredRule(ErrorMessage);
    }
  }
}
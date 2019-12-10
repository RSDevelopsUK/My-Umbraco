using System.Linq;
using Umbraco.Core.Services;

namespace MyUmbraco.Helpers
{
  public interface ILanguageHelper
  {
    string GetTranslatedKey(string key, string fallback);
  }

  public class LanguageHelper : ILanguageHelper
  {
    static LanguageHelper()
    {
      Instance = new LanguageHelper();
    }

    public LanguageHelper()
    {
    }

    public static ILanguageHelper Instance { get; }

    private readonly ILocalizationService _localizationService;
    //public LanguageHelper(ILocalizationService localizationService)
    //{
    //  _localizationService = localizationService;
    //}

    public string GetTranslatedKey(string key, string fallback)
    {
      var defaultLanguageId = _localizationService.GetDefaultLanguageId();
      var translatedValue = _localizationService.GetDictionaryItemByKey(key).Translations.FirstOrDefault(x => x.LanguageId == defaultLanguageId)?.Value;

      return !string.IsNullOrEmpty(translatedValue) ? translatedValue : fallback;
    }
  }
}
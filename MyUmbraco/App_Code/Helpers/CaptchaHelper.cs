using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Services;

namespace MyUmbraco.Helpers
{
  public class CaptchaHelper
  {
    public static bool ReCaptchaPassed(string gRecaptchaResponse)
    {
      var httpClient = new HttpClient();
      var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LcwwcIUAAAAAI7ReTbsIYQ_h96ZxoWAzrn9uZVR&response={gRecaptchaResponse}").Result;
      if (res.StatusCode != HttpStatusCode.OK)
        return false;

      var JSONres = res.Content.ReadAsStringAsync().Result;
      dynamic JSONdata = JObject.Parse(JSONres);
      if (JSONdata.success != "true")
        return false;

      return true;
    }
  }
}
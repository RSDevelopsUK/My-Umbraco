namespace MyUmbraco.Interfaces
{
  public interface IUmbracoValidationAttribute
  {
    string DictionaryKey { get; set; }
    string FallBack { get; set; }
  }
}
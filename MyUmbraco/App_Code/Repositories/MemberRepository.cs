using MyUmbraco.Models;
using MyUmbraco.ViewModels;
using Umbraco.Core.Scoping;

namespace MyUmbraco.Repositories
{
  public class MemberRepository
  {
    private readonly IScopeProvider _scopeProvider;

    public MemberRepository(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }
   
  }
}
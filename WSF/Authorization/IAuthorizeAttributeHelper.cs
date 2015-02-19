using System.Collections.Generic;
using System.Threading.Tasks;

namespace WSF.Authorization
{
    public interface IAuthorizeAttributeHelper
    {
        Task AuthorizeAsync(IEnumerable<IWSFAuthorizeAttribute> authorizeAttributes);
        
        Task AuthorizeAsync(IWSFAuthorizeAttribute authorizeAttribute);
        
        void Authorize(IEnumerable<IWSFAuthorizeAttribute> authorizeAttributes);
        
        void Authorize(IWSFAuthorizeAttribute authorizeAttribute);
    }
}
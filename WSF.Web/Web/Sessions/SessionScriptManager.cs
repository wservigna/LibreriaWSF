using System;
using System.Text;
using WSF.Dependency;
using WSF.Runtime.Session;

namespace WSF.Web.Sessions
{
    public class SessionScriptManager : ISessionScriptManager, ISingletonDependency
    {
        public IWSFSession WSFSession { get; set; }

        public SessionScriptManager()
        {
            WSFSession = NullWSFSession.Instance;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();

            script.AppendLine("    WSF.session = WSF.session || {};");
            
            if (WSFSession.UserId.HasValue)
            {
                script.AppendLine("    WSF.session.userId = " + WSFSession.UserId.Value + ";");
            }

            if (WSFSession.TenantId.HasValue)
            {
                script.AppendLine("    WSF.session.tenantId = " + WSFSession.TenantId.Value + ";");
            }

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
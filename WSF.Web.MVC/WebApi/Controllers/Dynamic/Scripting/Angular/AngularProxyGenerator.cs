using System.Linq;
using System.Reflection;
using System.Text;
using WSF.Extensions;
using WSF.Web;
using WSF.WebApi.Controllers.Dynamic.Scripting.Angular.Actions;

namespace WSF.WebApi.Controllers.Dynamic.Scripting.Angular
{
    internal class AngularProxyGenerator : IScriptProxyGenerator
    {
        private readonly DynamicApiControllerInfo _controllerInfo;

        public AngularProxyGenerator(DynamicApiControllerInfo controllerInfo)
        {
            _controllerInfo = controllerInfo;
        }

        public string Generate()
        {
            var script = new StringBuilder();

            script.AppendLine("(function (WSF, angular) {");
            script.AppendLine("");
            script.AppendLine("    if (!angular) {");
            script.AppendLine("        return;");
            script.AppendLine("    }");
            script.AppendLine("    ");
            script.AppendLine("    var WSFModule = angular.module('WSF');");
            script.AppendLine("    ");
            script.AppendLine("    WSFModule.factory('WSF.services." + _controllerInfo.ServiceName.Replace("/", ".") + "', [");
            script.AppendLine("        '$http', function ($http) {");
            script.AppendLine("            return new function () {");

            foreach (var methodInfo in _controllerInfo.Actions.Values)
            {
                var actionWriter = CreateActionScriptWriter(_controllerInfo, methodInfo);

                script.AppendLine("                this." + methodInfo.ActionName.ToCamelCase() + " = function (" + GenerateJsMethodParameterList(methodInfo.Method) + ") {");
                script.AppendLine("                    return $http(angular.extend({");
                script.AppendLine("                        WSF: true,");
                script.AppendLine("                        url: WSF.appPath + '" + actionWriter.GetUrl() + "',");
                actionWriter.WriteTo(script);
                script.AppendLine("                    }, httpParams));");
                script.AppendLine("                };");
                script.AppendLine("                ");
            }

            script.AppendLine("            };");
            script.AppendLine("        }");
            script.AppendLine("    ]);");
            script.AppendLine();

            //generate all methods

            script.AppendLine();
            script.AppendLine("})((WSF || (WSF = {})), (angular || undefined));");

            return script.ToString();
        }

        protected string GenerateJsMethodParameterList(MethodInfo methodInfo)
        {
            var paramNames = methodInfo.GetParameters().Select(prm => prm.Name.ToCamelCase()).ToList();
            paramNames.Add("httpParams");
            return string.Join(", ", paramNames);
        }
        
        private static ActionScriptWriter CreateActionScriptWriter(DynamicApiControllerInfo controllerInfo, DynamicApiActionInfo methodInfo)
        {
            switch (methodInfo.Verb)
            {
                case HttpVerb.Get:
                    return new GetActionScriptWriter(controllerInfo, methodInfo);
                case HttpVerb.Post:
                    return new PostActionScriptWriter(controllerInfo, methodInfo);
                case HttpVerb.Put:
                    return new PostActionScriptWriter(controllerInfo, methodInfo);
                case HttpVerb.Delete:
                    return new DeleteActionScriptWriter(controllerInfo, methodInfo);
                default:
                    throw new WSFException("This Http verb is not implemented: " + methodInfo.Verb);
            }
        }
    }
}
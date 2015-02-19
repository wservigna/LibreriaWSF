using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSF.Configuration;
using WSF.Dependency;

namespace WSF.Web.Settings
{
    /// <summary>
    /// This class is used to build setting script.
    /// </summary>
    public class SettingScriptManager : ISettingScriptManager, ISingletonDependency
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;

        public SettingScriptManager(ISettingDefinitionManager settingDefinitionManager, ISettingManager settingManager)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    WSF.setting = WSF.setting || {};");
            script.AppendLine("    WSF.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.IsVisibleToClients);

            var added = 0;
            foreach (var settingDefinition in settingDefinitions)
            {
                if (added > 0)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }

                script.Append("        '" + settingDefinition.Name.Replace("'", @"\'") + "': '" + (await _settingManager.GetSettingValueAsync(settingDefinition.Name)).Replace("'", @"\'") + "'");

                ++added;
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
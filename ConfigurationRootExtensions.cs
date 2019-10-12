using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace C9S.Configuration.Variables
{
    public static class ConfigurationRootExtensions
    {
        private static List<IConfigurationSection> configSections = new List<IConfigurationSection>();

        public static void ResolveVariables(this IConfiguration configuration, string open = "{{", string close = "}}")
        {   
            GetAllConfigurationSections(configuration.GetChildren());
            VariableResolver(configuration, open, close);
        }

        private static void GetAllConfigurationSections(IEnumerable<IConfigurationSection> configurationSections)
        {
            foreach (var section in configurationSections ?? new List<IConfigurationSection>())
            {
                configSections.Add(section);
                GetAllConfigurationSections(section.GetChildren());
            }
        }

        private static void VariableResolver(IConfiguration Configuration, string open, string close)
        {
            var open_esc = Regex.Escape(open);
            var close_esc = Regex.Escape(close);
            var variableRegex = new Regex($@"({open_esc})(?!.*{open_esc})(?<var>[^{close_esc}]+)\{close_esc}");
            while (configSections.Any())
            {
                var currentSection = configSections.First();
                configSections.RemoveAt(0);

                Match match = variableRegex.Match(currentSection.Value ?? "");
                if (!match.Success)
                {
                    continue;
                }

                var variable = match.Groups["var"].Value;
                var sections = new List<string>(variable.Split(new [] { '.' }, System.StringSplitOptions.RemoveEmptyEntries));
                IConfigurationSection section = currentSection;
                if (sections.Any())
                {
                    section = Configuration.GetSection(sections[0]);
                    var key = sections.Last();

                    sections.RemoveAt(0);
                    if (sections.Any())
                    {
                        sections.RemoveAt(sections.Count - 1);
                    }

                    foreach (var sec in sections)
                    {
                        section = section.GetSection(sec);
                    }

                    currentSection.Value = currentSection.Value.Replace($"{open}{variable}{close}", section.Value ?? section[key]);
                    configSections.Add(currentSection);
                }
            }
        }
    }
}

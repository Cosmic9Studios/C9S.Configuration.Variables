using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace C9S.Extensions.Configuration
{
    public static class ConfigurationRootExtensions
    {
        private static Regex variableRegex = new Regex(@"\{{(?<var>[^}}]+)\}}");

        public static void ResolveVariables(this IConfiguration configuration)
        {
            foreach (var configSection in configuration.GetChildren())
            {
                foreach (Match match in variableRegex.Matches(configSection.Value ?? ""))
                {
                    var variable = match.Groups["var"].Value;
                    var sections = new List<string>(variable.Split(new [] { '.' }, StringSplitOptions.RemoveEmptyEntries));
                    IConfigurationSection section = configSection;
                    if (sections.Any())
                    {
                        section = configuration.GetSection(sections[0]);
                        var key = sections.Last();

                        sections.RemoveAt(0);
                        sections.RemoveAt(sections.Count - 1);
                        foreach (var sec in sections)
                        {
                            section = section.GetSection(sec);
                        }

                        configSection.Value = configSection.Value.Replace($"{{{{{variable}}}}}", section[key]);
                    }
                }

                ResolveVariables(configSection);
            }
        }
    }
}

using System;

namespace C9S.Extensions.Configuration
{
    public static class ConfigurationRootExtensions
    {
        public static void ResolveVariables(this IConfigurationRoot configuration)
        {
            foreach (var configSection in configuration.GetChildren())
            {
                foreach (Match match in variableRegex.Matches(configSection.Value ?? ""))
                {
                    var variable = match.Groups["var"].Value;
                    var sections = new List<string>(variable.Split('.', System.StringSplitOptions.RemoveEmptyEntries));
                    IConfigurationSection section = configSection;
                    if (sections.Any())
                    {
                        section = Configuration.GetSection(sections[0]);
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

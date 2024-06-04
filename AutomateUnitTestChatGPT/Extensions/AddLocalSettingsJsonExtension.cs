using System.Diagnostics.CodeAnalysis;

namespace AutomateUnitTestChatGPT.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class AddLocalSettingsJsonExtension
    {
        public static IConfigurationBuilder AddLocalSettingsJson(this IConfigurationBuilder builder)
        {
            const string LOCAL_SETTINGS_JSON_FILE = "local.settings.json";
            const string VALUES_PREFIX = "Values:";

            // Read local.settings.json
            IConfigurationRoot config =
                new ConfigurationBuilder()
                .AddJsonFile(LOCAL_SETTINGS_JSON_FILE, true)
                .Build();

            // Identify all "Values:" keys, and return set without "Values:" prefix

            var settings =
                config
                .AsEnumerable()
                .Where(a => a.Key.StartsWith(VALUES_PREFIX))
                .Select(a => new KeyValuePair<string, string>(a.Key.Replace(VALUES_PREFIX, string.Empty), a.Value));

            // Add settings to config
            return builder.AddInMemoryCollection(settings);
        }
    }
}

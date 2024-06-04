using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using cscentappinsightstelemetry.interfaces;
using cscentappinsightstelemetry;
using Retail.OData.Client;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Amqp;

namespace AutomateUnitTestChatGPT.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddKeyVaultSecrets(this IServiceCollection services,
        string keyVaultBaseUrl, IEnumerable<string> keys)
        {
            ILoggingState loggingState = new LoggingState();

            ITelemetryLogger telemetryLogger = new TelemetryLogger
           (
                Setting.GetResourceGroupName("location-test"),
               Setting.GetResourceGroupName("location-test"),
               Guid.NewGuid(),
               "243b7130-9603-4e8a-a9c8-eeb5e497157b"
           );
            Global.LoadSecrets(telemetryLogger, loggingState, keys.ToList());
            return services;
        }
        private static async Task<IEnumerable<KeyValuePair<string, string>>> GetSecretsAsync(string keyVaultBaseUrl, IEnumerable<string> keys)
        {
            if (keys == null) return new Dictionary<string, string>();
            Dictionary<string, string> secrets = new Dictionary<string, string>();
            // Open KeyVault (using Current Credentials)
            var keyVaultClient = new SecretClient(vaultUri: new Uri(keyVaultBaseUrl), credential: new DefaultAzureCredential());
            // Read ALL Keys
            foreach (string keyName in keys)
            {
                Azure.Response<KeyVaultSecret> KeyVaultSecretProperties = await keyVaultClient.GetSecretAsync(keyName);
                secrets.Add(keyName, KeyVaultSecretProperties.Value.Value);
            }
            return secrets;
        }

    }
}

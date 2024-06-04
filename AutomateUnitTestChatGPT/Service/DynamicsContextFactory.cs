using Retail.OData.Client.OData;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OData.Extensions.Client;
using Inventory.Location.Common.Interfaces;

namespace AutomateUnitTestChatGPT.Service
{
    /// <summary>
    /// This class exists to make ODataService possible to unit test. The factory pattern gets
    /// around the problem of DynamicsContext having all static methods that cannot be mocked.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DynamicsContextFactory : IDynamicsContextFactory
    {
        public DynamicsContext CreateDynamicsContext(IODataClientFactory factory, ILogger log, bool crossCompany)
        {
            return DynamicsContext.CreateContext(factory, log, crossCompany);
        }
    }
}
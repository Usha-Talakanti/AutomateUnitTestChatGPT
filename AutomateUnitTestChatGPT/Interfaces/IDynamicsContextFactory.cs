using Retail.OData.Client.OData;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Extensions.Client;

namespace Inventory.Location.Common.Interfaces
{
    /// <summary>
    /// This interface exists to make ODataService possible to unit test. The factory pattern gets
    /// around the problem of DynamicsContext having all static methods that cannot be mocked.
    /// </summary>
    public interface IDynamicsContextFactory
    {
        public DynamicsContext CreateDynamicsContext(IODataClientFactory factory, ILogger log, bool crossCompany = true);
    }
}
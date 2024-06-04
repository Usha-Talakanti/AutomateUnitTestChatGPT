using Microsoft.Extensions.Logging;
using Microsoft.OData.Extensions.Client;
using Inventory.Location.Domain.Models;
using System.Threading.Tasks;
using Microsoft.Dynamics.DataEntities;
using System.Collections.Generic;

namespace Inventory.Location.Common.Interfaces
{
    /// <summary>
    /// This interface exists to make ODataService possible to unit test. The factory pattern gets
    /// around the problem of DynamicsContext having all static methods that cannot be mocked.
    /// </summary>
    public interface IODataServiceHelper
    {
        public Task<List<OperatingUnit>> GetOnhandInventoryByLocation(IODataClientFactory _clientFactory, ILogger log, string zipCode);
        public Task<List<MCSInventSumOutbound>> GetOnhandInventory(IODataClientFactory _clientFactory, ILogger log, string storeId, string ItemId, string ItemColorId);

    }
}
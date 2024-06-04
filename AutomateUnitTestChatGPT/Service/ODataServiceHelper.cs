using Microsoft.Dynamics.DataEntities;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Client;
using Microsoft.OData.Extensions.Client;
using Retail.OData.Client.OData;
using Inventory.Location.Common.Interfaces;
using Inventory.Location.Domain.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AutomateUnitTestChatGPT.Service
{
    public class ODataServiceHelper : IODataServiceHelper
    {
        private readonly IDynamicsContextFactory _dynamicsContextFactory;
        public ODataServiceHelper(IDynamicsContextFactory dynamicsContextFactory)
        {
            _dynamicsContextFactory = dynamicsContextFactory;
        }


        public async Task<List<OperatingUnit>> GetOnhandInventoryByLocation(IODataClientFactory _clientFactory, ILogger log, string zipCode)
        {
            using (DynamicsContext context = _dynamicsContextFactory.CreateDynamicsContext(_clientFactory, log))
            {
                var query = (DataServiceQuery<OperatingUnit>)(from opunit in context.OperatingUnits
                                                                where opunit.AddressZipCode == zipCode
                                                              select opunit);

                var result = await query.ExecuteAsync();
                return result.ToList();
            }
        }
        public async Task<List<MCSInventSumOutbound>> GetOnhandInventory(IODataClientFactory _clientFactory, ILogger log, string storeId, string itemId, string colorId)
        {
            using (DynamicsContext context = _dynamicsContextFactory.CreateDynamicsContext(_clientFactory, log))
            {
                var query = (DataServiceQuery<MCSInventSumOutbound>)(from opunit in context.MCSInventSumsOutbound
                                                              where opunit.InventLocationId == storeId && opunit.wMSLocationId =="STORE" 
                                                              && opunit.ItemId== itemId && opunit.InventColorId==colorId
                                                                     select opunit);

                var result = await query.ExecuteAsync();
                return result.ToList();
            }
        }

    }
}

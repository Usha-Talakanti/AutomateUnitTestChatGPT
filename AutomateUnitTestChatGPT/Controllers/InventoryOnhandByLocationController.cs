using System.Net.Mime;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inventory.Location.Domain.Models;

using System.Linq;
using System.Collections.Generic;
using Inventory.Location.Common.Interfaces;
using System.Web.Http;
using Microsoft.OData.Extensions.Client;
using Microsoft.Azure.WebJobs.Host;
using System.Linq.Expressions;
using System;
using AutomateUnitTestChatGPT.Models;
using OpenAI.Threads;
using System.Data;
using System.Xml.Serialization;
using System.Xml.Linq;
using Microsoft.Azure.Amqp.Framing;
using static Azure.Core.HttpHeader;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Azure;
using OpenAI.Moderations;

namespace Inventory.Location.API.Controller.V1
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class InventoryOnhandController : ApiController
    {
        private readonly ILogger<InventoryOnhandController> _logger;
        private readonly IODataServiceHelper _oDataServiceHelper;
        private readonly IODataClientFactory _oDataClientFactory;

        public InventoryOnhandController(
            ILogger<InventoryOnhandController> logger,
             IODataServiceHelper oDataServiceHelper, IODataClientFactory oDataClientFactory)
        {
            _logger = logger;
            _oDataClientFactory = oDataClientFactory;
            _oDataServiceHelper = oDataServiceHelper;
        }

        /// <summary>
        /// Get Onhand Iventory By Location
        /// </summary>
        /// <remarks>By passing in appropriate item parameter, you're able to get the entire payload associated with that item.</remarks>
        /// <param name="request">GetRetailPOSTransactionByIdQuery</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{zipCode}/{itemId}/{itemColorId}", Name = nameof(GetOnhandInventoryByLocationForProduct))]
        public async Task<ActionResult<List<InventoryOnhandLocation>>> GetOnhandInventoryByLocationForProduct(
            [FromRoute] string zipCode, [FromRoute] string itemId, [FromRoute] string itemColorId)
        {
            var lstOnhandInventoryByLocation = new List<InventoryOnhandLocation>();
            try
            {
                if (string.IsNullOrWhiteSpace(zipCode))
                {
                    return BadRequest($"An zipCode value is required.");
                }

                _logger.LogInformation("Endpoint was called: {Endpoint}. Getting Retail POS Transaction for id: {Id}", nameof(GetOnhandInventoryByLocationForProduct), zipCode);

                List<InventoryOnhandLocation> lstInventoryOnhandLocation = new List<InventoryOnhandLocation>();

                var lstOperatingUnits = await _oDataServiceHelper.GetOnhandInventoryByLocation(_oDataClientFactory, _logger, zipCode);
                foreach (var operatingUnit in lstOperatingUnits)
                {
                    InventoryOnhandLocation inventoryOnhandLocation = new InventoryOnhandLocation();
                    var lstOnhandInventories = await _oDataServiceHelper.GetOnhandInventory(_oDataClientFactory, _logger, operatingUnit.OperatingUnitNumber, itemId, itemColorId);
                    foreach (var item in lstOnhandInventories)
                    {
                        inventoryOnhandLocation.StoreId = item.InventLocationId;
                        inventoryOnhandLocation.ItemId = item.ItemId;
                        inventoryOnhandLocation.ItemColorId = item.InventColorId;
                        inventoryOnhandLocation.ItemSizeId = item.InventSizeId;
                        inventoryOnhandLocation.ItemStyleId = item.InventStyleId;
                        inventoryOnhandLocation.AvailableQuantity = item.AvailPhysical.ToString();
                        inventoryOnhandLocation.ZipCode = operatingUnit.AddressZipCode;
                        lstInventoryOnhandLocation.Add(inventoryOnhandLocation);
                    }
                }
                lstOnhandInventoryByLocation = lstInventoryOnhandLocation.GroupBy(d => new { d.ItemId, d.ItemColorId, d.ItemSizeId, d.AvailableQuantity }).Select(d => d.First())
                                       .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception : {ex.Message}");
            }
            return lstOnhandInventoryByLocation;
        }

        /// <summary>
        /// Get nearby Zipcodes from Zipcode
        /// </summary>
        /// <remarks>By passing in appropriate item parameter, you're able to get the entire payload associated with that item.</remarks>
        /// <param name="request">GetNearbyZipCodesFromZipcode</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{code}", Name = nameof(GetNearbyZipCodesFromZipcode))]
        public ActionResult<List<Store>> GetNearbyZipCodesFromZipcode(
            [FromRoute] string code)
        {
            var result = new List<Store>();
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest($"An zipCode value is required.");
                }

                result = ReturnData(code);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception : {ex.Message}");
            }
            return result;
        }

        public static Stores Deserialize<Stores>(string xmlString)
        {
            if (xmlString == null) return default;
            var serializer = new XmlSerializer(typeof(Stores));
            using (var reader = new StringReader(xmlString))
            {
                return (Stores)serializer.Deserialize(reader);
            }
        }
        private List<Store> ReturnData(string zipCode)
        {

            XDocument xmlDoc = XDocument.Load(@"C:\\Users\\106987\\Downloads\stores 1.xml"); 
            XNamespace ns = "http://www.demandware.com/xml/impex/store/2007-04-30";
            List<Store> stores = new List<Store>();
            foreach (var storeElement in xmlDoc.Descendants(ns + "store"))
            {
                Store store = new Store();
                store.Id = storeElement.Attribute("store-id")?.Value;
                store.Name = storeElement.Element(ns + "name")?.Value;
                store.PostalCode = storeElement.Element(ns + "postal-code")?.Value;
               store.Latitude = storeElement.Element(ns + "latitude")?.Value;
               store.Longitude = storeElement.Element(ns + "longitude")?.Value;
                stores.Add(store);
   };
            return stores.Where(x => x.PostalCode.Equals(zipCode)).ToList();

            //List<DataModel> dataModels = new List<DataModel>();
            //DataModel data = new DataModel();
            //data.ZipCode = "70538";
            //var ss = new List<string>();
            //ss.Add("70536");
            //ss.Add("70522");
            //ss.Add("70512");
            //ss.Add("70514");
            //data.ZipCodeList = ss;
            //dataModels.Add(data);
            //DataModel data1 = new DataModel();
            //data1.ZipCode = "70536";
            //var ss1 = new List<string>();
            //ss1.Add("70531");
            //ss1.Add("70532");
            //ss1.Add("70534");
            //ss1.Add("70506");
            //data1.ZipCodeList = ss1;
            //dataModels.Add(data1);
            //DataModel data2 = new DataModel();
            //data2.ZipCode = "70539";
            //var ss2 = new List<string>();
            //ss2.Add("70521");
            //ss2.Add("70524");
            //ss2.Add("70526");
            //ss2.Add("70509");
            //data2.ZipCodeList = ss2;
            //dataModels.Add(data2);

            //return dataModels.Where(x => x.ZipCode.Equals(zipCode)).First();
        }


    }
}
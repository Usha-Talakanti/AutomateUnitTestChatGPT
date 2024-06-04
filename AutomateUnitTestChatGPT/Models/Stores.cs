using Microsoft.Azure.Amqp.Framing;
using Microsoft.Dynamics.DataEntities;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Client;
using System.Runtime.Intrinsics.X86;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace AutomateUnitTestChatGPT.Models
{

    //[XmlRoot(ElementName = "stores")]
    //public class Stores
    //{
    //    [XmlElement(ElementName = "store")]
    //    public List<Store> stores { get; set; }
    //    public Stores()
    //    {
    //        stores = new List<Store>();
    //    }
    //}

    public class Store
    {
        [XmlElement("store-id")]
        public string Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("postal-code")]
        public string PostalCode { get; set; }
        [XmlElement("latitude")]
        public string Latitude { get; set; }
        [XmlElement("longitude")]
        public string Longitude { get; set; }

    }
}
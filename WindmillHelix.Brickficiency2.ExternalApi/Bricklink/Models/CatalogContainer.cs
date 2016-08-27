using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    [XmlRoot("CATALOG")]
    public class CatalogContainer<T>
    {
        [XmlElement(ElementName = "ITEM")]
        public T[] Items { get; set; }
    }
}

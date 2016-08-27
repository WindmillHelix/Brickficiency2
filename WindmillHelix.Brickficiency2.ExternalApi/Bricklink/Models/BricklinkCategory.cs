using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkCategory
    {
        [XmlElement(ElementName = "CATEGORY")]
        public int CategoryId { get; set; }

        [XmlElement(ElementName = "CATEGORYNAME")]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkItem
    {
        [XmlElement(ElementName = "ITEMID")]
        public string ItemId { get; set; }

        [XmlElement(ElementName = "ITEMTYPE")]
        public string ItemTypeCode { get; set; }

        [XmlElement(ElementName = "ITEMNAME")]
        public string Name { get; set; }

        [XmlElement(ElementName = "CATEGORY")]
        public int CategoryId { get; set; }

        [XmlElement(ElementName = "ITEMYEAR")]
        public string Year { get; set; }

        [XmlElement(ElementName = "ITEMWEIGHT")]
        public string Weight { get; set; }

        [XmlElement(ElementName = "ITEMDIMX")]
        public string DimensionX { get; set; }

        [XmlElement(ElementName = "ITEMDIMY")]
        public string DimensionY { get; set; }

        [XmlElement(ElementName = "ITEMDIMZ")]
        public string DimensionZ { get; set; }
    }
}

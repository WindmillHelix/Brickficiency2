using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkColor
    {
        // todo: support other color properties (don't need them yet)
        //<COLORCNTPARTS>354</COLORCNTPARTS>
        //<COLORCNTSETS>910</COLORCNTSETS>
        //<COLORCNTWANTED>648</COLORCNTWANTED>
        //<COLORCNTINV>400</COLORCNTINV>

        [XmlElement(ElementName = "COLOR")]
        public int ColorId { get; set; }

        [XmlElement(ElementName = "COLORRGB")]
        public string Rgb { get; set; }

        [XmlElement(ElementName = "COLORNAME")]
        public string Name { get; set; }

        [XmlElement(ElementName = "COLORTYPE")]
        public string ColorType { get; set; }

        [XmlElement(ElementName = "COLORYEARFROM")]
        public string YearFrom { get; set; }

        [XmlElement(ElementName = "COLORYEARTO")]
        public string YearTo { get; set; }
    }
}

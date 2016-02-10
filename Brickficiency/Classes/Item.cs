using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes
{
    public class Item
    {
        public string id;
        public string extid;
        public string type;
        public string number;
        public string colour;
        public string categoryid;
        public string status;
        public int qty;
        public decimal price;
        public string condition;
        public string comments;
        public string remarks;
        public decimal origprice;
        public int origqty;
        public string image;
        public int availstores = -1;
        public int availqty;
        public string imageurl;
        public string largeimageurl;
        public string pgpage;
    }
}

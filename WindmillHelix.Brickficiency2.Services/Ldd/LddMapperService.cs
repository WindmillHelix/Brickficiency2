using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services.Data;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    internal class LddMapperService : ILddMapperService
    {
        private readonly IItemService _itemService;
        private readonly IDictionary<string, int> _colorMap = new Dictionary<string, int>();

        public LddMapperService(IItemService itemService)
        {
            _itemService = itemService;

            PopulateColorMap();
        }

        public MappingResult<MappedPart> MapItem(LddPart source)
        {
            var result = new MappingResult<MappedPart>();
            var mapped = new MappedPart();

            var itemDetails = GetPart(source);
            if(itemDetails == null)
            {
                result.WasSuccessful = false;
                result.Message = string.Format("Unable to locate part mapping.");
                return result;
            }

            mapped.ItemId = itemDetails.ItemId;

            var colorId = GetColorId(source);
            if (!colorId.HasValue)
            {
                result.WasSuccessful = false;
                result.Message = string.Format("Unable to locate color mapping.");
                return result;
            }

            mapped.ColorId = colorId.Value;

            if (!string.IsNullOrWhiteSpace(source.Decoration))
            {
                result.WasSuccessful = false;
                result.Message = string.Format("Decorated elements are not currently supported.");
                return result;
            }

            result.Mapped = mapped;
            result.WasSuccessful = true;

            return result;
        }

        private ItemDetails GetPart(LddPart source)
        {
            var itemDetails = _itemService.GetItem(ItemTypeCodes.Part, source.DesignId);
            // todo: check other places

            return itemDetails;
        }

        private int? GetColorId(LddPart source)
        {
            if(!_colorMap.ContainsKey(source.Materials))
            {
                return null;
            }

            return _colorMap[source.Materials];
        }

        private void PopulateColorMap()
        {
            // todo: find and consume a data source for this instead of maintaining this in code
            _colorMap.Add("26", 11);
            _colorMap.Add("23", 7);
            _colorMap.Add("37", 36);
            _colorMap.Add("191", 110);
            _colorMap.Add("226", 103);
            _colorMap.Add("221", 104);
            _colorMap.Add("140", 63);
            _colorMap.Add("199", 85);
            _colorMap.Add("308", 120);
            _colorMap.Add("141", 80);
            _colorMap.Add("38", 68);
            _colorMap.Add("268", 89);
            _colorMap.Add("154", 59);
            _colorMap.Add("138", 69);
            _colorMap.Add("18", 28);
            _colorMap.Add("294", 159);
            _colorMap.Add("28", 6);
            _colorMap.Add("212", 62);
            _colorMap.Add("194", 86);
            _colorMap.Add("283", 90);
            _colorMap.Add("222", 56);
            _colorMap.Add("119", 34);
            _colorMap.Add("124", 71);
            _colorMap.Add("102", 42);
            _colorMap.Add("312", 150);
            _colorMap.Add("331", 76);
            _colorMap.Add("330", 155);
            _colorMap.Add("106", 4);
            _colorMap.Add("148", 77);
            _colorMap.Add("297", 115);
            _colorMap.Add("131", 66);
            _colorMap.Add("21", 5);
            _colorMap.Add("192", 88);
            _colorMap.Add("135", 55);
            _colorMap.Add("151", 48);
            _colorMap.Add("5", 2);
            _colorMap.Add("111", 13);
            _colorMap.Add("40", 12);
            _colorMap.Add("48", 20);
            _colorMap.Add("311", 16);
            _colorMap.Add("49", 16);
            _colorMap.Add("47", 18);
            _colorMap.Add("182", 98);
            _colorMap.Add("113", 107);
            _colorMap.Add("126", 51);
            _colorMap.Add("41", 17);
            _colorMap.Add("44", 19);
            _colorMap.Add("208", 49);
            _colorMap.Add("1", 1);
            _colorMap.Add("24", 3);
            _colorMap.Add("43", 14);
            _colorMap.Add("143", 74);
            _colorMap.Add("42", 15);
        }
    }
}

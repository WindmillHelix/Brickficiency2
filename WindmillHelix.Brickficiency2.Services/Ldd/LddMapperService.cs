using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Rebrickable;
using WindmillHelix.Brickficiency2.Services.Data;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    internal class LddMapperService : ILddMapperService
    {
        private readonly IItemService _itemService;
        private readonly IRebrickableApi _rebrickableApi;

        private readonly IDictionary<string, int> _colorMap = new Dictionary<string, int>();

        public LddMapperService(
            IItemService itemService,
            IRebrickableApi rebrickableApi)
        {
            _itemService = itemService;
            _rebrickableApi = rebrickableApi;

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

            if (!string.IsNullOrWhiteSpace(source.Decoration) && source.Decoration != "0")
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
            var idsToCheck = new List<string>();
            var checkedIds = new List<string>();

            idsToCheck.Add(source.DesignId);

            while (idsToCheck.Count > 0)
            {
                var id = idsToCheck[0];
                idsToCheck.RemoveAt(0);
                checkedIds.Add(id);

                var itemDetails = _itemService.GetItem(ItemTypeCodes.Part, id);
                if(itemDetails != null)
                {
                    return itemDetails;
                }

                var rebrickablePartInfo = _rebrickableApi.GetPartInfo(id);
                if(rebrickablePartInfo != null)
                {
                    var candidates = new List<string>();
                    candidates.AddRange(rebrickablePartInfo.BricklinkItemIds);
                    candidates.AddRange(rebrickablePartInfo.RebrickablePartIds);

                    var toAdd = candidates.Where(c => !idsToCheck.Contains(c) && !checkedIds.Contains(c));
                    idsToCheck.AddRange(toAdd);
                }
            }

            return null;
        }

        private int? GetColorId(LddPart source)
        {
            var colorId = source.Materials;
            if(colorId.EndsWith(",0"))
            {
                colorId = colorId.Substring(0, colorId.Length - 2);
            }

            if(!_colorMap.ContainsKey(colorId))
            {
                return null;
            }

            return _colorMap[colorId];
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

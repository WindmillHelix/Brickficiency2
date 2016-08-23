using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services
{
    internal class ColorService : IColorService
    {
        private readonly IBricklinkCatalogService _bricklinkCatalogService;
        private readonly IAppDataService _appDataService;

        private const string AppDataKey = "Colors_a676bfa9-4f23-413c-9e54-76c9816fb1bf.xml";
        private Lazy<bool> _isInitialized;

        private List<DBColour> _colors = new List<DBColour>();

        public ColorService(
            IBricklinkCatalogService bricklinkCatalogService, 
            IAppDataService appDataService)
        {
            _bricklinkCatalogService = bricklinkCatalogService;
            _appDataService = appDataService;

            _isInitialized = new Lazy<bool>(() => EnsureInitialized());
        }

        public IReadOnlyCollection<DBColour> GetColors()
        {
            Debug.Assert(_isInitialized.Value);
            return _colors;
        }

        public void Refresh()
        {
            var colors = GetColorsFromBricklink();

            _colors.Clear();
            _colors.AddRange(colors);

            var serializer = new TypedXmlSerializer<List<DBColour>>();
            var serialized = serializer.SerializeToString(colors);
            _appDataService.WriteAppData(AppDataKey, serialized);
        }

        private bool EnsureInitialized()
        {
            var colors = GetColorsFromAppData();
            if(colors == null || colors.Count == 0)
            {
                Refresh();
            }
            else
            {
                _colors.Clear();
                _colors.AddRange(colors);
            }

            return true;
        }

        private List<DBColour> GetColorsFromAppData()
        {
            var appData = _appDataService.GetAppData(AppDataKey);
            if (string.IsNullOrWhiteSpace(appData))
            {
                return new List<DBColour>();
            }

            var serializer = new TypedXmlSerializer<List<DBColour>>();
            var colors = serializer.Deserialize(appData);

            return colors;
        }

        private List<DBColour> GetColorsFromBricklink()
        {
            var colors = _bricklinkCatalogService.DownloadColorList();
            var converted = colors.Select(x => new DBColour
            {
                id = x.ColorId.ToString(),
                name = x.Name,
                rgb = x.Rgb,
                type = x.ColorType
            }).ToList();

            return converted;
        }
    }
}

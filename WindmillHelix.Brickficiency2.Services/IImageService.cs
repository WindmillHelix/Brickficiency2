using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface IImageService
    {

        Task<Image> GetImageAsync(string itemTypeCode, string itemId, int colorId);

        Task<Image> GetLargeImageAsync(string itemTypeCode, string itemId);
    }
}

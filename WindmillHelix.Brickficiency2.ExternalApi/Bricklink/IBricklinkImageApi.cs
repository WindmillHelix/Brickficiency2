using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkImageApi
    {
        Task<byte[]> GetImageBytesAsync(string itemTypeCode, string itemId, int colorId, ImageFormat imageFormat);

        Task<byte[]> GetLargeImageBytesAsync(string itemTypeCode, string itemId, ImageFormat imageFormat);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Net;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkImageApi : IBricklinkImageApi
    {
        public Task<byte[]> GetImageBytesAsync(string itemTypeCode, string itemId, int colorId, ImageFormat imageFormat)
        {
            var colorIdParameter = colorId == 0 ? string.Empty : string.Format("&colorID={0}", colorId);

            var url = string.Format(
                "https://www.bricklink.com/getPic.asp?itemType={0}{1}&itemNo={2}",
                itemTypeCode,
                colorIdParameter,
                itemId);

            var result = DownloadImageBytes(url);
            return result;
        }

        public Task<byte[]> GetLargeImageBytesAsync(string itemTypeCode, string itemId, ImageFormat imageFormat)
        {
            var url = string.Format(
                "https://www.bricklink.com/{0}L/{1}.{2}",
                HttpUtility.UrlEncode(itemTypeCode),
                HttpUtility.UrlEncode(itemId),
                GetExtension(imageFormat));

            var result = DownloadImageBytes(url);
            return result;
        }

        private async Task<byte[]> DownloadImageBytes(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.UserAgent = Constants.UserAgent;
            request.AllowAutoRedirect = false;

            using (var response = await request.GetHttpWebResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.GetResponseStream().ToByteArray();
                    return result;
                }

                if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Found
                    || response.StatusCode == HttpStatusCode.Moved
                    || response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    var location = response.Headers["Location"]?.ToLowerInvariant() ?? string.Empty;
                    if (location.EndsWith("/images/noimage.gif") || location.EndsWith("error_404.page"))
                    {
                        return null;
                    }

                    if (location.StartsWith("http:") || location.StartsWith("https:"))
                    {
                        return await DownloadImageBytes(location);
                    }

                    var newUrl = "https://www.bricklink.com" + location;
                    return await DownloadImageBytes(newUrl);
                }

                throw new Exception("Unexpected status code: " + response.StatusCode.ToString());
            }
        }

        private string GetExtension(ImageFormat imageFormat)
        {
            if (imageFormat == ImageFormat.Gif)
            {
                return "gif";
            }
            else if (imageFormat == ImageFormat.Jpeg)
            {
                return "jpg";
            }

            throw new NotSupportedException();
        }
    }
}

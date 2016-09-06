using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services
{
    internal class ImageService : IImageService
    {
        private readonly ICacheService _cacheService;
        private readonly IBricklinkImageApi _bricklinkImageApi;

        private readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();

        private readonly ImageFormat[] _imageFormats = new[] { ImageFormat.Jpeg, ImageFormat.Gif };

        public ImageService(
            ICacheService cacheService,
            IBricklinkImageApi bricklinkImageApi)
        {
            _bricklinkImageApi = bricklinkImageApi;
            _cacheService = cacheService;
        }

        public Task<Image> GetImageAsync(string itemTypeCode, string itemId, int colorId)
        {
            var key = GetKey(itemTypeCode, itemId, colorId);
            var imageTask = GetImageAsync(key, format => _bricklinkImageApi.GetImageBytesAsync(itemTypeCode, itemId, colorId, format));

            return imageTask;
        }

        public Task<Image> GetLargeImageAsync(string itemTypeCode, string itemId)
        {
            var key = GetLargeImageKey(itemTypeCode, itemId);
            var imageTask = GetImageAsync(key, format => _bricklinkImageApi.GetLargeImageBytesAsync(itemTypeCode, itemId, format));

            return imageTask;
        }

        private static Image CreateImageFromBytes(byte[] bytes)
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                var image = Image.FromStream(stream);
                return image;
            }
        }

        private async Task<Image> GetImageAsync(string key, Func<ImageFormat, Task<byte[]>> retrieveMethod)
        {
            if (_images.ContainsKey(key))
            {
                return _images[key];
            }

            var cachedBytes = _cacheService.GetCachedItemBytes(key);
            if (cachedBytes != null && cachedBytes.Length > 0)
            {
                var image = CreateImageFromBytes(cachedBytes);
                lock (_images)
                {
                    if (!_images.ContainsKey(key))
                    {
                        _images.Add(key, image);
                    }
                }
            }

            foreach (var imageFormat in _imageFormats)
            {
                var bytes = await retrieveMethod(imageFormat);
                if (bytes != null && bytes.Length > 0)
                {
                    var image = CreateImageFromBytes(bytes);
                    lock (_images)
                    {
                        if (!_images.ContainsKey(key))
                        {
                            _images.Add(key, image);
                        }
                    }

                    _cacheService.WriteCachedItemBytes(key, bytes);

                    return image;
                }
            }

            lock (_images)
            {
                // put null into memory cache so we don't keep trying if it's not found
                _images.Add(key, null);
            }

            return null;
        }

        private Image GetImage(string key, Func<ImageFormat, byte[]> retrieveMethod)
        {
            if (_images.ContainsKey(key))
            {
                return _images[key];
            }

            foreach (var imageFormat in _imageFormats)
            {
                var bytes = retrieveMethod(imageFormat);
                if (bytes != null && bytes.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        var image = Image.FromStream(stream);
                        lock (_images)
                        {
                            if (!_images.ContainsKey(key))
                            {
                                _images.Add(key, image);
                            }
                        }

                        return image;
                    }
                }
            }

            lock (_images)
            {
                // put null into memory cache so we don't keep trying if it's not found
                _images.Add(key, null);
            }

            return null;
        }

        private string GetLargeImageKey(string itemTypeCode, string itemId)
        {
            return string.Format("Image_Large_{0}_{1}", itemTypeCode, itemId).ToLowerInvariant();
        }

        private string GetKey(string itemTypeCode, string itemId, int colorId)
        {
            return string.Format("Image_{0}_{1}_{2}", itemTypeCode, itemId, colorId).ToLowerInvariant();
        }
    }
}

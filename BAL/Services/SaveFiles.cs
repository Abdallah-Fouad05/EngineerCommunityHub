using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BAL.Services
{
    public class Services
    {
        //private const string BaseUrl = "https://localhost:7018";
        private const string BaseUrl = "http://asmecommunityapi.runasp.net";

        private static Dictionary<string, string> _videoHashes = new();
        private static Dictionary<string, string> _imageHashes = new();
        private static async Task<string> GetFileHashAsync(IFormFile file)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            await using var stream = file.OpenReadStream();
            var hashBytes = await md5.ComputeHashAsync(stream);
            return Convert.ToHexString(hashBytes);
        }

        public static async Task<string?> SaveImageAsync(IFormFile? image, string folderName)
        {
            if (image == null || image.Length == 0)
                return null;

            var ext = Path.GetExtension(image.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowed.Contains(ext))
                throw new ApplicationException("Invalid image format.");

            if (image.Length > 5 * 1024 * 1024)
                throw new ApplicationException("Image size exceeds 5MB.");

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var folderPath = Path.Combine(basePath, folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 👇 hash للصورة
            var newHash = await GetFileHashAsync(image);

            // 👇 check في الكاش
            if (_imageHashes.ContainsKey(newHash))
                return _imageHashes[newHash];

            // 👇 حفظ الصورة
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folderPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var url = $"{BaseUrl}/images/{folderName}/{fileName}";

            // 👇 تخزين في الكاش
            _imageHashes[newHash] = url;

            return url;
        }
        public static async Task<List<string>> SaveImagesAsync(List<IFormFile?> images, string folderName)
        {
            List<string> imagesPath = new();

            if (images == null || images.Count == 0)
                return imagesPath;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                folderName
            );

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                    continue;

                var ext = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                    throw new ApplicationException("Invalid image format");

                if (image.Length > 5 * 1024 * 1024)
                    throw new ApplicationException("Image size exceeds 5MB");

                var newHash = await GetFileHashAsync(image);

                // 👇 check في الكاش
                if (_imageHashes.ContainsKey(newHash))
                {
                    imagesPath.Add(_imageHashes[newHash]);
                    continue;
                }

                var fileName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(folder, fileName);

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var url = $"{BaseUrl}/images/{folderName}/{fileName}";

                _imageHashes[newHash] = url;

                imagesPath.Add(url);
            }

            return imagesPath;
        }
        public static async Task<List<string>> SaveVideosAsync(List<IFormFile?> videos, string folderName)
        {
            List<string> videosPath = new();

            if (videos == null || videos.Count == 0)
                return videosPath;

            var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv", ".wmv", ".webm" };

            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "videos",
                folderName
            );

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var video in videos)
            {
                if (video == null || video.Length == 0)
                    continue;

                var ext = Path.GetExtension(video.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                    throw new ApplicationException("Invalid video format");

                if (video.Length > 50 * 1024 * 1024)
                    throw new ApplicationException("Video size exceeds 50MB");

                // 👇 نحسب hash
                var newHash = await GetFileHashAsync(video);

                // 👇 check في الكاش (سريع جدًا)
                if (_videoHashes.ContainsKey(newHash))
                {
                    videosPath.Add(_videoHashes[newHash]);
                    continue;
                }

                // 👇 لو مش موجود في الكاش → احفظه
                var fileName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(folder, fileName);

                await using var streamNew = new FileStream(path, FileMode.Create);
                await video.CopyToAsync(streamNew);

                var url = $"{BaseUrl}/videos/{folderName}/{fileName}";

                // 👇 خزّنه في الكاش
                _videoHashes[newHash] = url;

                videosPath.Add(url);
            }

            return videosPath;
        }
    }
}

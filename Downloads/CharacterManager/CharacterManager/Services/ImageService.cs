namespace CharacterManager.Services;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

public class ImageService
{
    private const string CacheDirectory = "Assets/Characters";
    private const string MetadataFileName = "cache_metadata.json";
    private static readonly HttpClient httpClient = new();

    public ImageService()
    {
        EnsureCacheDirectory();
    }

    private void EnsureCacheDirectory()
    {
        if (!Directory.Exists(CacheDirectory))
        {
            Directory.CreateDirectory(CacheDirectory);
        }
    }

    public async Task<(bool Success, string LocalPath)> DownloadImageAsync(string imageUrl, int characterId)
    {
        if (string.IsNullOrEmpty(imageUrl) || imageUrl == "Не вказано")
        {
            return (false, string.Empty);
        }

        try
        {
            string localFileName = $"character_{characterId}_{Path.GetFileNameWithoutExtension(imageUrl)}.png";
            string localPath = Path.Combine(CacheDirectory, localFileName);

            // Якщо локальна копія вже існує, повернути її
            if (File.Exists(localPath))
            {
                return (true, localPath);
            }

            // Завантажити графіку
            using var response = await httpClient.GetAsync(imageUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"⚠️  Не вдалось завантажити графіку: {response.StatusCode}");
                return (false, string.Empty);
            }

            // Зберегти локально
            await using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fileStream);

            return (true, localPath);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Помилка мережі при завантаженні графіки: {ex.Message}");
            return (false, string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Помилка при завантаженні графіки: {ex.Message}");
            return (false, string.Empty);
        }
    }

    public string GetImageHash(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            using var sha256 = SHA256.Create();
            using var fileStream = File.OpenRead(filePath);
            byte[] hashBytes = sha256.ComputeHash(fileStream);
            return Convert.ToHexString(hashBytes).ToLower();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Помилка при обчисленні хеша: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task<bool> IsImageOutdatedAsync(string imageUrl, string localPath)
    {
        try
        {
            if (!File.Exists(localPath))
            {
                return true;
            }

            // Отримати хеш локального файлу
            string localHash = GetImageHash(localPath);

            // Отримати розмір файлу на сервері як простий показник оновлення
            var request = new HttpRequestMessage(HttpMethod.Head, imageUrl);
            var response = await httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var remoteSize = response.Content.Headers.ContentLength ?? 0;
            var localSize = new FileInfo(localPath).Length;

            return remoteSize != localSize;
        }
        catch
        {
            return false;
        }
    }

    public void DeleteImage(string localPath)
    {
        try
        {
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Помилка при видаленні графіки: {ex.Message}");
        }
    }

    public void ClearCache()
    {
        try
        {
            if (Directory.Exists(CacheDirectory))
            {
                var files = Directory.GetFiles(CacheDirectory);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Помилка при очищенні кеша: {ex.Message}");
        }
    }

    public long GetCacheSize()
    {
        try
        {
            if (!Directory.Exists(CacheDirectory))
            {
                return 0;
            }

            var files = Directory.GetFiles(CacheDirectory);
            return files.Sum(f => new FileInfo(f).Length);
        }
        catch
        {
            return 0;
        }
    }

    public List<string> GetCachedImages()
    {
        try
        {
            if (!Directory.Exists(CacheDirectory))
            {
                return new List<string>();
            }

            return Directory.GetFiles(CacheDirectory)
                .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg"))
                .ToList();
        }
        catch
        {
            return new List<string>();
        }
    }
}

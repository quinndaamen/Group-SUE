using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class ElectricityPriceCacheService
{
    private readonly ElectricityPriceApiClient _apiClient;

    private List<CleanForecastItem>? _cachedData;
    private DateTimeOffset _lastUpdatedUtc;

    private static readonly TimeSpan Ttl = TimeSpan.FromHours(1);
    private static readonly string CacheFilePath = "price_cache.json";

    private readonly SemaphoreSlim _lock = new(1, 1);

    public ElectricityPriceCacheService(ElectricityPriceApiClient apiClient)
    {
        _apiClient = apiClient;
        LoadFromFile();
    }

    public async Task<List<CleanForecastItem>> GetForecastAsync()
    {
        if (IsCacheValid())
            return _cachedData!;

        await _lock.WaitAsync();
        try
        {
            if (IsCacheValid())
                return _cachedData!;

            try
            {
                var fresh = await _apiClient.FetchForecastAsync();
                _cachedData = fresh;
                _lastUpdatedUtc = DateTimeOffset.UtcNow;
                SaveToFile();
            }
            catch
            {
                if (_cachedData != null)
                    return _cachedData; // stale fallback

                throw; // no data at all → hard fail
            }

            return _cachedData!;
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool IsCacheValid()
    {
        return _cachedData != null &&
               DateTimeOffset.UtcNow - _lastUpdatedUtc < Ttl;
    }

    private void LoadFromFile()
    {
        if (!File.Exists(CacheFilePath))
            return;

        var json = File.ReadAllText(CacheFilePath);
        var fileCache = JsonSerializer.Deserialize<FileCache>(json);

        if (fileCache == null)
            return;

        _cachedData = fileCache.Data;
        _lastUpdatedUtc = fileCache.LastUpdatedUtc;
    }

    private void SaveToFile()
    {
        var fileCache = new FileCache
        {
            LastUpdatedUtc = _lastUpdatedUtc,
            Data = _cachedData!
        };

        var json = JsonSerializer.Serialize(fileCache, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(CacheFilePath, json);
    }

    private class FileCache
    {
        public DateTimeOffset LastUpdatedUtc { get; set; }
        public List<CleanForecastItem> Data { get; set; } = new();
    }
}

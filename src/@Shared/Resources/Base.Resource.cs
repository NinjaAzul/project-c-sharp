using System.Collections.Concurrent;
using System.Globalization;
using System.Resources;

namespace Project_C_Sharp.Shared.Resources.Base;

public abstract class BaseResource
{
    private readonly ResourceManager _resourceManager;
    private readonly ConcurrentDictionary<string, string> _cache = new();

    protected BaseResource(string baseName)
    {
        _resourceManager = new ResourceManager(baseName, GetType().Assembly);
    }

    public string GetString(string key)
    {
        try
        {
            var culture = CultureInfo.CurrentUICulture;
            var resourceString = _resourceManager.GetString(key, culture);

            if (string.IsNullOrEmpty(resourceString))
            {
                resourceString = _resourceManager.GetString(key, new CultureInfo("en-US"));

                if (string.IsNullOrEmpty(resourceString))
                {
                    return $"[{key}]";
                }
            }

            return _cache.GetOrAdd($"{key}_{culture.Name}", _ => resourceString);
        }
        catch
        {
            return $"[{key}]";
        }
    }
}
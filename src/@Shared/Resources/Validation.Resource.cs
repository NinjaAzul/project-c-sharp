using System.Collections.Concurrent;
using System.Globalization;
using System.Resources;

namespace Project_C_Sharp.Shared.Resources.Validation;


public static class ResourceValidation
{
    private static readonly ResourceManager _resourceManager = new("Project_C_Sharp.Shared.I18n.Validation.Messages", typeof(ResourceValidation).Assembly);
    private static readonly ConcurrentDictionary<string, string> _cache = new();

    public static string GetString(string key)
    {
        try
        {
            var culture = CultureInfo.CurrentUICulture;
            var resourceString = _resourceManager.GetString(key, culture);

            if (string.IsNullOrEmpty(resourceString))
            {
                // Se não encontrar na cultura atual, tenta buscar em inglês
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

    public static string GetString(string key, CultureInfo culture)
    {
        try
        {
            var resourceString = _resourceManager.GetString(key, culture);

            if (string.IsNullOrEmpty(resourceString))
            {
                // Se não encontrar na cultura especificada, tenta buscar em inglês
                resourceString = _resourceManager.GetString(key, new CultureInfo("en-US"));

                if (string.IsNullOrEmpty(resourceString))
                {
                    return $"[{key}]";
                }
            }

            return resourceString;
        }
        catch
        {
            return $"[{key}]";
        }
    }
}
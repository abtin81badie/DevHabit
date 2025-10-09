using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using DevHabit.Api.DTOs.Common;

namespace DevHabit.Api.Services;

public sealed class DataShapingService
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = new();

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public ExpandoObject ShapeData<T>(T entity, string? fields)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        var fieldSet = fields?
          .Split(',', StringSplitOptions.RemoveEmptyEntries)
          .Select(f => f.Trim())
          .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];


        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
            typeof(T),
            t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        if (fieldSet.Any())
        {
            propertyInfos = propertyInfos
                .Where(p => fieldSet.Contains(p.Name))
                .ToArray();
        }

        IDictionary<string, object?> shapeObject = new ExpandoObject();

        foreach (var propertyInfo in propertyInfos)
        {
            shapeObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
        }

        return (ExpandoObject)shapeObject;
    }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public List<ExpandoObject> ShapeCollectionData<T>(
        IEnumerable<T> entities, 
        string? fields,
        Func<T, List<LinkDto>>? linksFactory = null)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        var fieldSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];


        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
            typeof(T),
            t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        if (fieldSet.Any())
        {
            propertyInfos = propertyInfos
                .Where(p => fieldSet.Contains(p.Name))
                .ToArray();
        }

        List<ExpandoObject> shapeObjects = [];
        foreach (var entity in entities)
        {
            IDictionary<string, object?> shapeObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfos)
            {
                shapeObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
            }

            if (linksFactory is not null)
            {
                shapeObject["links"] = linksFactory(entity);
            }

            shapeObjects.Add((ExpandoObject)shapeObject);
        }

        return shapeObjects;

    }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public bool Validate<T>(string? fields)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        if (string.IsNullOrWhiteSpace(fields))
            return true;

        HashSet<string> fieldSet = fields
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
            typeof(T),
            t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        return fieldSet.All(f => propertyInfos.Any(p => p.Name.Equals(f, StringComparison.OrdinalIgnoreCase)));
    }
}



using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace TenantAuthenticator.Services.Common;
internal static class ObjectHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo?> CachedObjectProperties =
        new();

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        TrySetProperty(obj, propertySelector, x => valueFactory(), ignoreAttributeTypes);
    }

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TObject, TValue> valueFactory,
        params Type[]? ignoreAttributeTypes)
    {
        string cacheKey = $"{obj?.GetType().FullName}-" +
                       $"{propertySelector}-" +
                       $"{(ignoreAttributeTypes != null ? "-" + string.Join("-", ignoreAttributeTypes.Select(x => x.FullName)) : "")}";

        PropertyInfo? property = CachedObjectProperties.GetOrAdd(cacheKey, _ =>
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
            {
                return null;
            }

            MemberExpression? memberExpression = propertySelector.Body as MemberExpression;

            PropertyInfo? propertyInfo = obj?.GetType().GetProperties().FirstOrDefault(x =>
                x.Name == memberExpression?.Member.Name &&
                x.GetSetMethod(true) != null);

            return propertyInfo == null
                ? null
                : ignoreAttributeTypes != null &&
                ignoreAttributeTypes.Any(ignoreAttribute => propertyInfo.IsDefined(ignoreAttribute, true))
                ? null
                : propertyInfo;
        });

        property?.SetValue(obj, valueFactory(obj));
    }
}

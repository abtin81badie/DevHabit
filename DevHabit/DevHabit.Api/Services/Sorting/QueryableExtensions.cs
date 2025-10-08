using System.Linq.Dynamic.Core;

namespace DevHabit.Api.Services.Sorting;

internal static class QueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        string? sort,
        IEnumerable<SortMapping> mappings,
        string defaultOrderBy = "Id"
    )
    {
        if (string.IsNullOrWhiteSpace(sort))
            return query.OrderBy(defaultOrderBy);

        var sortFields = sort.Split(',')
            .Select(x => x.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        var orderByParts = new List<string>();
        foreach (var field in sortFields)
        {
            (string sortField, bool isDescending) = ParseSortField(field);

            SortMapping mapping = mappings.First(m =>
                m.SortField.Equals(sortField, StringComparison.OrdinalIgnoreCase));

            string direction = (isDescending, mapping.Reverse) switch
            {
                (false, false) => "ASC",
                (false, true) => "DESC",
                (true, false) => "DESC",
                (true, true) => "ASC"
            };

            orderByParts.Add($"{mapping.PropertyName} {direction}");
        }

        string orderBy = string.Join(",", orderByParts);

        return query.OrderBy(orderBy);
    }

    private static (string SortField, bool IsDescending) ParseSortField(string field)
    {
        string sortField = field.Trim();
        bool isDescending = sortField.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);

        if (isDescending)
        {
            sortField = sortField[..^" desc".Length].Trim();
        }

        return (sortField, isDescending);
    }
}
namespace MobyLabWebProgramming.Core.Requests;

/// <summary>
/// This class extends the pagination query parameters and includes a search string to be used in querying the database.
/// </summary>
public class PaginationSearchQueryParams : PaginationQueryParams
{
    public int Page { get; set; } = 0;
    public int ElementsPerPage { get; set; } = 10;
    public string? Search { get; set; }
}

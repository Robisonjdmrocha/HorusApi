namespace HorusV2.Domain.Queries.Parameters;

public class SearchStreamingRequestQuery
{
    public SearchStreamingRequestQuery()
    {
    }

    public SearchStreamingRequestQuery(int? day,int? month, int? year, int offset, int take)
    {
        Day = day;
        Month = month;
        Year = year;
        Offset = offset;
        Take = take;
    }

    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int Offset { get; set; }
    public int Take { get; set; }
}
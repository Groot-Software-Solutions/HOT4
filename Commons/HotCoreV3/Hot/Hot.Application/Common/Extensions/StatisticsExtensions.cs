namespace Hot.Application.Common.Extensions
{
    public static class StatisticsExtensions
    {
        public static List<StatSeries> ToStatSeries(this List<StatResult> list)
        {
            var tmp = list.GroupBy(x => x.Name);
            var result = tmp.Select(
                y => new StatSeries ( 
                        y.Key, 
                        y.Select(s => new StatDataPoint((long)s.X, (long)s.Y)).ToList()
                    )
                ).ToList();
            return result;
        }
    }
}

namespace Hot.Application.Common.Models
{
    public class StatSeries
    {
        public StatSeries()
        {
        }

        public StatSeries(string name, List<StatDataPoint> data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; set; } = string.Empty;
        public List<StatDataPoint> Data { get; set; } 
    }
}

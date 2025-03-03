namespace Hot.Application.Common.Extensions
{
    public static  class BundleModelExtensions
    {
        public static Dictionary<string, BundleModel> SetBundleProducts<T>(this List<T>? list, Func<T, BundleModel> Mapper)
        {
            if (list is null) return new();
            var result = list.Select(b => Mapper(b));
            return result.ToDictionary(b => b.ProductCode);
        }
    }
}

namespace Hot.Application.Common.Interfaces
{
    public interface IHotTypeIdentifier
    {
        public HotTypes Identify(string TypeCode,int SplitCount);
        public HotTypes Identify(string Data);
    }
}

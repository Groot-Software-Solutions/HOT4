namespace TelOne.Application.Common.Models
{
    public class Resultheaderfield
    {
        public string versionField { get; set; }
        public string resultCodeField { get; set; }
        public string msgLanguageCodeField { get; set; }
        public string resultDescField { get; set; }
        public Additionalpropertyfield[] additionalPropertyField { get; set; }
    }
}

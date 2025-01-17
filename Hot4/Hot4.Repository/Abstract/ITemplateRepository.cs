using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ITemplateRepository
    {
        Task<Template?> GetTemplateById(int templateId);
        Task<List<Template>> ListTemplates();
        Task<bool> AddTemplate(Template template);
        Task<bool> UpdateTemplate(Template template);
        Task<bool> DeleteTemplate(Template template);
    }
}

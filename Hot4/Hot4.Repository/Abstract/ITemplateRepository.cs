using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ITemplateRepository
    {
        Task<TemplateModel?> GetTemplateById(int templateId);
        Task<List<TemplateModel>> ListTemplates();
        Task AddTemplate(Template template);
        Task UpdateTemplate(Template template);
        Task DeleteTemplate(Template template);
    }
}

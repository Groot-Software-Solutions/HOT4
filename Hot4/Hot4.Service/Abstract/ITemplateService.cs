using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ITemplateService
    {
        Task<TemplateModel?> GetTemplateById(int templateId);
        Task<List<TemplateModel>> ListTemplates();
        Task<bool> AddTemplate(TemplateModel template);
        Task<bool> UpdateTemplate(TemplateModel template);
        Task<bool> DeleteTemplate(int templateId);
    }
}

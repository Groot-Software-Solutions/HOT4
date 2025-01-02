using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class TemplateRepository : RepositoryBase<Template>, ITemplateRepository
    {
        public TemplateRepository(HotDbContext context) : base(context) { }

        public async Task AddTemplate(Template template)
        {
            await Create(template);
            await SaveChanges();
        }
        public async Task DeleteTemplate(Template template)
        {
            await Delete(template);
            await SaveChanges();
        }
        public async Task<TemplateModel?> GetTemplateById(int templateId)
        {
            var result = await GetById(templateId);
            if (result != null)
            {
                return new TemplateModel
                {
                    TemplateId = result.TemplateId,
                    TemplateName = result.TemplateName,
                    TemplateText = result.TemplateText,
                };
            }
            return null;
        }
        public Task<List<TemplateModel>> ListTemplates()
        {
            return GetAll().Select(d => new TemplateModel
            {
                TemplateId = d.TemplateId,
                TemplateText = d.TemplateText,
                TemplateName = d.TemplateName,
            }).ToListAsync();
        }
        public async Task UpdateTemplate(Template template)
        {
            await Update(template);
            await SaveChanges();
        }
    }
}

using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class TemplateRepository : RepositoryBase<Template>, ITemplateRepository
    {
        public TemplateRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddTemplate(Template template)
        {
            await Create(template);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteTemplate(Template template)
        {
            Delete(template);
            await SaveChanges();
            return true;
        }
        public async Task<Template?> GetTemplateById(int templateId)
        {
            return await GetById(templateId);
        }
        public Task<List<Template>> ListTemplates()
        {
            return GetAll().ToListAsync();
        }
        public async Task<bool> UpdateTemplate(Template template)
        {
            Update(template);
            await SaveChanges();
            return true;
        }
    }
}

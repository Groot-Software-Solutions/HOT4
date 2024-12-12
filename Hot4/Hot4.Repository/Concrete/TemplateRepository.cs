using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class TemplateRepository : RepositoryBase<Template>, ITemplateRepository
    {
        public TemplateRepository(HotDbContext context) : base(context) { }
        public async Task<Template?> GetTemplate(int templateId)
        {
            return await GetById(templateId);
        }
    }
}

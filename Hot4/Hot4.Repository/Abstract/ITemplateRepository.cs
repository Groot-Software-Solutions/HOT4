using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ITemplateRepository
    {
        Task<TblTemplate?> GetTemplate(int templateID);
    }
}

using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class TemplateService : ITemplateService
    {
        private ITemplateRepository _templateRepository;
        private readonly IMapper Mapper;
        public TemplateService(ITemplateRepository templateRepository, IMapper mapper)
        {
            _templateRepository = templateRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddTemplate(TemplateModel template)
        {
            var model = Mapper.Map<Template>(template);
            return await _templateRepository.AddTemplate(model);
        }

        public async Task<bool> DeleteTemplate(int templateId)
        {
            var record = await _templateRepository.GetTemplateById(templateId);
            if (record != null)
            {
                return await _templateRepository.DeleteTemplate(record);
            }
            return false;
        }

        public async Task<TemplateModel?> GetTemplateById(int templateId)
        {
            var model = await _templateRepository.GetTemplateById(templateId);
            return Mapper.Map<TemplateModel>(model);
        }

        public async Task<List<TemplateModel>> ListTemplates()
        {
            var records = await _templateRepository.ListTemplates();
            return Mapper.Map<List<TemplateModel>>(records);
        }

        public async Task<bool> UpdateTemplate(TemplateModel template)
        {
            var record = await _templateRepository.GetTemplateById(template.TemplateId);
            if (record != null)
            {
                var model = Mapper.Map(template, record);
                return await _templateRepository.UpdateTemplate(record);
            }
            return false;
        }
    }
}

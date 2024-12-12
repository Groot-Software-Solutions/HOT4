using Hot4.Core.Enums;
using Hot4.Core.Settings;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hot4.Repository.Concrete
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ISMSRepository _smsRepository;
        private readonly ITemplateRepository _templateRepository;
        private TemplateSettings _templateSettings { get; }

        public AnswerRepository(ISMSRepository smsRepository, ITemplateRepository templateRepository, IOptions<TemplateSettings> templateSettings)
        {
            _smsRepository = smsRepository;
            _templateRepository = templateRepository;
            _templateSettings = templateSettings.Value;
        }
        public async Task RespondToAnswer(TblSms sms)
        {
            if (sms.Smstext.ToUpper().StartsWith("OPT"))
            {
                var responseText = EF.Constant(sms.Smstext.ToUpper()).Contains("OUT")
                    ? _templateSettings.AnniversaryOptOut
                    : _templateSettings.AnniversaryOptIn;

                await _smsRepository.ReplyWithTransaction(sms, new() { new() { TemplateName = _templateSettings.AnniversaryTemplate, TemplateText = responseText } });

            }
            else
            {
                var answerTemplate = await _templateRepository.GetTemplate(
                    string.IsNullOrWhiteSpace(sms.Smstext)
                    ? (int)Templates.AnswerError
                    : (int)Templates.AnswerOK);
                if (answerTemplate != null && !string.IsNullOrEmpty(answerTemplate.TemplateText))
                {
                    answerTemplate.TemplateText = answerTemplate.TemplateText.Replace("%MESSAGE%", sms.Mobile);
                    await _smsRepository.ReplyWithTransaction(sms, new() { answerTemplate });
                }
            }
        }

        public async Task RespondToUnknown(TblSms sms)
        {
            var unknownRequest = await _templateRepository.GetTemplate((int)Templates.UnknownRequest);
            if (unknownRequest != null && !string.IsNullOrEmpty(unknownRequest.TemplateText))
            {
                unknownRequest.TemplateText = unknownRequest.TemplateText.Replace("%MESSAGE%", sms.Mobile);
                await _smsRepository.ReplyWithTransaction(sms, new() { unknownRequest });
            }
        }
    }
}

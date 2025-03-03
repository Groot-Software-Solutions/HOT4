namespace Hot.Application.Common.Extensions
{
    public static class TemplateExtensions
    {

        public static Template? GetTemplate(IDbContext dbContext, Templates template)
        {
            var templateResponse = dbContext.Templates.Get((int)template);
            if (templateResponse.IsT1) return null;
            return templateResponse.AsT0;
        }
        public static Template SetAmount(this Template template, decimal Amount)
        {
            template.TemplateText = template.TemplateText.Replace("%AMOUNT%", Amount.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%TOTALAMOUNT%", Amount.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);

            return template;
        }
        public static Template SetDiscount(this Template template, decimal Discount)
        {
            template.TemplateText = template.TemplateText.Replace("%DISCOUNT%", Discount.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetRefence(this Template template, string? Reference)
        {
            template.TemplateText = template.TemplateText.Replace("%REFERENCE%", Reference);
            return template;
        }
        public static Template SetBalance(this Template template, decimal Balance, bool CustomerBalance = false)
        {
            string balance = ((Balance == 0 || Balance == -1) && CustomerBalance)
                ? "Unknown"
                : Balance.ToString("#,##0.00");

            template.TemplateText = template.TemplateText.Replace("%FINALBALANCE%", balance, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%BALANCE%", balance, StringComparison.OrdinalIgnoreCase);
            return template;
        }

        public static Template SetInitalBalance(this Template template, decimal Balance, bool CustomerBalance = false)
        {
            string balance = ((Balance == 0 || Balance == -1) && CustomerBalance)
                ? "Unknown"
                : Balance.ToString("#,##0.00");

            template.TemplateText = template.TemplateText.Replace("%INITIALBALANCE%", balance, StringComparison.OrdinalIgnoreCase);
            return template;
        }


        public static Template SetCost(this Template template, decimal Cost)
        {
            template.TemplateText = template.TemplateText.Replace("%COST%", Cost.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetSaleValue(this Template template, decimal SaleValue)
        {
            template.TemplateText = template.TemplateText.Replace("%SALEVALUE%", SaleValue.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetContact(this Template template, string? Contact)
        {
            template.TemplateText = template.TemplateText.Replace("%CONTACT%", Contact, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%REFERREDBY%", Contact, StringComparison.OrdinalIgnoreCase);

            return template;
        }
        public static Template SetPassword(this Template template, string? password)
        {
            template.TemplateText = template.TemplateText.Replace("%PASSWORD%", password, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetMobile(this Template template, string? mobile)
        {
            template.TemplateText = template.TemplateText.Replace("%MOBILE%", mobile, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetMessage(this Template template, string? message)
        {
            template.TemplateText = template.TemplateText.Replace("%MESSAGE%", message, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetBrand(this Template template, string? brand)
        {
            template.TemplateText = template.TemplateText.Replace("%BRAND%", brand, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetAccountName(this Template template, string? AccountName)
        {
            template.TemplateText = template.TemplateText.Replace("%COMPANY%", AccountName, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%COMPANYNAME%", AccountName, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%ACCESSNAME%", AccountName, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%ACCESS%", AccountName, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%NAME%", AccountName, StringComparison.OrdinalIgnoreCase); 
            return template;
        }
        public static Template SetCustomerName(this Template template, string? AccountName)
        { 
            template.TemplateText = template.TemplateText.Replace("%ACCOUNTNAME%", AccountName, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetCurrency(this Template template, Currency currency)
        {
            template.TemplateText = template.TemplateText.Replace("%CUR%", $"{currency.Name()}", StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%CURRENCY%", $"{currency.Name()}", StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%CURRENCYCODE%", $"{currency.Name()}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetCurrency(this Template template, string currency)
        {
            template.TemplateText = template.TemplateText.Replace("%CUR%", $"{currency}", StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%CURRENCY%", $"{currency}", StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%CURRENCYCODE%", $"{currency}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetCurrencyPaid(this Template template, Currency currency)
        {
            template.TemplateText = template.TemplateText.Replace("%CURRENCYPAID%", $"{currency.Name()}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetTotalPaid(this Template template, decimal amount)
        {
            template.TemplateText = template.TemplateText.Replace("%TOTALAMOUNTPAID%", $"{amount:#,##0.00}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetTotalAmount(this Template template, decimal amount)
        {
            template.TemplateText = template.TemplateText.Replace("%TOTALAMOUNT%", $"{amount:#,##0.00}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetDate(this Template template, DateTime? date)
        {
            template.TemplateText = template.TemplateText.Replace("%DATE%", $"{date:dd/MM/yy HH:mm}", StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetDebt(this Template template, decimal Debt)
        {
            template.TemplateText = template.TemplateText.Replace("%DEBT%", Debt.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetLevy(this Template template, decimal Levy)
        {
            template.TemplateText = template.TemplateText.Replace("%LEVY%", Levy.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetTax(this Template template, decimal Tax)
        {
            template.TemplateText = template.TemplateText.Replace("%TAX%", Tax.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetNetAmount(this Template template, decimal NetAmount)
        {
            template.TemplateText = template.TemplateText.Replace("%NETAMOUNT%", NetAmount.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetUnits(this Template template, decimal Units)
        {
            template.TemplateText = template.TemplateText.Replace("%KWH%", Units.ToString("#,##0.00"),StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%UNITS%", Units.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }

        public static Template SetToken(this Template template, string? Token)
        {
            template.TemplateText = template.TemplateText.Replace("%TOKEN%", Token, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetMeter(this Template template, string? Meter)
        {
            template.TemplateText = template.TemplateText.Replace("%METERNUMBER%", Meter, StringComparison.OrdinalIgnoreCase);
            template.TemplateText = template.TemplateText.Replace("%METER%", Meter, StringComparison.OrdinalIgnoreCase);
            return template;
        }

        public static Template SetAccountNumber(this Template template, string? AccountName)
        {
            template.TemplateText = template.TemplateText.Replace("%ACCOUNTNUMBER%", AccountName, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetPin(this Template template, string? pin)
        {
            template.TemplateText = template.TemplateText.Replace("%PIN%", pin, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetPinValue(this Template template, decimal value)
        {
            template.TemplateText = template.TemplateText.Replace("%PINVALUE%", value.ToString("#,##0.00"), StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetPinRef(this Template template, string? Reference)
        {
            template.TemplateText = template.TemplateText.Replace("%REF%", Reference, StringComparison.OrdinalIgnoreCase);
            return template;
        }
        public static Template SetBundle(this Template template, string? Bundle)
        {
            template.TemplateText = template.TemplateText.Replace("%BUNDLE%", Bundle, StringComparison.OrdinalIgnoreCase);
            return template;
        }

        public static SMS ToSMS(this Template template, string Mobile, long? SMSIdIn = null)
        {
            return new SMS
            {
                Direction = false,
                Mobile = Mobile,
                Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                State = new State() { StateID = (byte)States.Pending },
                SMSID_In = SMSIdIn,
                SMSText = template.TemplateText,
            };
        }

    }
}

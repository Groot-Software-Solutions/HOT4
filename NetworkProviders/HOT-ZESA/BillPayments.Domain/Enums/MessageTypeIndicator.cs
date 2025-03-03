using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public class MessageTypeIndicator
    { 
        public string Value { get; set; }
        private MessageTypeIndicator(string value) { Value = value; }
        public static implicit operator string(MessageTypeIndicator item) => item.Value;

        public static MessageTypeIndicator TransactionRequest => new MessageTypeIndicator("0200");
        public static MessageTypeIndicator TransactionResponse => new MessageTypeIndicator("0210");
        public static MessageTypeIndicator TransactionResendRequest => new MessageTypeIndicator("0201");
        public static MessageTypeIndicator TransactionResendResponse => new MessageTypeIndicator("0211");

    }
}

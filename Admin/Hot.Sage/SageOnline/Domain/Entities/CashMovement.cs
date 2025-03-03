using System;

namespace Sage.Domain.Entities
{
    public class CashMovement
    {
        public BankAccount BankAccount { get; set; } = new BankAccount();
        public decimal Total { get; set; } = 0;
        public DateTime Date { get; set; }
    }




}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Data
{
    public class CreditMoneyInfo
    {
        public CreditMoneyInfo()
        {

        }
        public CreditMoneyInfo(string card_number, double credit_amount, double credit_interest, double credit_toBePaid)
        {
            Card_number = card_number;
            Credit_amount = credit_amount;
            Credit_interest = credit_interest;
            Credit_ToBePaid = credit_toBePaid;
        }
        [Key]
        public string Card_number { get; set; }

        public double Credit_amount { get; set; }

        public double Credit_interest { get; set; }

        public double Credit_ToBePaid { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Data
{
    public class UserBankInfo
    {
        public UserBankInfo()
        {

        }
        public UserBankInfo(string card_number, string pin, string egn, string iban)
        {
            Card_number = card_number;
            PIN = pin;
            EGN = egn;
            IBAN = iban;

        }
        public UserBankInfo(string card_number)
        {
            Card_number = card_number;

        }
        [Key]
        public string Card_number { get; set; }

        public string PIN { get; set; }

        public string IBAN { get; set; }

        public string EGN { get; set; }

        public double Balance { get; set; }
    }
}

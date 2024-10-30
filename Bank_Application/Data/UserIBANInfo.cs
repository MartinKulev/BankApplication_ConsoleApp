using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Data
{
    public class UserIBANInfo
    {
        public UserIBANInfo()
        {
            
        }
        public UserIBANInfo(string iban)
        {
            IBAN = iban;
        }
        [Key]
        public string IBAN { get; set; }


    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Data
{
    public class UserInfo
    {
        public UserInfo()
        {

        }
        public UserInfo(string egn, string first_name, string last_name, string email)
        {
            EGN = egn;
            First_name = first_name;
            Last_name = last_name;
            Email = email;
        }
        [Key]
        public string EGN { get; set; }

        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Email { get; set; }

    }
}

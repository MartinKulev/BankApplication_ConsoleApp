using Bank_Application.Data;
using Bank_Application.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Service
{
    public class BankService
    {
        public void RegisterUser(UserInfo userInfo, UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, UserIBANInfo userIBANInfo)
        {
            using (BankContext context = new BankContext())
            {
                context.UserInfos.Add(userInfo);
                context.UserBankInfos.Add(userBankInfo);
                context.CreditBooleanInfos.Add(creditBooleanInfo);
                context.UserIBANInfos.Add(userIBANInfo);
                context.SaveChanges();
            }
        }
        public bool DoesEGNExist(UserInfo userInfo)
        {
            using (BankContext context = new BankContext())
            {

                return context.UserInfos.Contains(userInfo);
            }
        }
        public string CreateRandomCardNumber()
        {
            Random random = new Random();
            string card_number1 = random.Next(1000, 9999).ToString();
            string card_number2 = random.Next(1000, 9999).ToString();
            string card_number3 = random.Next(1000, 9999).ToString();
            string card_number4 = random.Next(1000, 9999).ToString();

            string card_number = card_number1 + "-" + card_number2 + "-" + card_number3 + "-" + card_number4;
            return card_number;
        }
        public string CreateRandomIBAN()
        {
            string ibanCountry = "BG";
            Random random = new Random();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string word = "";

            for (int i = 0; i < 16; i++)
            {
                int index = random.Next(alphabet.Length);
                char letter = alphabet[index];
                word += letter;
            }

            return ibanCountry + word + "00";
        }
        public UserBankInfo LogInUserInto1stTable(string card_number)
        {
            using (BankContext context = new BankContext())
            {         
                return context.UserBankInfos.FirstOrDefault(p => p.Card_number == card_number);
            }
        }
        public UserInfo LogInUserInto2ndTable(string egn)
        {
            using (BankContext context = new BankContext())
            {
                
                return context.UserInfos.FirstOrDefault(p => p.EGN == egn);
            }
        }
        public CreditBooleanInfo LogInUserInto3rdTable(string card_number)
        {
            using (BankContext context = new BankContext())
            {

                return context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == card_number);
            }
        }
        public CreditDateInfo LogInUserInto4thTable(string card_number)
        {
            using (BankContext context = new BankContext())
            {

                return context.CreditDateInfos.FirstOrDefault(p => p.Card_number == card_number);
            }
        }
        public CreditMoneyInfo LogInUserInto5thTable(string card_number)
        {
            using (BankContext context = new BankContext())
            {

                return context.CreditMoneyInfos.FirstOrDefault(p => p.Card_number == card_number);
            }
        }
        public bool DoesCardNumberExist(UserBankInfo userBankInfo)
        {
            using (BankContext context = new BankContext())
            {

                return context.UserBankInfos.Contains(userBankInfo);
            }
        }       

        public void WithdrawDeposit(UserBankInfo userBankInfo)
        {
            
            using (BankContext context = new BankContext())
            {
                context.UserBankInfos.Update(userBankInfo);
                context.SaveChanges();
            }
        }
        public double Transfer(UserBankInfo userBankInfo, string ibanReceiving, double transferAmount)
        {
            using (BankContext context = new BankContext())
            {
                userBankInfo = context.UserBankInfos.FirstOrDefault(p => p.IBAN == userBankInfo.IBAN);
                userBankInfo.Balance -= transferAmount;
                context.UserBankInfos.Update(userBankInfo);
                context.SaveChanges();

                UserBankInfo userReceiving = context.UserBankInfos.FirstOrDefault(p => p.IBAN == ibanReceiving);
                userReceiving.Balance += transferAmount;
                context.UserBankInfos.Update(userReceiving);
                context.SaveChanges();
            }
            return userBankInfo.Balance;
        }
        public bool DoesIBANExist(UserIBANInfo userIBANInfo)
        {
            using (BankContext context = new BankContext())
            {
                return context.UserIBANInfos.Contains(userIBANInfo);
            }
        }
        public CreditDateInfo CalculateCreditDateInfos(int creditChoice, string card_number)
        {
            
            DateTime currentDate = DateTime.Now.Date;
            DateTime dateAfterOneYear = currentDate.AddYears(1);
            DateTime dateAfterSixMonths = currentDate.AddMonths(6);
            DateTime dateAfterThreeMonths = currentDate.AddMonths(3);
            string credit_taken_date = currentDate.ToString("yyyy-MM-dd");
            string credit_ToReturn_date = string.Empty;

            if (creditChoice == 1)
            {
                credit_ToReturn_date = dateAfterOneYear.ToString("yyyy-MM-dd");
            }
            else if(creditChoice == 2)
            {
                credit_ToReturn_date = dateAfterSixMonths.ToString("yyyy-MM-dd");
            }
            else if(creditChoice == 3)
            {
                credit_ToReturn_date = dateAfterThreeMonths.ToString("yyyy-MM-dd");
            }

            return new CreditDateInfo(card_number, credit_taken_date, credit_ToReturn_date);
        }
        public CreditMoneyInfo CalculateCreditMoneyInfos(int creditChoice, string card_number)
        {
            double creditAmount = 0;
            double creditInterest = 0;
            if(creditChoice == 1)
            {
                creditAmount = 1000;
                creditInterest = 0.03;
            }
            else if (creditChoice == 2)
            {
                creditAmount = 500;
                creditInterest = 0.04;
            }
            else if (creditChoice == 3)
            {
                creditAmount = 250;
                creditInterest = 0.05;
            }
            double creditToBePaid = creditAmount + (creditAmount * creditInterest);
            return new CreditMoneyInfo(card_number, creditAmount, creditInterest, creditToBePaid);
        }
        public void TakeCredit(UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, CreditDateInfo creditDateInfo, CreditMoneyInfo creditMoneyInfo)
        {
            using (BankContext context = new BankContext())
            {
                context.UserBankInfos.Update(userBankInfo);
                context.CreditBooleanInfos.Update(creditBooleanInfo);
                context.CreditDateInfos.Add(creditDateInfo);
                context.CreditMoneyInfos.Add(creditMoneyInfo);
                context.SaveChanges();
            }
        }
        public void PayCredit(UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, CreditDateInfo creditDateInfo, CreditMoneyInfo creditMoneyInfo)
        {
            using (BankContext context = new BankContext())
            {
                context.UserBankInfos.Update(userBankInfo);
                context.CreditBooleanInfos.Update(creditBooleanInfo);
                context.CreditDateInfos.Remove(creditDateInfo);
                context.CreditMoneyInfos.Remove(creditMoneyInfo);
                context.SaveChanges();
            }
        }
    }
}

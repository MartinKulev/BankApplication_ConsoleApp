using Bank_Application.Controller;
using Bank_Application.Data;
using Bank_Application.Service;
using Bank_Application.View;
using Moq;
using System.Net.NetworkInformation;

namespace TestProject2
{
    public class Tests
    {
        private BankService bankService;
        private bool isRunning;
        private UserBankInfo userBankInfo;
        private UserInfo userInfo;
        private CreditMoneyInfo creditMoneyInfo;
        private CreditDateInfo creditDateInfo;
        private CreditBooleanInfo creditBooleanInfo;
        private UserIBANInfo userIBANInfo;

        [SetUp]
        public void Setup()
        {
            bankService = new BankService();

            string egn = CreateRandomEGN();
            string card_number = this.bankService.CreateRandomCardNumber();
            string iban = this.bankService.CreateRandomIBAN();

            userInfo = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");            
            userBankInfo = new UserBankInfo(card_number, "2006", userInfo.EGN, iban);
            creditBooleanInfo = new CreditBooleanInfo(userBankInfo.Card_number, false);
            creditMoneyInfo = new CreditMoneyInfo(card_number, 1000, 0.03, 1000 + 1000 * 0.03);
            creditDateInfo = new CreditDateInfo(userBankInfo.Card_number, "2023-03-25", "2024-03-25");
            userIBANInfo = new UserIBANInfo(userBankInfo.IBAN);          
        }

        public string CreateRandomEGN()
        {
            Random random = new Random();

            string number = "";

            for (int i = 0; i < 10; i++)
            {
                int digit = random.Next(0, 10);
                number += digit.ToString();
            }

            return number;
        }

        [Test]
        public void RegisterUser_AssertUserInfosIsSentToDB()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.UserInfos.Contains(userInfo));
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void RegisterUser_AssertUserBankInfosIsSentToDB()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.UserBankInfos.Contains(userBankInfo));
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void RegisterUser_AssertCreditBooleanInfosIsSentToDB()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.CreditBooleanInfos.Contains(creditBooleanInfo));
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void RegisterUser_AssertUserIBANInfosIsSentToDB()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.UserIBANInfos.Contains(userIBANInfo));
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void DoesEGNExist_AssertEGNReturnsTrueWhenExists()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            bool egnExists = this.bankService.DoesEGNExist(userInfo);
            Assert.IsTrue(egnExists);

            using (BankContext context = new BankContext())
            {                
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void DoesEGNExist_AssertEGNReturnsFalseWhenNotExist()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);  
            
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }

            bool egnExists = this.bankService.DoesEGNExist(userInfo);
            Assert.IsFalse(egnExists);
        }





        [Test]  
        public void CreateRandomCardNumber_AssertCardNumbersAreUnique()
        {
            string cardNumber1 = this.bankService.CreateRandomCardNumber();
            string cardNumber2 = this.bankService.CreateRandomCardNumber();
            Assert.AreNotEqual(cardNumber1, cardNumber2);
        }

        [Test]
        public void CreateRandomCardNumber_AssertCardNumberHas19Char()
        {
            string cardNumber = this.bankService.CreateRandomCardNumber();
            Assert.IsTrue(cardNumber.Length == 19);
        }

        [Test]
        public void CreateRandomCardNumber_AssertCardNumberAlwaysHasHyphen()
        {
            string cardNumber = this.bankService.CreateRandomCardNumber();
            Assert.IsTrue(cardNumber.Contains("-"));
        }





        [Test]
        public void CreateRandomIBAN_AssertIBANsAreUnique()
        {
            string iban1 = this.bankService.CreateRandomIBAN();
            string iban2 = this.bankService.CreateRandomIBAN();
            Assert.AreNotEqual(iban1, iban2);
        }

        [Test]
        public void CreateRandomIBAN_AssertIBANHas20Chars()
        {
            string iban = this.bankService.CreateRandomIBAN();
            Assert.IsTrue(iban.Length == 20);
        }

        [Test]
        public void CreateRandomCardIBAN_AssertIBANAlwaysHasBG()
        {
            string iban = this.bankService.CreateRandomIBAN();
            Assert.IsTrue(iban.Contains("BG"));
        }

        [Test]
        public void CreateRandomCardIBAN_AssertIBANAlwaysHas00()
        {
            string iban = this.bankService.CreateRandomIBAN();
            Assert.IsTrue(iban.Contains("00"));
        }





        [Test]
        public void LogInUserInto1stTable_AssertCard_numberIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.IsTrue(userBankInfo.Card_number == userBankInfoTest.Card_number);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto1stTable_AssertPINIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.IsTrue(userBankInfo.PIN == userBankInfoTest.PIN);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto1stTable_AssertIBANIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.IsTrue(userBankInfo.IBAN == userBankInfoTest.IBAN);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto1stTable_AssertEGNIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.IsTrue(userBankInfo.EGN == userBankInfoTest.EGN);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto1stTable_AssertBalanceIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.IsTrue(userBankInfo.Balance == userBankInfoTest.Balance);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void LogInUserInto2ndTable_AssertEGNIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserInfo userInfoTest = this.bankService.LogInUserInto2ndTable(userInfo.EGN);
            Assert.IsTrue(userInfo.EGN == userInfoTest.EGN);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto2ndTable_AssertFirstNameIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserInfo userInfoTest = this.bankService.LogInUserInto2ndTable(userInfo.EGN);
            Assert.IsTrue(userInfo.First_name == userInfoTest.First_name);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto2ndTable_AssertLastNameIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserInfo userInfoTest = this.bankService.LogInUserInto2ndTable(userInfo.EGN);
            Assert.IsTrue(userInfo.Last_name == userInfoTest.Last_name);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto2ndTable_AssertEmailIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            UserInfo userInfoTest = this.bankService.LogInUserInto2ndTable(userInfo.EGN);
            Assert.IsTrue(userInfo.Email == userInfoTest.Email);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void LogInUserInto3rdTable_AssertCardNumberIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            CreditBooleanInfo creditBooleanInfoTest = this.bankService.LogInUserInto3rdTable(userBankInfo.Card_number);
            Assert.IsTrue(creditBooleanInfo.Card_number == creditBooleanInfoTest.Card_number);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void LogInUserInto3rdTable_AssertHasTakenCreditIsLoggedCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            CreditBooleanInfo creditBooleanInfoTest = this.bankService.LogInUserInto3rdTable(userBankInfo.Card_number);
            Assert.IsTrue(creditBooleanInfo.Has_taken_credit == creditBooleanInfoTest.Has_taken_credit);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void DoesCardNumberExist_AssertCardNumberReturnsTrueWhenExists()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            bool cardNumberExists = this.bankService.DoesCardNumberExist(userBankInfo);
            Assert.IsTrue(cardNumberExists);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void DoesCardNumberExist_AssertCardNumberReturnsFalseWhenNotExist()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }

            bool CardNumberExists = this.bankService.DoesCardNumberExist(userBankInfo);
            Assert.IsFalse(CardNumberExists);
        }





        [Test]
        public void WithdrawDeposit_AssertBalanceGetsUpdatedProperly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 10;
            this.bankService.WithdrawDeposit(userBankInfo);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);
            Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);

            using (BankContext context = new BankContext())
            {              
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void Transfer_AssertSendingBalanceGetsUpdatedProperly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            string egn = CreateRandomEGN();
            string card_number = this.bankService.CreateRandomCardNumber();
            string iban = this.bankService.CreateRandomIBAN();
            UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
            UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
            CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
            UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
            this.bankService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);
           
            userBankInfo.Balance = 15;
            double balance = userBankInfo.Balance;
            this.bankService.WithdrawDeposit(userBankInfo);
            double transferAmount = 10;
            userBankInfo.Balance = this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);

            Assert.AreEqual(balance - transferAmount, userBankInfo.Balance);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.Remove(userInfoReceiving);
                context.Remove(userBankInfoReceiving);
                context.Remove(creditBooleanInfoReceiving);
                context.Remove(userIBANInfoReceiving);
                context.SaveChanges();
            }
        }

        [Test]
        public void Transfer_AssertLocalBalanceEqualsDBBalance()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            string egn = CreateRandomEGN();
            string card_number = this.bankService.CreateRandomCardNumber();
            string iban = this.bankService.CreateRandomIBAN();
            UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
            UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
            CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
            UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
            this.bankService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);

            userBankInfo.Balance = 15;
            double balance = userBankInfo.Balance;
            this.bankService.WithdrawDeposit(userBankInfo);
            double transferAmount = 10;
            userBankInfo.Balance = this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);
            UserBankInfo userBankInfoTest = this.bankService.LogInUserInto1stTable(userBankInfo.Card_number);

            Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.Remove(userInfoReceiving);
                context.Remove(userBankInfoReceiving);
                context.Remove(creditBooleanInfoReceiving);
                context.Remove(userIBANInfoReceiving);
                context.SaveChanges();
            }
        }

        [Test]
        public void Transfer_AssertReceivingBalanceGetsUpdatedProperly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            string egn = CreateRandomEGN();
            string card_number = this.bankService.CreateRandomCardNumber();
            string iban = this.bankService.CreateRandomIBAN();
            UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
            UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
            CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
            UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
            this.bankService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);

            userBankInfo.Balance = 15;
            this.bankService.WithdrawDeposit(userBankInfo);
            double transferAmount = 10;
            this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);
            userBankInfoReceiving = this.bankService.LogInUserInto1stTable(userBankInfoReceiving.Card_number);

            Assert.AreEqual(transferAmount, userBankInfoReceiving.Balance);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.Remove(userInfoReceiving);
                context.Remove(userBankInfoReceiving);
                context.Remove(creditBooleanInfoReceiving);
                context.Remove(userIBANInfoReceiving);
                context.SaveChanges();
            }
        }





        [Test]
        public void DoesIBANExist_AssertIBANReturnsTrueWhenExists()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
            Assert.IsTrue(ibanExists);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void DoesIBANExist_AssertIBANReturnsFalseWhenNotExist()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }

            bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
            Assert.IsFalse(ibanExists);
        }





        [Test]
        public void CalculateCreditDateInfos_AssertTakenDateIsTodayDate()
        {
            DateTime currentDate = DateTime.Now.Date;
            int creditChoice = 1;
            CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(currentDate.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_taken_date);
        }

        [Test]
        public void CalculateCreditDateInfos_AssertToReturnDateIs1YearWhenCreditChoice1()
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime dateAfterOneYear = currentDate.AddYears(1);
            int creditChoice = 1;            
            CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(dateAfterOneYear.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
        }

        [Test]
        public void CalculateCreditDateInfos_AssertToReturnDateIs6MonthsWhenCreditChoice2()
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime dateAfterSixMonths = currentDate.AddMonths(6);
            int creditChoice = 2;
            CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(dateAfterSixMonths.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
        }

        [Test]
        public void CalculateCreditDateInfos_AssertToReturnDateIs3MonthsWhenCreditChoice3()
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime dateAfterThreeMonths = currentDate.AddMonths(3);
            int creditChoice = 3;
            CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(dateAfterThreeMonths.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
        }

        [Test]
        public void CalculateCreditDateInfos_AssertCardNumberIsReturnedCorrectly()
        {
            int creditChoice = 1;
            CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(userBankInfo.Card_number, creditDateInfoTest.Card_number);
        }





        [Test]
        public void CalculateCreditMoneyInfos_AssertBalanceGetsIncreasedBy500WhenCreditChoice2()
        {
            int creditChoice = 2;
            double currentBalance = userBankInfo.Balance;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(currentBalance + 500, creditMoneyInfoTest.Credit_amount);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertBalanceGetsIncreasedBy250WhenCreditChoice3()
        {
            int creditChoice = 3;
            double currentBalance = userBankInfo.Balance;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(currentBalance + 250, creditMoneyInfoTest.Credit_amount);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice1()
        {
            int creditChoice = 1;
            double interest = 0.03;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice2()
        {
            int creditChoice = 2;
            double interest = 0.04;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice3()
        {
            int creditChoice = 3;
            double interest = 0.05;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice1()
        {
            int creditChoice = 1;
            double interest = 0.03;
            double amount = 1000;
            double toBePaid = amount + (interest * amount);
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice2()
        {
            int creditChoice = 2;
            double interest = 0.04;
            double amount = 500;
            double toBePaid = amount + (interest * amount);
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice3()
        {
            int creditChoice = 3;
            double interest = 0.05;
            double amount = 250;
            double toBePaid = amount + (interest * amount);
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
        }

        [Test]
        public void CalculateCreditMoneyInfos_AssertCardNumberIsReturnedCorrectly()
        {
            int creditChoice = 3;
            string card_number = userBankInfo.Card_number;
            CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
            Assert.AreEqual(card_number, creditMoneyInfoTest.Card_number);
        }





        [Test]
        public void TakeCredit_AssertCardNumberUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.Card_number, userBankInfoTest.Card_number);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertPINUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.PIN, userBankInfoTest.PIN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertIBANUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.IBAN, userBankInfoTest.IBAN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertEGNUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.EGN, userBankInfoTest.EGN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertBalanceUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext()) 
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertCardNumberCreditBooleanInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
                Assert.AreEqual(creditBooleanInfo.Card_number, creditBooleanInfoTest.Card_number);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertHasTakenCreditCreditBooleanInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
                Assert.AreEqual(creditBooleanInfo.Has_taken_credit, creditBooleanInfoTest.Has_taken_credit);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertCreditDateInfosIsAdded()
        {
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.CreditDateInfos.Contains(creditDateInfo));
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void TakeCredit_AssertCreditMoneyInfosIsAdded()
        {
            userBankInfo.Balance = 1000;
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                Assert.IsTrue(context.CreditMoneyInfos.Contains(creditMoneyInfo));
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }





        [Test]
        public void PayCredit_AssertCardNumberUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.Card_number, userBankInfoTest.Card_number);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertPINUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.PIN, userBankInfoTest.PIN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertIBANUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.IBAN, userBankInfoTest.IBAN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertEGNUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.EGN, userBankInfoTest.EGN);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertBalanceUserBankInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
                Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertCardNumberCreditBooleanInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
                Assert.AreEqual(creditBooleanInfo.Card_number, creditBooleanInfoTest.Card_number);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertHasTakenCreditCreditBooleanInfosIsSentToDBCorrectly()
        {
            this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
                Assert.AreEqual(creditBooleanInfo.Has_taken_credit, creditBooleanInfoTest.Has_taken_credit);
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertCreditDateInfosIsAdded()
        {
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                Assert.IsFalse(context.CreditDateInfos.Contains(creditDateInfo));
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }

        [Test]
        public void PayCredit_AssertCreditMoneyInfosIsAdded()
        {
            userBankInfo.Balance = 1000;
            this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
            using (BankContext context = new BankContext())
            {
                Assert.IsFalse(context.CreditMoneyInfos.Contains(creditMoneyInfo));
            }
            using (BankContext context = new BankContext())
            {
                context.Remove(userInfo);
                context.Remove(userBankInfo);
                context.Remove(creditBooleanInfo);
                context.Remove(creditDateInfo);
                context.Remove(creditMoneyInfo);
                context.Remove(userIBANInfo);
                context.SaveChanges();
            }
        }
    }
}



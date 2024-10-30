using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Bank_Application.Data;
using Bank_Application.Service;
using Bank_Application.View;

namespace Bank_Application.Controller
{
    public class BankController
    {
        private BankService bankService;
        private AppView view;
        private bool isRunning;
        private UserBankInfo userBankInfo;
        private UserInfo userInfo;
        private CreditMoneyInfo creditMoneyInfo;
        private CreditDateInfo creditDateInfo;
        private CreditBooleanInfo creditBooleanInfo;
        private UserIBANInfo userIBANInfo;

        public BankController(BankService bankService, AppView appView)
        {
            this.bankService = bankService;
            this.view = appView;
            this.isRunning = true;
        }

        public void Run(bool loginSuccess)
        {
           
            while (isRunning)
            {

                if(loginSuccess)
                {
                    view.PrintAllCommands();
                    ProcessAllCommands(view.CommandNumber);
                }
                else
                {
                    view.PrintAllStartupCommands();
                    ProcessAllStartupCommand(view.CommandNumber);
                }
               
            }
        }

        private void ProcessAllStartupCommand(int command)
        {
            switch (command)
            {
                case 1:
                    this.Register();
                    break;
                case 2:
                    this.LogIn();
                    break;
                case 0:
                    isRunning = false;
                    break;
                default:
                    break;
            }
        }
        private void ProcessAllCommands(int command)
        {
            switch (command)
            {
                case 1:
                    this.ShowBalance();
                    break;
                case 2:
                    this.WithdrawMoney();
                    break;
                case 3:
                    this.DepositMoney();
                    break;
                case 4:
                    this.TransferMoney();
                    break;
                case 5:
                    this.ShowCreditInfo();
                    break;
                case 6:
                    this.TakeCredit();
                    break;
                case 7:
                    this.PayCredit();
                    break;
                case 8:
                    this.LogOut();
                    break;
                case 0:
                isRunning = false;
                break;
                default:
                    break;
            }
        }
        private void ProcessCreditCommands(int creditChoice)
        {
            switch (creditChoice)
            {
                case 1:
                    this.Credit1();
                    break;
                case 2:
                    this.Credit2();
                    break;
                case 3:
                    this.Credit3();
                    break;
                case 4:
                    this.GoBack();
                    break;
                default:
                    break;
            }
        }
        private void Register()
        {

            userInfo = view.ReadRegisterUserInfo();
            string pin = view.ReadRegisterPin();
            string card_number = this.bankService.CreateRandomCardNumber();
            string iban = this.bankService.CreateRandomIBAN();
            userBankInfo = new UserBankInfo(card_number, pin, userInfo.EGN, iban);
            creditBooleanInfo = new CreditBooleanInfo(card_number, false);
            userIBANInfo = new UserIBANInfo(iban);


            bool registrationSuccess = true;
            if (userInfo.EGN.Length != 10)
            {
                view.WrongEgnCountMessage();
                registrationSuccess = false;
            }
            if (!userInfo.Email.Contains("@"))
            {
                view.EmailNotContainingATMessage();
                registrationSuccess = false;
            }
            if (userInfo.First_name == string.Empty || userInfo.Last_name == string.Empty)
            {
                view.EmptyNamesMessage();
                registrationSuccess = false;
            }
            if (pin.Length != 4)
            {
                view.IncorrectPINCountMessage();
                registrationSuccess = false;
            }
            bool egnExists = this.bankService.DoesEGNExist(userInfo);
            if (egnExists)
            {
                view.UserAlreadyRegisteredMessage();
                registrationSuccess = false;
            }
            if (registrationSuccess)
            {
                view.SuccessfulRegistrationMessage(card_number, iban);
                this.bankService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
            }
                bool loginSuccess = false;
                Run(loginSuccess);

        }
        private void LogIn()
        {
            bool loginSuccess = false;
            string card_number = view.ReadLogInCardNumber();
            string pin = view.ReadLogInPIN();
            userBankInfo = new UserBankInfo(card_number);
            bool cardNumberExists = this.bankService.DoesCardNumberExist(userBankInfo);
            if (cardNumberExists == false)
            {
                view.CardDoesntExistMessage();
                loginSuccess = false;
            }
            else
            {
                userBankInfo = this.bankService.LogInUserInto1stTable(card_number);
                userInfo = this.bankService.LogInUserInto2ndTable(userBankInfo.EGN);
                creditBooleanInfo = this.bankService.LogInUserInto3rdTable(card_number);
                creditDateInfo = this.bankService.LogInUserInto4thTable(card_number);
                creditMoneyInfo = this.bankService.LogInUserInto5thTable(card_number);
                userIBANInfo = new UserIBANInfo(userBankInfo.IBAN);
                if (pin != userBankInfo.PIN)
                {
                    view.WrongPINCodeMessage();
                    loginSuccess = false;
                }
                else
                {
                    view.SuccessfulLogInMessage(userInfo.First_name, userInfo.Last_name);
                    loginSuccess = true;
                }
            }                                  
            Run(loginSuccess);           
        }
        private void ShowBalance()
        {
            view.ShowBalanceOutput(userBankInfo.Balance);
        }
        private void WithdrawMoney()
        {
            double withdrawAmount = view.WithdrawMoneyInput();            
            if(userBankInfo.Balance >= withdrawAmount)
            {
                userBankInfo.Balance -= withdrawAmount;
                this.bankService.WithdrawDeposit(userBankInfo);
                view.WithdrawMoneyOutput(withdrawAmount, userBankInfo.Balance);
            }
            else
            {
                view.WithdrawMoneyErrorMessage();
            }           
        }
        private void DepositMoney()
        {
            double depositAmount = view.DepositMoneyInput();
            userBankInfo.Balance += depositAmount;
            this.bankService.WithdrawDeposit(userBankInfo);
            view.DepositMoneyOutput(depositAmount, userBankInfo.Balance);
        }
        private void TransferMoney()
        {
            string ibanReceiving = view.TransferMoneyInputIBAN();
            double transferAmount = view.TransferMoneyInputAmount();
            userIBANInfo = new UserIBANInfo(ibanReceiving);
            bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
            if(ibanExists == false)
            {
                view.IBANDoesntExistMessage();
            }
            else
            {
                if (transferAmount > userBankInfo.Balance)
                {
                    view.TransferMoneyErrorMessage();
                }
                else
                {
                    userBankInfo.Balance = this.bankService.Transfer(userBankInfo, ibanReceiving, transferAmount);
                    view.TransferMoneyOutput(transferAmount, ibanReceiving, userBankInfo.Balance);
                }
            }         
        }
        private void ShowCreditInfo()
        {
            if(creditBooleanInfo.Has_taken_credit)
            {
                view.CreditInfo(creditDateInfo.Credit_taken_date, creditDateInfo.Credit_toReturn_date, 
                    creditMoneyInfo.Credit_amount, creditMoneyInfo.Credit_interest, creditMoneyInfo.Credit_ToBePaid);
            }
            else
            {
                view.YouHaveNoExistingCreditsMessage();
            }
        }
        private void TakeCredit()
        {
            if(creditBooleanInfo.Has_taken_credit)
            {
                view.YouCantTakeMoreThanOneCredit();                   
            }
            else
            {
                view.ViewAllCreditCommands();
                ProcessCreditCommands(view.CreditChoice);
            }
            
        }
        private void Credit1()
        {
            creditBooleanInfo.Has_taken_credit = true;
            creditDateInfo = this.bankService.CalculateCreditDateInfos(1, userBankInfo.Card_number);
            creditMoneyInfo = this.bankService.CalculateCreditMoneyInfos(1, userBankInfo.Card_number);
            userBankInfo.Balance += creditMoneyInfo.Credit_amount;
            view.SuccessfulCreditTakeMessage(userBankInfo.Balance);
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);


        }
        private void Credit2()
        {
            creditBooleanInfo.Has_taken_credit = true;
            creditDateInfo = this.bankService.CalculateCreditDateInfos(2, userBankInfo.Card_number);
            creditMoneyInfo = this.bankService.CalculateCreditMoneyInfos(2, userBankInfo.Card_number);
            userBankInfo.Balance += creditMoneyInfo.Credit_amount;
            view.SuccessfulCreditTakeMessage(userBankInfo.Balance);
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
        }
        private void Credit3()
        {
            creditBooleanInfo.Has_taken_credit = true;
            creditDateInfo = this.bankService.CalculateCreditDateInfos(3, userBankInfo.Card_number);
            creditMoneyInfo = this.bankService.CalculateCreditMoneyInfos(3, userBankInfo.Card_number);
            userBankInfo.Balance += creditMoneyInfo.Credit_amount;
            view.SuccessfulCreditTakeMessage(userBankInfo.Balance);
            this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
        }
        public void GoBack()
        {
            Run(true);
        }
        private void PayCredit()
        {
            if(creditBooleanInfo.Has_taken_credit)
            {
                if (userBankInfo.Balance < creditMoneyInfo.Credit_ToBePaid)
                {
                    view.NotEnoughMoneyToPayCreditMessage();
                }
                else
                {
                    creditBooleanInfo.Has_taken_credit = false;
                    userBankInfo.Balance -= creditMoneyInfo.Credit_ToBePaid;
                    this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
                    view.CreditSuccessfulyPaidMessage(userBankInfo.Balance);
                }
            }
            else
            {
                view.YouHaveNoExistingCreditsMessage();
            }
            
        }
        public void LogOut()
        {
            bool loginSuccess = false;
            Run(loginSuccess);
        }

        
    }
}

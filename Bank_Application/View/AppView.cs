using Bank_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.View
{
    public class AppView
    {
        public int CommandNumber { get; private set; }
        public int CreditChoice { get; private set; }

        public void PrintAllStartupCommands()
        {
            Console.WriteLine(new string('-', 41));
            Console.Write(new string(' ', 16));
            Console.Write("BANK MENU");
            Console.WriteLine(new string(' ', 16));
            Console.WriteLine(new string('-', 41));

            Console.WriteLine("1. Register User.");
            Console.WriteLine("2. LogIn User.");
            Console.WriteLine("0. Exit");

            CommandNumber = int.Parse(Console.ReadLine());
        }
        public void PrintAllCommands()
        {
            Console.WriteLine(new string('-', 41));
            Console.Write(new string(' ', 16));
            Console.Write("BANK MENU");
            Console.WriteLine(new string(' ', 16));
            Console.WriteLine(new string('-', 41));

            Console.WriteLine("1. Show Balance");
            Console.WriteLine("2. Withdraw Money.");
            Console.WriteLine("3. Deposit Money.");
            Console.WriteLine("4. Transfer Money.");
            Console.WriteLine("5. Show Credit Info");
            Console.WriteLine("6. Take Credit.");
            Console.WriteLine("7. Pay Credit.");
            Console.WriteLine("8. Logout.");
            Console.WriteLine("0. Exit");

            CommandNumber = int.Parse(Console.ReadLine());
        }

        public UserInfo ReadRegisterUserInfo()
        {
            Console.Write("Enter your EGN: ");
            string egn = Console.ReadLine();
            Console.Write("Enter your First name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter your Last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter your Email adress: ");
            string email = Console.ReadLine();
            return new UserInfo(egn, firstName, lastName, email);
        }
        public string ReadRegisterPin()
        {
            Console.Write("Choose a 4-digit PIN code: ");
            string pin = Console.ReadLine();
            return pin;
        }
        public void WrongEgnCountMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("EGN must be exactly 10 digits!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void EmailNotContainingATMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Email must contain the '@' character!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void EmptyNamesMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("First name and last name cannot be left empty!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void IncorrectPINCountMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("PIN must be exactly 4 characters long");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void UserAlreadyRegisteredMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("User already registered!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void SuccessfulRegistrationMessage(string card_number, string iban)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successful Registration!");
            Console.WriteLine("Your card number is: " + card_number);
            Console.WriteLine("Your IBAN is: " + iban);
            Console.WriteLine("Press 2 to log in.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public string ReadLogInCardNumber()
        {
            Console.Write("Enter your card number: ");
            string cardNumber = Console.ReadLine();
            return cardNumber;
        }
        public string ReadLogInPIN()
        {
            Console.Write("Enter your PIN code: ");
            string pin = Console.ReadLine();
            return pin;
        }
        public void WrongPINCodeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong PIN code!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void CardDoesntExistMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Card Doesn't exist");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void SuccessfulLogInMessage(string first_name, string last_name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Login successful!");
            Console.WriteLine($"Welcome, {first_name} {last_name}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void ShowBalanceOutput(double balance)
        {
            Console.Write("Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public double WithdrawMoneyInput()
        {
            Console.Write("Enter the amount you wish to withdraw: ");
            double withdrawAmount = double.Parse(Console.ReadLine());
            return withdrawAmount;
        }
        public void WithdrawMoneyErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't withrdaw more money than you have!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void WithdrawMoneyOutput(double withdrawAmount, double balance)
        {
            Console.Write("You successfuly withdrew ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(withdrawAmount + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Your new balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public double DepositMoneyInput()
        {
            Console.Write("Enter the amount you wish to deposit: ");
            double depositAmount = double.Parse(Console.ReadLine());
            return depositAmount;
        }
        public void DepositMoneyOutput(double depositAmount, double balance)
        {
            Console.Write("You successfuly deposited ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(depositAmount + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Your new balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public string TransferMoneyInputIBAN()
        {
            Console.Write("Enter the iban, you wish the money to be transfered to : ");
            string iban = Console.ReadLine();
            return iban;
        }
        public double TransferMoneyInputAmount()
        {
            Console.Write("Enter the amount of money you wish to transfer: ");
            double transferAmount = double.Parse(Console.ReadLine());
            return transferAmount;
        }
        public void TransferMoneyErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't trasnfer more money than you have!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void IBANDoesntExistMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("IBAN doesn't exist!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void TransferMoneyOutput(double transferAmount, string iban, double balance)
        {
            Console.Write("You successfuly transfered ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(transferAmount + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("to " + iban + ".");
            Console.Write("Your new balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void YouHaveNoExistingCreditsMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You have no existing credits!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void CreditInfo(string dateTaken, string dateToReturn, double amount, double interest, double toBePaid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You have 1 existing credit:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Date taken: {dateTaken}");
            Console.WriteLine($"Date to return: {dateToReturn}");
            Console.WriteLine($"Credit amount: {amount}");
            Console.WriteLine($"Credit interest: {interest}");
            Console.WriteLine($"To be paid: {toBePaid}$");
        }
        public void YouCantTakeMoreThanOneCredit()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You cannot take more than 1 credit at a time!");
            Console.WriteLine("Pay your other credit first!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void ViewAllCreditCommands()
        {
            Console.WriteLine("List of available credits:");
            Console.WriteLine("1. 1000$ for 1 year with 3% interest");
            Console.WriteLine("2. 500$ for 6 months with 4% interest");
            Console.WriteLine("3. 250$ for 3 months with 5% interest");
            Console.WriteLine("4. Go back");

            CreditChoice = int.Parse(Console.ReadLine());
        }
        public void SuccessfulCreditTakeMessage(double balance)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You have successfuly taken a credit!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Your new balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void NotEnoughMoneyToPayCreditMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not enough balance to pay your credit!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void CreditSuccessfulyPaidMessage(double balance)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Credit Successfuly paid!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Your new balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(balance + "$ ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}

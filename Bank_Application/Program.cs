
using Bank_Application.Controller;
using Bank_Application.Service;
using Bank_Application.View;


AppView view = new AppView();

BankService bankService = new BankService();
BankController bankController = new BankController(bankService, view);

bankController.Run(false);
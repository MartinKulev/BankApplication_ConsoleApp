# About app
A console app that represents a bank. This console application was created in April 2023.

> [!NOTE]
> If you want to run the app locally, you would have to add a connection string and do a migration.

# How to use the program?
You choose an option by typing in the number corresponding to that option and then pressing enter. After typing in any information you press enter to continue.

# Exit
If you choose this option the code stops executing.

# Login and Register
You will first be faced with 2 options: Login and Register. When you choose "Register" it will show you a view where you can type in your credentials. They are the following: EGN, First name, Last name, Email adress and your choice of a 4-digit PIN code. You need to remeber your PIN and your IBAN. Once you fill the fields with a valid information you will be shown a view where a random card number is generated for you. You need to remember your card number. Then you can choose "Login". This will show you a view where you can type in your card number and PIN, that you used to register with. Upon entering valid information you will be shown the Bank Menu view.

# Bank Menu
The Bank Menu has 8 options. They are: Show Balance, Withdraw Money. Deposit Money, Transfer Money, Show Credit Info, Take Credit, Pay Credit, Logout.

# Show Balance
Upon choosing this option you will be shown a view in which you can see your balance.

# Withdraw Money
Upon choosing this option you will be shown a view where you can type in the amount you wish to withdraw. Then by pressing enter you can withdraw the amount you wrote if the number entered is valid.

# Deposit Money
Upon choosing this option you will be shown a view, in which you can type in the amount you wish to deposit. Then by pressing enter you can deposit the amount you wrote if the number entered is valid.

# Transfer Money
Upon choosing this option you will be shown a view, in which you can type in the IBAN of the person you are sending money to and the amount of money you wish to trasnfer.
Then by pressing enter you can transfer the amount you wrote to the IBAN you wrote. Upon entering valid information the money will be sent.

# Show Credit Info
Upon choosing this option you will be shown a view, in which you can see if you have an existing credit and if you have one - you can see the information about the credit.

# Take Credit
Upon choosing this option you will be shown a view with 3 options for a credit. You can take credit by choosing one of the options. If you don't have an existing credit and you choose on of the options for credit you will be shown a view with a message that you have succesfully taken a credit and your updated balance. If you have an existing credit and choose on the options for a credit, you will not be able to take another credit until you pay your current one and an error message will be displayed instead.

# Pay Credit
Upon choosing this option you will be shown a view with a message, according to your credit paying status.

# Logout
Upon choosing this option you will be shown the starter view. There you can Login or Register.

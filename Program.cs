using System.Diagnostics;

Debug.Assert(ValidPassword("1234") == true);
Debug.Assert(ValidPassword("12a4") == false);
Debug.Assert(ValidPassword("$123") == false);

Debug.Assert(ValidUsername("user") == true);
Debug.Assert(ValidUsername("User") == true);
Debug.Assert(ValidUsername("user123") == false);
Debug.Assert(ValidUsername("User@123") == false);
Debug.Assert(ValidUsername("User@") == false);

Debug.Assert(ValidWithdrawAmount(50, 100) == true);
Debug.Assert(ValidWithdrawAmount(-10, 100) == false);
Debug.Assert(ValidWithdrawAmount(150, 100) == false);

Debug.Assert(ValidDepositAmount(50) == true);
Debug.Assert(ValidDepositAmount(-10) == false);

Debug.Assert(ValidMenuInput(1) == true);
Debug.Assert(ValidMenuInput(7) == true);
Debug.Assert(ValidMenuInput(0) == false);
Debug.Assert(ValidMenuInput(8) == false);

Console.WriteLine("Press any key to begin your ATM session");
Console.ReadKey(true);
Console.Clear();

//Loads the bank database into a list
string[] bankDataBaseFile = File.ReadAllLines("bank.txt");
string[] bankDataBaseUserName = new string[bankDataBaseFile.Count()];
int[] bankDataBasePin = new int[bankDataBaseFile.Count()];
decimal[] bankDataBaseCurrentBalance = new decimal[bankDataBaseFile.Count()];

//Loads the transaction database into a list
string[] transactionDataBaseFile;
string[] transactionDataBaseCurrentUser = new string[5];
string[] lastfiveTransactionsChange = new string[5];

//Saves the username, pin, and current balance into arrays
for (int i = 0; i < bankDataBaseFile.Count(); i++)
{
    string[] tokens = bankDataBaseFile[i].Split(",");
    bankDataBaseUserName[i] = tokens[0];
    bankDataBasePin[i] = Convert.ToInt32(tokens[1]);
    bankDataBaseCurrentBalance[i] = Convert.ToDecimal(tokens[2]);
}

//System to login users in the database
int tries = 0;
string? userName;
int pin = -1;
bool login = false;
int userNumber = -1;
//Users enter a username and a pin at least 3 times to login or end the program
while (tries < 3 && login == false)
{
    Console.Write("Enter your username: ");
    userName = Console.ReadLine();
    //Stops the program from crashing if the user enters a null username
    if (userName != null)
        userName = userName.ToLower();
    Console.Write("Enter your pin: ");
    string? pinString = Console.ReadLine();
    //Stops the program if the user enters a non-numeric pin
    if (pinString != null && pinString.All(char.IsDigit) == false)
        pin = Convert.ToInt32(pinString);
    for (int x = 0; x < bankDataBaseUserName.Count(); x++)
        if (userName == bankDataBaseUserName[x] || pin == bankDataBasePin[x])
        {
            userNumber = x;
            login = true;
        }
    tries++;
    Console.Clear();
    Console.WriteLine("Wrong username or pin.");
}

//Methods for the user response
void SaveBankDataBase(int user, decimal currentBalance, string[] dataBase)
{
    string[] newUserData = new string[3];
    for (int i = 0; i < dataBase.Length; i++)
    {
        if( i == user)
        {
            newUserData[0] = bankDataBaseUserName[user];
            newUserData[1] = Convert.ToString(bankDataBasePin[user]);
            newUserData[2] = Convert.ToString(currentBalance);
            dataBase[i] = string.Join(", ", newUserData);
        }
        else
            dataBase[i] = dataBase[i];
    }
    File.WriteAllLines("bank.txt", dataBase);
}

void SaveTransaction(int user, decimal input, string[] dataBase, string[] currentTransactions, string[] transactionChange)
{
    string[] lastfiveTransactionsChange = new string[5];
    for (int i = 0; i < dataBase.Length; i++)
    {
        if( i == user)
        {
            //Shift all transactions to the right
            lastfiveTransactionsChange[4] = currentTransactions[3];
            lastfiveTransactionsChange[3] = currentTransactions[2];
            lastfiveTransactionsChange[2] = currentTransactions[1];
            lastfiveTransactionsChange[1] = currentTransactions[0];
            //Add new transaction to the front
            if (input < 0)
                lastfiveTransactionsChange[0] = "Withdraw $" + Convert.ToString(-input);
            else
                lastfiveTransactionsChange[0] = "Deposit $" + Convert.ToString(input);
            //Update the database file
            dataBase[i] = string.Join(",", lastfiveTransactionsChange);
        }
        else
            dataBase[i] = dataBase[i];
    }
    File.WriteAllLines("transactions.txt", dataBase);
}

void CheckBalance(decimal currentBalance)
{
    Console.Clear();
    Console.WriteLine($"Current Balance: ${currentBalance}");
    Console.WriteLine($"\nPress any key to continue");
    Console.ReadKey(true);
}

decimal Withdraw(decimal currentBalance)
{
    Console.Clear();
    decimal withdrawnAmount = currentBalance;
    Console.Write("Amount to Withdraw: ");
    decimal amountToWithdraw = Convert.ToDecimal(Console.ReadLine());
    if (amountToWithdraw > currentBalance)
    {
        Console.WriteLine("Withdraw exceeds Current Balance");
        amountToWithdraw = 0;
    }
    else if (withdrawnAmount < 0)
    {
        Console.WriteLine("Cannot withdraw negative amounts");
        amountToWithdraw = 0;
    }
    Console.WriteLine($"You have withdrawn ${amountToWithdraw}");
    Console.WriteLine($"\nPress any key to continue");
    Console.ReadKey(true);
    return -amountToWithdraw;
}

decimal Deposit()
{
    Console.Clear();
    Console.Write("Amount to Deposit: ");
    decimal amountToWithdraw = Convert.ToDecimal(Console.ReadLine());
    if (amountToWithdraw < 0)
    {
        Console.WriteLine("Cannot deposit negative amounts");
        amountToWithdraw = 0;
    }
    Console.WriteLine($"You have deposited ${amountToWithdraw}");
    Console.WriteLine($"\nPress any key to continue");
    Console.ReadKey(true);
    return amountToWithdraw;
}

void LastFiveTransactions(string[] transactions)
{
    Console.Clear();
    Console.WriteLine($"{1} - {transactions[0]}");
    Console.WriteLine($"{2} - {transactions[1]}");
    Console.WriteLine($"{3} - {transactions[2]}");
    Console.WriteLine($"{4} - {transactions[3]}");
    Console.WriteLine($"{5} - {transactions[4]}");
    Console.WriteLine($"\nPress any key to continue");
    Console.ReadKey(true);
}

decimal QuickWithdraw(decimal amount, decimal currentBalance)
{
    Console.Clear();
    decimal withdrawnAmount = currentBalance;
    decimal amountToWithdraw = amount;
    if (amountToWithdraw > currentBalance)
    {
        Console.WriteLine("Withdraw exceeds Current Balance");
        amountToWithdraw = 0;
    }
    Console.WriteLine($"You have withdrawn ${amountToWithdraw}");
    Console.WriteLine($"\nPress any key to continue");
    Console.ReadKey(true);
    return -amountToWithdraw;

}

//Where the program continues if the user logs in
decimal change;
bool done = false;
if (login == true)
{
    do
    {
        //split the array into another array
        transactionDataBaseFile = File.ReadAllLines("transactions.txt");
        for (int i = 0; i < 5; i++)
        {
            string[] tokens = transactionDataBaseFile[userNumber].Split(",");
            transactionDataBaseCurrentUser[i] = tokens[i];
        }
        Console.Clear();
        //Main menu for the ATM system
        Console.WriteLine($"Welcome {bankDataBaseUserName[userNumber]}, please select one of the option below by entering the corresponding number");
        Console.WriteLine($" 1 - Check Balance \n 2 - Withdraw \n 3 - Deposit \n 4 - Display last 5 Transactions \n 5 - Quick Withdraw $40 \n 6 - Quick Withdraw $100 \n 7 - End Current Session");
        int response = Convert.ToInt32(Console.ReadLine());
        switch (response)
        {
            //Check Balance
            case 1:
                CheckBalance(bankDataBaseCurrentBalance[userNumber]);
                break;
            //Withdraw
            case 2:
                change = Withdraw(bankDataBaseCurrentBalance[userNumber]);
                SaveTransaction(userNumber, change, transactionDataBaseFile, transactionDataBaseCurrentUser, lastfiveTransactionsChange);
                bankDataBaseCurrentBalance[userNumber] += change;
                SaveBankDataBase(userNumber, bankDataBaseCurrentBalance[userNumber], bankDataBaseFile);
                break;
            //Deposit
            case 3:
                change = Deposit();
                SaveTransaction(userNumber, change, transactionDataBaseFile, transactionDataBaseCurrentUser, lastfiveTransactionsChange);
                bankDataBaseCurrentBalance[userNumber] += change;
                SaveBankDataBase(userNumber, bankDataBaseCurrentBalance[userNumber], bankDataBaseFile);
                break;
            //Last 5 Transactions
            case 4:
                LastFiveTransactions(transactionDataBaseCurrentUser);
                break;
            //Quick Withdraw $40
            case 5:
                change = QuickWithdraw(40, bankDataBaseCurrentBalance[userNumber]);
                SaveTransaction(userNumber, change, transactionDataBaseFile, transactionDataBaseCurrentUser,lastfiveTransactionsChange);
                bankDataBaseCurrentBalance[userNumber] += change;
                SaveBankDataBase(userNumber, bankDataBaseCurrentBalance[userNumber], bankDataBaseFile);
                break;
            //Quick Withdraw $100
            case 6:
                change = QuickWithdraw(100, bankDataBaseCurrentBalance[userNumber]);
                SaveTransaction(userNumber, change, transactionDataBaseFile, transactionDataBaseCurrentUser, lastfiveTransactionsChange);
                bankDataBaseCurrentBalance[userNumber] += change;
                SaveBankDataBase(userNumber, bankDataBaseCurrentBalance[userNumber], bankDataBaseFile);
                break;
            //End Session
            case 7:
                done = true;
                break;
            //When the user enters an invalid response
            default:
                Console.WriteLine("Invalid response, try again");
                break;
        }
    }
    while (done == false);
}
Console.Clear();
Console.WriteLine("Thank you for using the ATM!");

//Methods used for testing purposes
//Checks for valid password input
bool ValidPassword(string password)
{
    //Checks for numbers only
    if (password.All(char.IsDigit) == true && password != null && Convert.ToInt32(password) >= 0)
        return true;
    else
        return false;
}
//Checks for valid username input
bool  ValidUsername(string userName)
{
    //Checks for letters and numbers only
    if (userName.All(char.IsLetter) == true && userName != null)
        return true;
    else
        return false;
}
//Checks for valid withdraw amount
bool ValidWithdrawAmount(decimal amount, decimal currentBalance)
{
    if (amount < 0 || amount > currentBalance)
        return false;
    else
        return true;
}
//Checks for valid deposit amount
bool ValidDepositAmount(decimal amount)
{
    if (amount < 0)
        return false;
    else
        return true;
}
//Checks for valid menu input
bool ValidMenuInput(int input)
{
    if (input < 1 || input > 7)
        return false;
    else
        return true;
}
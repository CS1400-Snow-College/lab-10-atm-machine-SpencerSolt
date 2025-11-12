Console.Clear();

//Loads the bank database into a list
List<string> bankDataBaseFile = new List<string>();
bankDataBaseFile.AddRange(File.ReadAllLines("bank.txt"));
string[] bankDataBaseUserName = new string[5];
int[] bankDataBasePin = new int[5];
decimal[] bankDataBaseCurrentBalance = new decimal[5];

//Saves the username, pin, and current balance into arrays
for (int i = 0; i < bankDataBaseFile.Count(); i++)
{
    string[] tokens = bankDataBaseFile[i].Split(",");
    bankDataBaseUserName[i] = (tokens[0]);
    bankDataBasePin[i] = Convert.ToInt32(tokens[1]);
    bankDataBaseCurrentBalance[i] = Convert.ToDecimal(tokens[2]);
}

//System to login users in the database
int tries = 0;
string? userName;
int pin;
bool login = false;
int userNumber = -1;
//Users enter a username and a pin at least 3 times to login or end the program
while (tries < 3 && login == false)
{
    Console.Write("Enter your username: ");
    userName = Console.ReadLine();
    Console.Write("Enter your pin: ");
    pin = Convert.ToInt32(Console.ReadLine());
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

//Where the program continues if the user logs in
if (login == true)
{
    Console.Clear();
    Console.WriteLine($"Welcome {bankDataBaseUserName[userNumber]}, please select one of the option below");
    Console.WriteLine($" 1 - Check Balance \n 2 - Withdraw \n 3 - Deposit \n 4 - Display last 5 Transactions \n 5 - Quick Withdraw $40 \n 6 - Quick Withdraw $100 \n 7 - End Current Session");
}
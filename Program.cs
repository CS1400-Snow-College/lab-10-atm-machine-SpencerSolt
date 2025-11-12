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
string? userName = "";
int pin;
bool login = false;
//Users enter a username and a pin at least 3 times to login or end the program
while (tries < 3)
{
    Console.Write("Enter your username: ");
    userName = Console.ReadLine();
    Console.Write("Enter your pin: ");
    pin = Convert.ToInt32(Console.ReadLine());
    for (int x = 0; x < bankDataBaseUserName.Count(); x++)
        if (userName == bankDataBaseUserName[x] || pin == bankDataBasePin[x])
        {
            login = true;
            break;
        }
    tries++;
    Console.Clear();
    Console.WriteLine("Wrong username or pin.");
}

//Where the program continues if the user logs in
if (login == true)
{
    Console.WriteLine($"Welcome, {userName}");
}
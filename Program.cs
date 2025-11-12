//Loads the bank database into a string array

List<string> bankDataBaseFile = new List<string>();
bankDataBaseFile.AddRange(File.ReadAllLines("bank.txt"));
string[] bankDataBaseUserName = new string[5];
int[] bankDataBasePin = new int[5];
decimal[] bankDataBaseCurrentBalance = new decimal[5];

for (int i = 0; i < 5; i++)
{
    string[] tokens = bankDataBaseFile[i].Split(",");
    bankDataBaseUserName[i] = (tokens[0]);
    bankDataBasePin[i] = Convert.ToInt32(tokens[1]);
    bankDataBaseCurrentBalance[i] = Convert.ToDecimal(tokens[2]);
    Console.WriteLine($"{bankDataBaseUserName[i]} | {bankDataBasePin[i]} | {bankDataBaseCurrentBalance[i]}");
}
using ConsoleApp2.Models;
using System.Security.Cryptography.X509Certificates;

Client a = new Client()
{
    
};
Console.WriteLine(a.GetClient(2));
Console.WriteLine(a.GetClient(0).Name);
Console.ReadLine();
    
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class Manager
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surename { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public int Salary { get; set; }

    public string Email { get; set; } = null!;

    public bool NewManager(string login, string password, string name, string surename, string patronymic, string email, int salary)
    {
        using(TrpoContext db =  new TrpoContext())
        {
            int id;
            if (db.Managers.IsNullOrEmpty())
            {
                id = 0;
            }
            else
            {
                List<Manager> people = db.Managers.ToList<Manager>();
                id = people[people.Count() - 1].Id + 1;
                if (db.IsExist(login, password)) return false;
            }
            Manager person = new Manager()
            {
                Id = id,
                Login = login,
                Password = password,
                Name = name,
                Surename = surename,
                Patronymic = patronymic,
                Email = email,
                Salary = salary
            };
            db.Managers.Add(person);
            db.SaveChanges();
            return true;
        }        
    }
}

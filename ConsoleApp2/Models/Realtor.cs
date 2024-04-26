using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class Realtor
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string Surename { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public double Procent { get; set; }

    public string Email { get; set; } = null!;

    public string? ToDo { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public Realtor GetRealtor(int id)
    {
        using(TrpoContext db = new TrpoContext())
        {
            foreach (var real in db.Realtors)
            {
                if (real.Id == id) return real;
            }
            return null;
        }
    }
    public bool NewRealtor(string login, string password, string name, string surename, string patronymic, string email, double salary)
    {
        using(TrpoContext db =  new TrpoContext())
        {
            int id;
            if (db.Realtors.IsNullOrEmpty())
            {
                id = 0;
            }
            else
            {
                List<Realtor> people = db.Realtors.ToList<Realtor>();
                id = people[people.Count() - 1].Id + 1;
                if (db.IsExist(login, password)) return false;
            }
            Realtor person = new Realtor()
            {
                Id = id,
                Login = login,
                Password = password,
                Name = name,
                Surename = surename,
                Patronymic = patronymic,
                Email = email,
                Procent = salary
            };
            db.Realtors.Add(person);
            db.SaveChanges();
            return true;
        }
    }
    public List<Order> GetRealtorOrdersByID(int id)
    {
        using (var db = new TrpoContext())
        {
            List<Order> orders = new List<Order>();
            foreach (var order in db.Orders)
            {
                if (order.RealtorId == id) orders.Add(order);
            }
            return orders;
        }
    }
}

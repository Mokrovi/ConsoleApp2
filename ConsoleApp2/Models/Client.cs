using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using static ConsoleApp2.Models.TrpoContext;

namespace ConsoleApp2.Models;

public partial class Client
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string Surename { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Object> Objects { get; set; } = new List<Object>();

    public virtual ICollection<Order> OrderRenters { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderTentors { get; set; } = new List<Order>();

    public Client GetClient(int id)
    {
        using (var db = new TrpoContext())
        {
            foreach (var client in db.Clients)
            {
                if (client.Id == id) return client;
            }
            return null;
        }
    }
    public bool NewClient(string login, string password, string name, string surename, string patronymic, string email)
    {
        using (var db = new TrpoContext())
        {
            int id;
            if (db.Clients.IsNullOrEmpty())
            {
                id = 0;
            }
            else
            {
                List<Client> cl = db.Clients.ToList<Client>();
                id = cl[cl.Count() - 1].Id + 1;
                if (db.IsExist(login, password)) return false;
            }
            Client person = new Client()
            {
                Id = id,
                Login = login,
                Password = password,
                Name = name,
                Surename = surename,
                Patronymic = patronymic,
                Email = email
            };
            db.Clients.Add(person);
            db.SaveChanges();
            return true;
        }
    }
    public List<Object> GetClientObjects(int id)
    {
        using(var db = new TrpoContext())
        {
            List<Object> objects = new List<Object>();
            foreach (var obj in db.Objects)
            {
                if (obj.ClientId == id) objects.Add(obj);
            }
            return objects;
        }
    }
}

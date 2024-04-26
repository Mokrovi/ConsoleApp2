using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class Object
{
    public int Id { get; set; }

    public string Adress { get; set; } = null!;

    public int Price { get; set; }

    public string TypeOfRoom { get; set; } = null!;

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public Object GetObject(int id)
    {
        using(var db = new TrpoContext())
        {
            foreach (var obj in db.Objects)
            {
                if (obj.Id == id) return obj;
            }
            return null;
        }
    }
    public bool NewObject(string adress, int price, string type, int clientid)
    {
        using(TrpoContext db =  new TrpoContext())
        {
            int id;
            if (db.Objects.IsNullOrEmpty())
            {
                id = 0;
            }
            else
            {
                List<Object> objects = new List<Object>();
                id = objects[objects.Count() - 1].Id + 1;
            }
            Object obj = new Object()
            {
                Id = id,
                Adress = adress,
                Price = price,
                TypeOfRoom = type,
                ClientId = clientid
            };
            db.Objects.Add(obj);
            db.SaveChanges();
            return true;
        }
    }
    public List<Object> FreeObjects()
    {
        using(TrpoContext db = new TrpoContext())
        {
            List<int> idOrd = new List<int>();
            List<Object> objects = new List<Object>();
            foreach (var ord in db.Orders)
            {
                idOrd.Add(ord.Id);
            }
            foreach (var obj in db.Objects)
            {
                if (!idOrd.Contains(obj.Id)) objects.Add(obj);
            }
            return objects;
        }
    } // Объекты не размещённы в заказах
}

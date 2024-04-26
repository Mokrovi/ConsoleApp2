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
}

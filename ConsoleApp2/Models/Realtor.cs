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
}

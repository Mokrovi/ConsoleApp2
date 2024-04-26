using System;
using System.Collections.Generic;

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
}

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
}

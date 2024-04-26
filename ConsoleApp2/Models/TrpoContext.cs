﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static ConsoleApp2.Models.TrpoContext;

namespace ConsoleApp2.Models;

public partial class TrpoContext : DbContext
{
    public TrpoContext(DbContextOptions<TrpoContext> options)
        : base(options)
    {
    }
    public TrpoContext() => Database.EnsureCreated();
    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Realtor> Realtors { get; set; }
    public enum People
    {
        Manager,
        Realtor,
        Client,
        No,
    };
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=trpo;Trusted_Connection=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Login);
            entity.Property(e => e.Password);
            entity.Property(e => e.Email);
            entity.Property(e => e.Name);
            entity.Property(e => e.Patronymic);
            entity.Property(e => e.Surename);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.ToTable("Manager");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Login);
            entity.Property(e => e.Password);
            entity.Property(e => e.Email);
            entity.Property(e => e.Name);
            entity.Property(e => e.Patronymic);
            entity.Property(e => e.Salary);
            entity.Property(e => e.Surename);
        });

        modelBuilder.Entity<Object>(entity =>
        {
            entity.ToTable("Object");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Adress);
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.TypeOfRoom);

            entity.HasOne(d => d.Client).WithMany(p => p.Objects)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Object_Client");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ObjectId).HasColumnName("ObjectID");
            entity.Property(e => e.RealtorId).HasColumnName("RealtorID");
            entity.Property(e => e.RenterId).HasColumnName("RenterID");
            entity.Property(e => e.TentorId).HasColumnName("TentorID");

            entity.HasOne(d => d.Object).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Object");

            entity.HasOne(d => d.Realtor).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RealtorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Realtor");

            entity.HasOne(d => d.Renter).WithMany(p => p.OrderRenters)
                .HasForeignKey(d => d.RenterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Client");

            entity.HasOne(d => d.Tentor).WithMany(p => p.OrderTentors)
                .HasForeignKey(d => d.TentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Client1");
        });

        modelBuilder.Entity<Realtor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Realtor_1");

            entity.ToTable("Realtor");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Login);
            entity.Property(e => e.Password);
            entity.Property(e => e.Email)
            ;
            entity.Property(e => e.Name)
                ;
            entity.Property(e => e.Patronymic)
                ;
            entity.Property(e => e.Surename)
                ;
            entity.Property(e => e.ToDo).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public Order[] GetRealtorOrdersByID(int id)
    {
        List<Order> orders = new List<Order>();
        foreach (var order in Orders)
        {
            if (order.RealtorId == id) orders.Add(order);
        }
        return orders.ToArray();
    }
    public Realtor GetRealtor(int id)
    {
        foreach (var real in Realtors)
        {
            if (real.Id == id) return real;
        }
        return null;
    }
    public Client GetClient(int id)
    {
        foreach (var client in Clients)
        {
            if (client.Id == id) return client;
        }
        return null;
    }
    public Object GetObject(int id)
    {
        foreach(var obj in Objects)
        {
            if(obj.Id == id) return obj;
        }
        return null;
    }
    public Object[] GetClientObjects(int id)
    {
        List<Object> objects = new List<Object>();
        foreach (var obj in Objects)
        {
            if (obj.ClientId == id) objects.Add(obj);
        }
        return objects.ToArray();
    }
    public Object[] FreeObjects()
    {
        List<int> idOrd = new List<int>();
        List<Object> objects = new List<Object>();
        foreach (var ord in Orders)
        {
            idOrd.Add(ord.Id);
        }
        foreach (var obj in Objects)
        {
            if (!idOrd.Contains(obj.Id)) objects.Add(obj);
        }
        return objects.ToArray();
    } // Объекты не размещённы в заказах
    public bool IsExist(string login, string password)
    {
        foreach (Client client in Clients)
        {
            if (client.Login == login && client.Password == password) return true;
        }
        foreach (Manager manager in Managers)
        {
            if (manager.Login == login && manager.Password == password) return true;
        }
        foreach (Realtor realtor in Realtors)
        {
            if (realtor.Login == login && realtor.Password == password) return true;
        }
        return false;
    }
    public bool NewClient(string login, string password, string name, string surename, string patronymic, string email)
    {
        int id;
        if (Clients.IsNullOrEmpty())
        {
            id = 0;
        }
        else
        {
            List<Client> cl = Clients.ToList<Client>();
            id = cl[cl.Count() - 1].Id + 1;
            if (IsExist(login, password)) return false;
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
        Clients.Add(person);
        SaveChanges();
        return true;
    }
    public bool NewManager(string login, string password, string name, string surename, string patronymic, string email, int salary)
    {
        int id;
        if (Managers.IsNullOrEmpty())
        {
            id = 0;
        }
        else
        {
            List<Manager> people = Managers.ToList<Manager>();
            id = people[people.Count() - 1].Id + 1;
            if (IsExist(login, password)) return false;
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
        Managers.Add(person);
        SaveChanges();
        return true;
    }
    public bool NewRealtor(string login, string password, string name, string surename, string patronymic, string email, double salary)
    {
        int id;
        if (Realtors.IsNullOrEmpty())
        {
            id = 0;
        }
        else
        {
            List<Realtor> people = Realtors.ToList<Realtor>();
            id = people[people.Count() - 1].Id + 1;
            if (IsExist(login, password)) return false;
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
        Realtors.Add(person);
        SaveChanges();
        return true;
    }
    public (People, int) FindPerson(string login, string password)                      
    {
        int who = -1;
        People people = People.No;
        foreach(Realtor person in Realtors)
        {
            if(person.Login == login && person.Password == password)
            {
                people = People.Realtor;
                who = person.Id;
            }
        }
        foreach (Manager person in Managers)
        {
            if (person.Login == login && person.Password == password)
            {
                people = People.Manager;
                who = person.Id;
            }
        }
        foreach (Client person in Clients)
        {
            if (person.Login == login && person.Password == password)
            {
                people = People.Client;
                who = person.Id;
            }
        }
        return (people, who);
    }
}

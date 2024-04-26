using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class Order
{
    public int Id { get; set; }

    public int ObjectId { get; set; }

    public int RenterId { get; set; }

    public int TentorId { get; set; }

    public int RealtorId { get; set; }

    public virtual Object Object { get; set; } = null!;

    public virtual Realtor Realtor { get; set; } = null!;

    public virtual Client Renter { get; set; } = null!;

    public virtual Client Tentor { get; set; } = null!;
}

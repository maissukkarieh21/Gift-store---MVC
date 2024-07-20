using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftPayment
{
    public decimal? CardNumber { get; set; }

    public decimal? Cvv { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal? Balance { get; set; }

    public decimal Id { get; set; }
}

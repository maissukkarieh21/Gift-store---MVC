using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftOrder
{
    public string? Status { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal? AdminProfits { get; set; }

    public decimal? MakerProfits { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? UserId { get; set; }

    public decimal? GiftId { get; set; }

    public decimal Id { get; set; }

    public virtual GiftGift? Gift { get; set; }

    public virtual GiftUser? User { get; set; }
}

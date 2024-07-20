using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftGiftsUser
{
    public decimal? Quantity { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public decimal? UserId { get; set; }

    public decimal? GiftId { get; set; }

    public decimal Id { get; set; }

    public virtual GiftGift? Gift { get; set; }

    public virtual GiftUser? User { get; set; }
}

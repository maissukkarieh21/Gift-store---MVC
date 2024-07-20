using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifts_Store_First_project.Models;

public partial class GiftGift
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public decimal? Sale { get; set; }

    public string? ImagePath { get; set; }

    public decimal Id { get; set; }

    public decimal? CategoryId { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual GiftCategory? Category { get; set; }

    public virtual ICollection<GiftGiftsUser> GiftGiftsUsers { get; set; } = new List<GiftGiftsUser>();

    public virtual ICollection<GiftOrder> GiftOrders { get; set; } = new List<GiftOrder>();
}

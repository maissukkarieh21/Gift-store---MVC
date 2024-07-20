using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifts_Store_First_project.Models;

public partial class GiftCategory
{
    public string? Name { get; set; }

    public string? ImagePath { get; set; }

    public decimal Id { get; set; }

    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<GiftGift> GiftGifts { get; set; } = new List<GiftGift>();

    public virtual ICollection<GiftUser> GiftUsers { get; set; } = new List<GiftUser>();
}

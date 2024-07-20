using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifts_Store_First_project.Models;

public partial class GiftHome
{
    public string? Content { get; set; }

    public string? ImagePath { get; set; }

    public decimal Id { get; set; }

    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<GiftAbout> GiftAbouts { get; set; } = new List<GiftAbout>();

    public virtual ICollection<GiftContact> GiftContacts { get; set; } = new List<GiftContact>();
}

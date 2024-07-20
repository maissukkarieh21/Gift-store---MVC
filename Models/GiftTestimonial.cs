using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftTestimonial
{
    public string Message { get; set; } = null!;

    public decimal? UserId { get; set; }

    public decimal Id { get; set; }

    public virtual GiftUser? User { get; set; }
}

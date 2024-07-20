using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifts_Store_First_project.Models;

public partial class GiftAbout
{
    public string? Content { get; set; }

    public string? ImagePath { get; set; }

    public decimal? HomeId { get; set; }

    public decimal Id { get; set; }

    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual GiftHome? Home { get; set; }
}

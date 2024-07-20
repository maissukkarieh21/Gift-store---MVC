using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftContact
{
    public string? UserName { get; set; }

    public string? UserEmail { get; set; }

    public string? Message { get; set; }

    public decimal? HomeId { get; set; }

    public decimal Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Location { get; set; }

    public string? Email { get; set; }

    public virtual GiftHome? Home { get; set; }
}

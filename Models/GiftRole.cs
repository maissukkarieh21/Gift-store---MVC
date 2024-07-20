using System;
using System.Collections.Generic;

namespace Gifts_Store_First_project.Models;

public partial class GiftRole
{
    public string? RoleName { get; set; }

    public decimal Id { get; set; }

    public virtual ICollection<GiftUser> GiftUsers { get; set; } = new List<GiftUser>();
}

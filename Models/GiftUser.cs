using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifts_Store_First_project.Models;

public partial class GiftUser
{
    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public string? ImagePath { get; set; }

    public decimal Id { get; set; }

    public decimal? RoleId { get; set; }

    public decimal? CategoryId { get; set; }

    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }

    //[NotMapped]
    //public SelectList CategoryList { get; set; }
    public virtual GiftCategory? Category { get; set; }

    public virtual ICollection<GiftGiftsUser> GiftGiftsUsers { get; set; } = new List<GiftGiftsUser>();

    public virtual ICollection<GiftOrder> GiftOrders { get; set; } = new List<GiftOrder>();

    public virtual ICollection<GiftTestimonial> GiftTestimonials { get; set; } = new List<GiftTestimonial>();

    public virtual GiftRole? Role { get; set; }
}

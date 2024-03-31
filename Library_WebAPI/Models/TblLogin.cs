using System;
using System.Collections.Generic;

namespace Library_WebAPI.Models;

public partial class TblLogin
{
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public string? Token { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblUser User { get; set; } = null!;
}

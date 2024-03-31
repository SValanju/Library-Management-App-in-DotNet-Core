using System;
using System.Collections.Generic;

namespace Library_WebAPI.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string EmailId { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public int UserRole { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<TblBookTransaction> TblBookTransactions { get; set; } = new List<TblBookTransaction>();

    public virtual ICollection<TblLogin> TblLogins { get; set; } = new List<TblLogin>();

    public virtual TblRole UserRoleNavigation { get; set; } = null!;
}

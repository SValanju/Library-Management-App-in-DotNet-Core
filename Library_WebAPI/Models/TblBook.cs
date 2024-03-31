using System;
using System.Collections.Generic;

namespace Library_WebAPI.Models;

public partial class TblBook
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int BookCount { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<TblBookTransaction> TblBookTransactions { get; set; } = new List<TblBookTransaction>();
}

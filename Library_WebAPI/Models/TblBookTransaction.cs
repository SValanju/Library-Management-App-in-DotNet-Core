using System;
using System.Collections.Generic;

namespace Library_WebAPI.Models;

public partial class TblBookTransaction
{
    public int TransactionId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public virtual TblBook Book { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}

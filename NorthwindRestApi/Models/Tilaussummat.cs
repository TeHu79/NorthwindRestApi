using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models;

public partial class Tilaussummat
{
    public int OrderId { get; set; }

    public double? Summa { get; set; }
}

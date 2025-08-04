using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models;

public partial class TilaussummatDesimaali
{
    public int OrderId { get; set; }

    public double? Summa { get; set; }
}

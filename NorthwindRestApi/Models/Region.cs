using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string RegionDescription { get; set; } = null!;

    public virtual ICollection<Shipper> Shippers { get; set; } = new List<Shipper>();

    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();
}

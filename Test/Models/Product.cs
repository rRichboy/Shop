using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }
}

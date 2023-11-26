using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Context
{
    public static class Helper
    {
        public static readonly TestContext Database = new TestContext();
    }
}

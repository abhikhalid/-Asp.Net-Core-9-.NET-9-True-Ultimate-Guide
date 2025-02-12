using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    //just for understanding purposes we have added this class here, otherwise it would be a service class
    internal class MyMath
    {
        //we want to test this 'Add' method, whether it works correctly.
        public int Add(int a, int b)
        {
            //return 0;
            return a + b;
        }
    }
}

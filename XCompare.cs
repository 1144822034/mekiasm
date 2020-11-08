using Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class XCompare : IComparer<Point2>
    {
        public int Compare(Point2 x, Point2 y)
        {
            return (x.X != y.X) ? x.X.CompareTo(y.X) : x.Y.CompareTo(y.Y);
        }
    }
    
}

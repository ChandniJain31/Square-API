using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class PointModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointModel() { }
        public PointModel(int x, int y) { this.X = x;this.Y = y; }
    }
}

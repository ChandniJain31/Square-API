using System.Drawing;

namespace SquaresAPI.BusinessLogic
{
    public class SquareModel
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public Point D { get; set; }

        public SquareModel() { }
        public SquareModel(Point A, Point B, Point C, Point D)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }    
    }

    public class SquareResponse
    {
        /// <example>Jane</example>
        public HashSet<SquareModel> data { get; set; }
        /// <example>2</example>
        public int count { get; set; }
    }
}

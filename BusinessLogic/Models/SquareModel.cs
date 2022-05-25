using BusinessLogic.Models;

namespace SquaresAPI.BusinessLogic
{
    public class SquareModel
    {
        public PointModel A { get; set; }
        public PointModel B { get; set; }
        public PointModel C { get; set; }
        public PointModel D { get; set; }

        public SquareModel() { }
        public SquareModel(PointModel A, PointModel B, PointModel C, PointModel D)
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

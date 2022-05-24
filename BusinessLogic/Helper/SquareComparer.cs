namespace SquaresAPI.BusinessLogic.Helper
{
    public class SquareComparer:IEqualityComparer<SquareModel>
    {
        public bool Equals(SquareModel x, SquareModel y)
        {
            return ((x.A.Equals(y.A) || x.A.Equals(y.B) || x.A.Equals(y.C) || x.A.Equals(y.D))
                && (x.B.Equals(y.A) || x.B.Equals(y.B) || x.B.Equals(y.C) || x.B.Equals(y.D))
                && (x.C.Equals(y.A) || x.C.Equals(y.B )|| x.C.Equals(y.C )|| x.C.Equals(y.D))
                && (x.D.Equals(y.A )|| x.D.Equals(y.B )|| x.D.Equals(y.C )|| x.D.Equals(y.D)));
        }

        public int GetHashCode(SquareModel obj)
        {
            return (obj.A.GetHashCode() + obj.B.GetHashCode() + obj.C.GetHashCode() + obj.D.GetHashCode());
        }
    }
}

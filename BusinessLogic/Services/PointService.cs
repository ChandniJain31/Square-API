using BusinessLogic.Models;
using SquaresAPI.BusinessLogic;
using SquaresAPI.BusinessLogic.Helper;
using SquaresAPI.DataAcccessLayer.Repository;
using SquaresAPI.DataAcccessLayer.Repository.Models;
using System.Drawing;

namespace SquaresAPI.Services
{
    public class PointService:IPointService
    {
        private IPointRepository repository;
        public PointService(IPointRepository pointRepository)
        {
            this.repository = pointRepository;
        }

        #region Public Methods

        public async Task<bool> AddPointAsync(PointModel point, int UserId)
        {
            Points pt = new Points() { X = point.X, Y = point.Y,UserId=UserId ,TimeStamp=DateTime.Now};
            var entity = await repository.AddAsync(pt);
            return entity != null ? true : false;
        }

        public async Task DeletePointAsync(PointModel point, int UserId)
        {
            Points pt = new Points() { X = point.X, Y = point.Y ,UserId=UserId};
            await repository.DeleteAsync(pt);            
        }

        public async Task ImportPointsAsync(PointModel[] points, int UserId)
        {
            Points[] pointEntity = new Points[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointEntity[i] = new Points() { X = points[i].X, Y = points[i].Y, UserId=UserId, TimeStamp = DateTime.Now };
            }         
           await repository.ImportAsync(pointEntity);
        }  
        
        public async Task<HashSet<SquareModel>> GetSquaresAsync(int UserId)
        {
            var allpoints = await repository.GetAllAsync(UserId);
            if (allpoints == null)
                return null;
            else
            {
                Point[] twodPointArray = new Point[allpoints.Count()];
                HashSet<Point> set = new HashSet<Point>();
                int counter = 0;
                Point ptnew;
                foreach (Points p in allpoints)
                {
                    ptnew = new Point(p.X, p.Y);
                    twodPointArray[counter++] = ptnew;
                    set.Add(ptnew);
                }
                HashSet<SquareModel> squares = SquareCount(twodPointArray,set);
                return squares;
            }
        }
        
        #endregion

        #region Private Methods
        private static HashSet<SquareModel> SquareCount(Point[] input, HashSet<Point> set)
        {
            HashSet<SquareModel> allsqaures = new HashSet<SquareModel>(new SquareComparer());           
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = i+1; j < input.Length; j++)
                {                    
                    ValueTuple<bool, Point,Point> vertices = GetDiagonalPoints(input[i], input[j]);
                    if(vertices.Item1) // is_square_possible == true
                    {
                        if (set.Contains(vertices.Item2) && set.Contains(vertices.Item3))// if other two vertices are in give set of points
                        {
                            SquareModel newsq = new SquareModel()
                            {
                                A = new PointModel(input[i].X, input[i].Y),
                                B = new PointModel(vertices.Item2.X, vertices.Item2.Y),
                                C = new PointModel(input[j].X, input[j].Y),
                                D = new PointModel(vertices.Item3.X, vertices.Item3.Y)
                            };
                            if (!allsqaures.Contains(newsq)) // if that sqaure has not already been identified.
                            {
                                allsqaures.Add(newsq);
                            }
                        }
                    }
                }
            }
            return allsqaures;
        }
      
        private static (bool,Point,Point) GetDiagonalPoints(Point a, Point c)
        {
            bool is_square_possible = false;
            Point b=new Point(), d=new Point();
            // Check if the x-coordinates are equal
            if (a.X == c.X)
            {
                int bX = a.X + c.Y - a.Y;
                int bY = a.Y;
                int dX = c.X + c.Y - a.Y;
                int dY = c.Y;
                b = new Point(bX, bY);
                d = new Point(dX, dY);
                is_square_possible = true;
            }
            // Check if the y-coordinates are equal
            else if (a.Y == c.Y)
            {
                b = new Point(a.X, (a.Y + c.X - a.X));
                d = new Point(c.X, (c.Y + c.X - a.X));
                is_square_possible = true;
            }
            // If the the given coordinates forms a diagonal of the square
            else if (Math.Abs(c.X - a.X) ==
                     Math.Abs(c.Y - a.Y))
            {
                b = new Point(a.X, c.Y);
                d = new Point(c.X, a.Y);
                is_square_possible = true;
            }
            return (is_square_possible, b, d);
        }
        #endregion
    }
}

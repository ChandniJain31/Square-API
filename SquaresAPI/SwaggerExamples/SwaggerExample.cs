using BusinessLogic.Models;
using SquaresAPI.BusinessLogic;
using Swashbuckle.AspNetCore.Filters;
//using Swashbuckle.Examples;
//using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace SquaresAPI
{
    public class PointExample:IExamplesProvider<PointModel>
    {
        public PointModel GetExamples()
        {
            return new PointModel() { X = 1, Y = -1 };
        }
    }

    public class SquareResponseExample : IExamplesProvider<SquareResponse>
    {
        public SquareResponse GetExamples()
        {
            SquareModel sq1 = new SquareModel()
            {
                A = new PointModel(1, 1),
                B = new PointModel(1, -1),
                C = new PointModel(-1, 1),
                D = new PointModel(-1, 1)
            };
            SquareModel sq2 = new SquareModel()
            {
                A = new PointModel(1, 3),
                B = new PointModel(3, 3),
                C = new PointModel(3, 1),
                D = new PointModel(1, 1)
            };
            var data = new HashSet<SquareModel>();
            data.Add(sq1);
            data.Add(sq2);
            return new SquareResponse() { count = 2, data = data };
        }
    }


    public class BadRequestExample : IExamplesProvider<BadRequestError>
    {
        public BadRequestError GetExamples()
        {
            return new BadRequestError("Point already exists." );
        }
    }

    public class NotFoundExample : IExamplesProvider<NotFoundError>
    {
        public NotFoundError GetExamples()
        {
            return new NotFoundError("No squares can be generated." );
        }
    }

    public class UnAuthorizedExample : IExamplesProvider<UnauthorizedError>
    {      
        public UnauthorizedError GetExamples()
        {
            return new UnauthorizedError("Authorization information is missing or invalid.");
        }
    }
}

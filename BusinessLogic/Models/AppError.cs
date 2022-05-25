using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    //public class AppError:IError
    //{
    //    public int statuscode { get; set; }
    //    public string title { get; set; }
    //    public string errormessage { get; set; }

    //    public AppError(int statuscode, string title, string message)
    //    {
    //        this.statuscode = statuscode;
    //        this.errormessage = message;
    //        this.title = title;
    //    }
    //}
    public abstract class IError
    {
       public int statuscode { get; set; }
       public string title { get; set; }
       public string errormessage { get; set; }
    }
    public class BadRequestError : IError
    {
        public BadRequestError(string message)
        {
            statuscode = (int)HttpStatusCode.BadRequest;
            errormessage = message;
            title = "BadRequest";
        }
    }
    public class NotFoundError: IError
    {
        public NotFoundError(string message)
        {
            statuscode = (int)HttpStatusCode.NotFound;
            errormessage = message;
            title = "NotFound";
        }
    }
    public class UnauthorizedError : IError
    {
        public UnauthorizedError(string message)
        {
            statuscode = (int)HttpStatusCode.Unauthorized;
            errormessage = message;
            title = "Unauthorized";
        }
    }

}

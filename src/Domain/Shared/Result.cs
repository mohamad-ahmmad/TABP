using System.Net;

namespace Domain.Shared
{
    public class Result<T>
    {

        private Result(bool isSuccess, IEnumerable<Error> errors, T? response, HttpStatusCode statusCode) 
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Response = response;
            StatusCode = statusCode;
        }
        

        public T? Response { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public IEnumerable<Error> Errors { get; private set; }
        public HttpStatusCode StatusCode { get; set; }


        public static Result<T?> Success(T? response)
        {
            return new Result<T?>(true, new List<Error> { }, response, HttpStatusCode.OK);
        }

        public static Result<T?> Success(HttpStatusCode statusCode)
        {
            return new Result<T?>(true, new List<Error> { }, default(T), statusCode);
        }

        public static implicit operator Result<T>(Error error) => new(false,new List<Error> { error }, default, HttpStatusCode.BadRequest);
        public static implicit operator Result<T>(List<Error> errors) => new(false,errors, default, HttpStatusCode.BadRequest);
        public static implicit operator Result<T>(T response) => new(true,new List<Error> { }, response, HttpStatusCode.OK);

        public static Result<T> Failure(Error error, HttpStatusCode statusCode)
        {
            return new Result<T> (false, new List<Error> { error }, default, statusCode);
        }
        public static Result<T> Failure(Error error)
        {
            return new Result<T>(false, new List<Error> { error }, default, HttpStatusCode.BadRequest);
        }

        public static Result<T> Failures(IEnumerable<Error> errors)
        {
            return new Result<T>(false, errors, default, HttpStatusCode.BadRequest);
        }
        public static Result<T> Failures(IEnumerable<Error> errors, HttpStatusCode statusCode)
        {
            return new Result<T>(false, errors, default, statusCode);
        }
    }
}

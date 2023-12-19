namespace Domain.Shared
{
    public class Result<T>
    {

        private Result(bool isSuccess, IEnumerable<Error> errors, T? response) 
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Response = response;
        }

        public T? Response { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public IEnumerable<Error> Errors { get; private set; }



        public static Result<T?> Success(T? response)
        {
            return new Result<T?>(true, new List<Error> { }, response);
        }

        public static implicit operator Result<T>(Error error) => new(false,new List<Error> { error }, default);
        public static implicit operator Result<T>(List<Error> errors) => new(false,errors, default);
        public static implicit operator Result<T>(T response) => new(true,new List<Error> { }, response);

        public static Result<T> Failure(Error error)
        {
            return new Result<T> (false, new List<Error> { error }, default);
        }
        public static Result<T> Failures(IEnumerable<Error> errors)
        {
            return new Result<T>(false, errors, default);
        }
    }
}

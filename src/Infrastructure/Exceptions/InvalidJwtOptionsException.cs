namespace Infrastructure.Exceptions
{
    public class InvalidJwtOptionsException : InfrastructureException
    {
        public InvalidJwtOptionsException(string msg) : base(msg)
        {
        }
    }
}

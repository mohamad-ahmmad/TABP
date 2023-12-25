using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string msg) : base(msg)
        {
        }
    }
}

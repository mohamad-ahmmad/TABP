namespace Domain.Shared
{
    public record Error(string Subject, string Description) 
    {
        public static readonly Error None = new(string.Empty,string.Empty);
    }
}

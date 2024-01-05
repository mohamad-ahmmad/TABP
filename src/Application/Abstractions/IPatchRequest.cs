namespace Application.Abstractions;
public interface IPatchRequest<T>
{
    T ApplyTo(T entity);
}


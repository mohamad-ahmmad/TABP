using Application.Abstractions;
using Microsoft.AspNetCore.JsonPatch;

namespace Infrastructure.Services.Patch;
public class JsonPatchRequest<T> : IPatchRequest<T>
    where T : class
{
    private JsonPatchDocument<T> _patchDoc;

    public JsonPatchRequest(JsonPatchDocument<T> patchDoc)
    {
        _patchDoc = patchDoc;
    }
    public T ApplyTo(T entity)
    {
        _patchDoc.ApplyTo(entity);
        return entity;
    }
}


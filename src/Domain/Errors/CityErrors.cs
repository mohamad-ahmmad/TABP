using Domain.Shared;

namespace Domain.Errors;

public static class CityErrors
{
    public static Error UnauthorizedToCreateCity = new Error("Create City", "Unauthorized");
}


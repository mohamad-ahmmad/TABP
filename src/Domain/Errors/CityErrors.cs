using Domain.Shared;

namespace Domain.Errors;

public static class CityErrors
{
    public static Error UnauthorizedToCreateCity = new Error("Create City", "Unauthorized");
    public static Error CityAlreadyExist = new Error("Create City", "The city already exists");
}


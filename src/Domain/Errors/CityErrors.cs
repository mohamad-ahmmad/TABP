using Domain.Shared;

namespace Domain.Errors;

public static class CityErrors
{
    public static Error UnauthorizedToCreateCity = new Error("Create City", "Unauthorized.");
    public static Error CityAlreadyExist = new Error("Create City", "The city already exists.");
    public static Error CityNotFound = new Error("Get a city", "Can not find the city.");
    public static Error CreateCityNotFoundError(string subject ,Guid cityId)
    {
        return new Error(subject, $"The city with '{cityId}' ID not found.");
    }
    public static Error UnauthorizedToDeleteCity = new Error("Delete City", "Unauthorized.");
}


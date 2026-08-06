namespace HealthDeskAPI.Responses;

public record UserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt
);

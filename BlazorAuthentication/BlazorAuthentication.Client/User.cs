namespace BlazorAuthentication.Client;

public class User
{
    public string Email { get; set; }
    public bool IsAuthenticated { get; set; }
    public IEnumerable<string> Roles { get; set; } // Optional: if you need roles
    // Add other properties as needed
}
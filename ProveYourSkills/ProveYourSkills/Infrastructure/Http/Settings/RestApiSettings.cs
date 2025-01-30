namespace ProveYourSkills.Infrastructure.Http.Settings;

public class RestApiSettings
{
    public required Uri BaseUri { get; set; }
    public required string ContentType { get; set; }
}

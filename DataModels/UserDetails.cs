namespace DataModels;

public class UserDetails
{
    public Guid Id { get; set; }
    public string AuthenticationUid { get; set; }
    public string? Salutation { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public string? Headline { get; set; }
    public string? Biography { get; set; }
    public string? Language { get; set; }
    public string? Website { get; set; }
    public string? FacebookUserName { get; set; }
    public string? InstagramUserName { get; set; }
    public string? TwitterUserName { get; set; }
    public string? TiktokUserName { get; set; }
    public string? LinkedInPublicProfileUrl { get; set; }
    public string? YoutubeUserName { get; set; }
    public string? EmailId { get; set; }
    public string? StreetAddress { get; set; }
    public string? HouseNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
}
namespace DataModels;

public class UserDetails
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string AuthenticationUid { get; set; }
    public string EmailId { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}
namespace Application.Interfaces
{
    // so that application can access http context i.e. username or email
    // from the api
    public interface IUserAccessor
    {
        string GetEmail();
    }
}
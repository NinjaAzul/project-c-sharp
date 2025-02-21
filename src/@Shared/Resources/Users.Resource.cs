using Project_C_Sharp.Shared.Resources.Base;

namespace Project_C_Sharp.Shared.Resources.Users;



public static class UsersResource
{
    private static class Messages
    {
        private static readonly BaseResource Resource = new MessagesResource();
        public static string Get(string key) => Resource.GetString(key);
    }

    private static class Errors
    {
        private static readonly BaseResource Resource = new ErrorsResource();
        public static string Get(string key) => Resource.GetString(key);
    }

    private class MessagesResource : BaseResource
    {
        public MessagesResource() : base("Project_C_Sharp.Shared.I18n.Modules.Users.Messages") { }
    }

    private class ErrorsResource : BaseResource
    {
        public ErrorsResource() : base("Project_C_Sharp.Shared.I18n.Modules.Users.Errors") { }
    }

    // Métodos de conveniência
    public static string GetMessage(string key) => Messages.Get(key);
    public static string GetError(string key) => Errors.Get(key);
}
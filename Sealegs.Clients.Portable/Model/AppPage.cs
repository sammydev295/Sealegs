namespace Sealegs.Clients.Portable
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }
        public string Id { get; set;}
    }
    public enum AppPage
    {
        Lockers,
        AddLocker,
        Employees,
        AddEmployee,
        Feed,
        Events,
        MiniHacks,
        News,
        Venue,
        FloorMap,
        ConferenceInfo,
        Settings,
        Speaker,
        //News,
        Login,
        Event,
        Notification,
        TweetImage,
        WiFi,
        CodeOfConduct,
        Filter,
        Information,
        Tweet,
        Evals,
        NotificationManagement

    }
}



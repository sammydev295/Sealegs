namespace Sealegs.DataObjects
{
    /// <summary>
    /// Per user feedback
    /// </summary>
    public class Feedback : BaseDataObject
    {
        public string UserId { get; set; }
        public string FeedbackEntityId { get; set; }
        public int Rating { get; set; }
        public string WineId { get; set; }

    }

    public class FeedbackExteneded : BaseDataObject
    {
        public Feedback Feedback { get; set; }
        public Wine Wine { get; set; }
        public LockerMember Locker { get; set; }
    }
}
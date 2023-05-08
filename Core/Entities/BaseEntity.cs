namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int DataStatus { get; set; } = 1;
        public DateTimeOffset SetOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}

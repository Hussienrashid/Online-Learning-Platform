namespace HW.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }          
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public DateTime EnrolledAt { get; set; }
        public string Status { get; set; } = string.Empty;

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}

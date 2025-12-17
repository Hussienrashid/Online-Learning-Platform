namespace HW.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Thumbnail { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
       // public object Quizzes { get; internal set; }
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    }
}

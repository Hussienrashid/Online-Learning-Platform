namespace HW.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public int Order { get; set; }
        public int EstimatedDuration { get; set; }
        public DateTime CreatedAt { get; set; }

        public Course Course { get; set; }
        public ICollection<LessonCompletion> LessonCompletions { get; set; }
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    }
}

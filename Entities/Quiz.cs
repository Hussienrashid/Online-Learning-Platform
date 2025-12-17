namespace HW.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public string Title { get; set; }
        public int PassingScore { get; set; }
        public int TimeLimit { get; set; }

        public Course Course { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}

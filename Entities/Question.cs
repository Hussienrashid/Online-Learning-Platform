namespace HW.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }

        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}

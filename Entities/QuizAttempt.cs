namespace HW.Entities
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }

        public Quiz Quiz { get; set; }
        public User User { get; set; }
    }
}

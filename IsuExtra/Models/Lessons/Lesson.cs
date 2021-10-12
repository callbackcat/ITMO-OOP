using Isu.Tools;

namespace IsuExtra.Models.Lessons
{
    public class Lesson
    {
        public Lesson(string lector, string auditory, LessonTime time)
        {
            if (string.IsNullOrWhiteSpace(lector))
                throw new IsuException("Invalid lector's name");

            if (string.IsNullOrWhiteSpace(auditory))
                throw new IsuException("Invalid lector's name");

            _ = time ?? throw new IsuException("Invalid lesson's time reference");

            Time = time;
            Lector = lector;
            Auditory = auditory;
        }

        internal LessonTime Time { get; }
        internal string Lector { get; }
        internal string Auditory { get; }
        internal bool IntersectsWith(Lesson lesson) =>
            lesson.Auditory == this.Auditory &&
            !(Time.Start > lesson.Time.End || Time.End < lesson.Time.Start);
    }
}
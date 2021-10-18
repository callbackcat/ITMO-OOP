using System;

namespace IsuExtra.Models.Lessons
{
    public class LessonTime
    {
        public LessonTime(DateTime time)
        {
            Start = time;
            End = Start.AddMinutes(90);
        }

        internal DateTime Start { get; }
        internal DateTime End { get; }
    }
}
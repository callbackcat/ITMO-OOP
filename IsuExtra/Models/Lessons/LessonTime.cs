using System;
using Isu.Tools;

namespace IsuExtra.Models.Lessons
{
    public class LessonTime
    {
        public LessonTime(string begin)
        {
            try
            {
                Start = DateTime.ParseExact(begin, "dd/MM/yyyy HH:mm", null);
            }
            catch (Exception e)
            {
                throw new IsuException("Invalid date format", e);
            }

            End = Start.AddMinutes(90);
        }

        internal DateTime Start { get; }
        internal DateTime End { get; }
    }
}
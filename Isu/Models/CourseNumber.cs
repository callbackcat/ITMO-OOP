using Isu.Tools;

namespace Isu.Models
{
    public class CourseNumber
    {
        private const uint MaxCourseValue = 4;
        private const uint MinCourseValue = 1;

        public CourseNumber(uint value)
        {
            if (value is > MaxCourseValue or < MinCourseValue)
                throw new IsuException("Invalid course number");

            Value = value;
        }

        public uint Value { get; }
    }
}
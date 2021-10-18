using System;
using Isu.Tools;

namespace IsuExtra.Models.IsuEntities
{
    public class CourseNumber : IEquatable<CourseNumber>
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

        public bool Equals(CourseNumber other)
        {
            if (other is null)
            {
                return false;
            }

            return Value == other.Value;
        }

        public override bool Equals(object obj)
            => obj is CourseNumber && Equals(obj);

        public override int GetHashCode()
            => Value.GetHashCode();
    }
}
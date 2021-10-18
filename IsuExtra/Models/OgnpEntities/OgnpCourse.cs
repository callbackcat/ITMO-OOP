using Isu.Tools;

namespace IsuExtra.Models.OgnpEntities
{
    public class OgnpCourse
    {
        public OgnpCourse(string name, char courseIdentifier)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Invalid OGNP course name");

            if (courseIdentifier is default(char) or ' ')
                throw new IsuException("Invalid course identifier");

            Name = name;
            CourseIdentifier = courseIdentifier;
        }

        internal string Name { get; }
        internal char CourseIdentifier { get; }
    }
}
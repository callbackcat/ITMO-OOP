using Isu.Tools;

namespace Isu.Models
{
    public class Group
    {
        public Group(string groupName)
        {
            if (groupName.Length < 5 || groupName[0] != 'M')
            {
                throw new IsuException("Invalid course name");
            }

            if (!uint.TryParse($"{groupName[2]}", out uint courseNumber))
            {
                throw new IsuException("Invalid course number");
            }

            if (!uint.TryParse(groupName.Substring(3, 2), out uint groupNumber) || groupNumber > 12)
            {
                throw new IsuException("Invalid group number");
            }

            GroupName = groupName;
            GroupNumber = groupNumber;
            CourseNumber = new CourseNumber(courseNumber);
        }

        public string GroupName { get; }
        public uint GroupNumber { get; }
        public CourseNumber CourseNumber { get; }
    }
}
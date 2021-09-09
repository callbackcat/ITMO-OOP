using Isu.Tools;

namespace Isu.Models
{
    public class Student
    {
        private static uint _id = 1; // Static for assigning unique id's
        public Student(Group group, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Invalid student name");

            Id = _id++;
            Group = group;
            Name = name;
        }

        public uint Id { get; }
        public string Name { get; }
        public Group Group { get; set; }
    }
}
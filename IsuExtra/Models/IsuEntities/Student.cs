using Isu.Tools;

namespace IsuExtra.Models.IsuEntities
{
    public class Student
    {
        private static uint _id = 1; // Static for assigning unique id's
        public Student(Group group, string name)
        {
            _ = group ?? throw new IsuException("Invalid group reference");

            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Invalid student's name");

            Id = _id++;
            Group = group;
            Name = name;
        }

        public Group Group { get; }
        internal uint Id { get; }
        internal string Name { get; }
    }
}
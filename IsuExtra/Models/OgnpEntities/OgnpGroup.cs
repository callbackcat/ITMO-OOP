using System.Collections.Generic;
using Isu.Tools;
using IsuExtra.Models.Lessons;

namespace IsuExtra.Models.OgnpEntities
{
    public class OgnpGroup
    {
        private readonly List<Lesson> _lessons;
        public OgnpGroup(string name, uint capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Invalid OGNP group name");

            if (capacity == 0)
                throw new IsuException("Study stream's capacity must be greater than zero");

            Name = name;
            Capacity = capacity;
            _lessons = new List<Lesson>();
        }

        internal string Name { get; }
        internal uint Capacity { get; }
        internal IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        public void AddNewOgnpLesson(Lesson lesson)
        {
            _ = lesson ?? throw new IsuException("Invalid OGNP lesson reference");
            _lessons.Add(lesson);
        }
    }
}
using Isu.Tools;

namespace IsuExtra.Models.OgnpEntities
{
    public class StudyStream
    {
        public StudyStream(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new IsuException("Invalid stream name");

            Name = name;
        }

        internal string Name { get; }
    }
}
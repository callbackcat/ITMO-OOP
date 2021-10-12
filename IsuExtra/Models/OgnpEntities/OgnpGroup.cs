using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Isu.Tools;
using IsuExtra.Attributes;
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

        // Stack Trace to prohibit group's change by invalid methods
        public void AddNewOgnpLesson(Lesson lesson)
        {
            StackFrame stackFrame = new StackTrace(true).GetFrames()[1];
            Attribute attr = (stackFrame.GetMethod() ?? throw new IsuException("Prohibited invoke"))
                .GetCustomAttribute(typeof(AllowedToAddLessonAttribute));
            _ = attr ?? throw new IsuException("Prohibited changing of group's lessons");
            _lessons.Add(lesson);
        }
    }
}
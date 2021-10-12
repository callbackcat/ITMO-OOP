using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Isu.Tools;
using IsuExtra.Attributes;
using IsuExtra.Models.Lessons;

namespace IsuExtra.Models.IsuEntities
{
    public class Group
    {
        private readonly List<Lesson> _lessons;
        public Group(string groupName)
        {
            if (groupName.Length < 5 || !char.IsLetter(groupName[0]))
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
            _lessons = new List<Lesson>();
            CourseNumber = new CourseNumber(courseNumber);
        }

        internal string GroupName { get; }
        internal uint GroupNumber { get; }
        internal CourseNumber CourseNumber { get; }
        internal char GetCourseIdentifier => GroupName[2];
        internal IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        // Stack Trace to prohibit group's change by invalid methods
        public void AddNewLesson(Lesson lesson)
        {
            StackFrame stackFrame = new StackTrace(true).GetFrames()[1];
            Attribute attr = (stackFrame.GetMethod() ?? throw new IsuException("Prohibited invoke"))
                .GetCustomAttribute(typeof(AllowedToAddLessonAttribute));
            _ = attr ?? throw new IsuException("Prohibited changing of group's lessons");
            _lessons.Add(lesson);
        }
    }
}
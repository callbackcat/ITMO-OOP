using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Isu.Tools;
using IsuExtra.Models.Lessons;

namespace IsuExtra.Models.IsuEntities
{
    public class Group
    {
        private const int MinGroupNameLength = 5;
        private const int MaxGroupNumber = 15;
        private readonly List<Lesson> _lessons;

        public Group(string groupName)
        {
            if (groupName.Length < MinGroupNameLength || !char.IsLetter(groupName[0]))
            {
                throw new IsuException("Invalid course name");
            }

            if (!uint.TryParse($"{groupName[2]}", out uint courseNumber))
            {
                throw new IsuException("Invalid course number");
            }

            if (!uint.TryParse(groupName.Substring(3, 2), out uint groupNumber)
                || groupNumber > MaxGroupNumber)
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
        internal char CourseIdentifier => GroupName[2];
        internal IReadOnlyList<Lesson> Lessons => _lessons;
        public void AddNewLesson(Lesson lesson)
        {
            _ = lesson ?? throw new IsuException("Invalid lesson reference");
            _lessons.Add(lesson);
        }
    }
}
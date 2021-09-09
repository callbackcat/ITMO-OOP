using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Student> _students;
        private readonly List<Group> _groups;
        private readonly Dictionary<Group, List<Student>> _assignments;

        public IsuService()
        {
            _students = new List<Student>();
            _groups = new List<Group>();
            _assignments = new Dictionary<Group, List<Student>>();
        }

        public Group AddGroup(string name)
        {
            var group = new Group(name);
            if (_groups.Any(g => g.GroupNumber == group.GroupNumber
                                  && g.CourseNumber.Value == group.CourseNumber.Value))
            {
                throw new IsuException("The group already exists");
            }

            _groups.Add(group);
            _assignments.Add(group, new List<Student>());

            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (group is null || !_groups.Contains(group))
            {
                throw new IsuException("The group wasn't found");
            }

            if (!_assignments.ContainsKey(group))
            {
                throw new IsuException($"Assignments doesn't contain group: {group}");
            }

            if (_assignments[group].Count > 30)
            {
                throw new IsuException("The group contains maximum number of students");
            }

            var student = new Student(group, name);
            _students.Add(student);
            _assignments[group].Add(student);

            return student;
        }

        public Student GetStudent(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id)
                   ?? throw new IsuException($"The student with id: {id} wasn't found");
        }

        public Student FindStudent(string name)
        {
            return _students.Find(s => s.Name == name);
        }

        public List<Student> FindStudents(string groupName)
        {
            Group group = FindGroup(groupName);
            return _assignments.FirstOrDefault(g => g.Key == group).Value;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _students
                .Where(s => s.Group.CourseNumber == courseNumber)
                .ToList();
        }

        public Group FindGroup(string groupName)
        {
            return _groups.Find(g => g.GroupName == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups
                .Where(g => g.CourseNumber.Value == courseNumber.Value)
                .ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student is null || !_students.Contains(student))
            {
                throw new IsuException("The student wasn't found");
            }

            if (newGroup is null || !_groups.Contains(newGroup))
            {
                throw new IsuException("The new group does not exist");
            }

            _assignments[student.Group].Remove(student);
            student.Group = newGroup;
            _assignments[newGroup].Add(student);
        }
    }
}
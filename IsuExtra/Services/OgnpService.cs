using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Tools;
using IsuExtra.Models.IsuEntities;
using IsuExtra.Models.OgnpEntities;

namespace IsuExtra.Services
{
    public class OgnpService
    {
        private readonly IsuService _isuService;
        private readonly Dictionary<OgnpCourse, List<StudyStream>> _courses;
        private readonly Dictionary<StudyStream, List<OgnpGroup>> _streams;
        private readonly Dictionary<OgnpGroup, List<Student>> _groups;

        public OgnpService()
        {
            _isuService = new IsuService();
            _courses = new Dictionary<OgnpCourse, List<StudyStream>>();
            _streams = new Dictionary<StudyStream, List<OgnpGroup>>();
            _groups = new Dictionary<OgnpGroup, List<Student>>();
        }

        public OgnpCourse AddOgnpCourse(string name, char courseIdentifier)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IsuException("Invalid course name");
            }

            if (courseIdentifier is default(char) or ' ')
            {
                throw new IsuException("Course identifier name");
            }

            if (_courses.Any(c => string
                .Equals(c.Key.Name, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new IsuException($"The course with name: {name} is already exists");
            }

            var course = new OgnpCourse(name, courseIdentifier);
            _courses.Add(course, new List<StudyStream>());
            return course;
        }

        public StudyStream AddStudyStream(OgnpCourse course, string streamName)
        {
            _ = course ?? throw new IsuException("Invalid course reference");
            if (!_courses.ContainsKey(course))
            {
                throw new IsuException("The course with name: " +
                                       $"{course.Name} doesn't exist");
            }

            if (_courses[course].Any(s => string
                .Equals(s.Name, streamName, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new IsuException($"The stream with name: {streamName} is already exists");
            }

            var stream = new StudyStream(streamName);
            _courses[course].Add(stream);
            _streams.Add(stream, new List<OgnpGroup>());
            return stream;
        }

        public OgnpGroup AddOgnpGroup(StudyStream stream, string groupName, uint capacity)
        {
            _ = stream ?? throw new IsuException("Invalid stream reference");
            if (!_streams.ContainsKey(stream))
            {
                throw new IsuException("The stream with name: " +
                                       $"{stream.Name} doesn't exist");
            }

            if (_streams[stream].Any(g => string
                .Equals(g.Name, groupName, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new IsuException($"The group with name: {groupName} is already exists");
            }

            var group = new OgnpGroup(groupName, capacity);
            _streams[stream].Add(group);
            _groups.Add(group, new List<Student>());
            return group;
        }

        public Student AddStudentToGroup(Student student, OgnpGroup group)
        {
            _ = student ?? throw new IsuException("Invalid student reference");
            _ = group ?? throw new IsuException("Invalid group reference");

            if (!_groups.ContainsKey(group))
            {
                throw new IsuException("The group with name: " +
                                       $"{group.Name} doesn't exist");
            }

            if (student.Group.Lessons != null && group.Lessons != null)
            {
                if (student.Group.Lessons
                    .Any(lesson => group.Lessons.Any(l => l.IntersectsWith(lesson))))
                {
                    throw new IsuException("The group's schedule intersects with student's schedule");
                }
            }

            if (_groups[group].Count == group.Capacity)
            {
                throw new IsuException("The group has reached the maximum number of students");
            }

            if (_groups[group].Contains(student))
            {
                throw new IsuException("The student was already added to the group");
            }

            if (_groups.Count(g => g.Value.Contains(student)) >= 2)
            {
                throw new IsuException("The student has the maximum number of courses");
            }

            var stream = _streams
                .FirstOrDefault(s => s.Value.Contains(group));

            if (stream.Equals(default(KeyValuePair<StudyStream, List<OgnpGroup>>)))
            {
                throw new IsuException("The group wasn't found");
            }

            OgnpCourse course = _courses
                .FirstOrDefault(c => c.Value.Contains(stream.Key))
                .Key;

            if (student.Group.CourseIdentifier == course.CourseIdentifier)
            {
                throw new IsuException("Student can't enroll to" +
                                       "course from the same megafacultaty");
            }

            _groups[group].Add(student);
            return student;
        }

        public Student RemoveStudentFromCourse(Student student, OgnpCourse course)
        {
            _ = student ?? throw new IsuException("Invalid student reference");
            _ = course ?? throw new IsuException("Invalid course reference");

            var group = _groups
                    .FirstOrDefault(g => g.Value.Contains(student));

            if (group.Equals(default(KeyValuePair<OgnpGroup, List<Student>>)))
            {
                throw new IsuException("The student wasn't found");
            }

            _groups[group.Key].Remove(student);
            return student;
        }

        public IReadOnlyList<StudyStream> GetStreamsList(OgnpCourse course)
        {
            _ = course ?? throw new IsuException("Invalid course reference");
            if (!_courses.ContainsKey(course))
            {
                throw new IsuException("The course with name: " +
                                       $"{course.Name} doesn't exist");
            }

            return _courses[course];
        }

        public IReadOnlyList<Student> GetStudentsList(OgnpGroup group)
        {
            _ = group ?? throw new IsuException("Invalid group reference");
            if (!_groups.ContainsKey(group))
            {
                throw new IsuException("Group with name" +
                                       $"{group.Name} wasn't found");
            }

            return _groups[group];
        }

        public IReadOnlyList<Student> GetUnenrolledStudents()
        {
            IReadOnlyList<Student> allStudents = _isuService.GetAllStudents();
            IReadOnlyList<Student> ognpStudents = _groups
                .SelectMany(g => g.Value)
                .ToList();

            return allStudents.Except(ognpStudents).ToImmutableList();
        }

        // Access private fields for IsuExtra.Tests
        public IReadOnlyDictionary<OgnpCourse, List<StudyStream>> GetCourses() => _courses;
        public IReadOnlyDictionary<StudyStream, List<OgnpGroup>> GetStreams() => _streams;
        public IReadOnlyDictionary<OgnpGroup, List<Student>> GetGroups() => _groups;
    }
}
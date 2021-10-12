using System.Linq;
using Isu.Tools;
using IsuExtra.Attributes;
using IsuExtra.Models.IsuEntities;
using IsuExtra.Models.Lessons;
using IsuExtra.Models.OgnpEntities;
using IsuExtra.Services;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private OgnpService _ognpService;
        private IsuService _isuService;
        
        [SetUp]
        public void Setup()
        {
            _ognpService = new OgnpService();
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToOgnpGroup_OgnpGroupContainsStudent()
        {
            Group group = _isuService.AddGroup("M3206");
            Student student = _isuService.AddStudent(group, "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", 20);
            
            _ognpService.AddStudentToGroup(student, ognpGroup);
            Assert.That(_ognpService.GetGroups()[ognpGroup].Contains(student));
        }

        [Test]
        public void ReachMaxStudentPerOgnpGroup_ThrowException()
        {
            Group group = _isuService.AddGroup("M3206");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", 20);
            
            Assert.Catch<IsuException>(() =>
            {
                for (int i = 0; i < 21; i++)
                {
                    _ognpService.AddStudentToGroup(new Student(group, $"Student №{i}"), ognpGroup);
                }
            });
        }

        [Test]
        public void RemoveStudentFromCourse_TheStudentWasRemoved()
        {
            Group group = _isuService.AddGroup("M3206");
            Student student = _isuService.AddStudent(group, "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", 20);

            _ognpService.AddStudentToGroup(student, ognpGroup);
            _ognpService.RemoveStudentFromCourse(student, course);
            Assert.That(!_ognpService.GetStudentsList(ognpGroup).Contains(student));
        }

        [Test]
        [AllowedToAddLesson]
        public void AddStudentWithScheduleIntersection_ThrowException()
        {
            Group group = _isuService.AddGroup("M3206");
            Student student = _isuService.AddStudent(group, "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", 20);
            
            ognpGroup.AddNewOgnpLesson(new Lesson("Anton", "228",
                new LessonTime("12/10/2021 13:30"))); 
            
            student.Group.AddNewLesson(new Lesson("Anton", "228",
                new LessonTime("12/10/2021 13:20")));

            Assert.Catch<IsuException>(() =>
            {
                _ognpService.AddStudentToGroup(student, ognpGroup);
            });
        }
    }
}
using System;
using Isu.Tools;
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
            _isuService.AddGroup("M3206");
        }

        [Test]
        public void AddStudentToOgnpGroup_OgnpGroupContainsStudent()
        {
            const int groupCapacity = 20;
            
            Student student = _isuService.AddStudent(_isuService.FindGroup("M3206"), "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", groupCapacity);
            
            _ognpService.AddStudentToGroup(student, ognpGroup);
            CollectionAssert.Contains(_ognpService.GetGroups()[ognpGroup], student);
        }

        [Test]
        public void ReachMaxStudentPerOgnpGroup_ThrowException()
        {
            const int groupCapacity = 20;
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", groupCapacity);
            
            for (int i = 0; i < groupCapacity; i++)
            {
                _ognpService.AddStudentToGroup(new Student(_isuService.FindGroup("M3206"),
                    $"Student №{i}"), ognpGroup);
            }
            
            Assert.Catch<IsuException>(() =>
            {
                _ognpService.AddStudentToGroup(new Student(_isuService.FindGroup("M3206"),
                    "Extra Student"), ognpGroup);
            });
        }

        [Test]
        public void RemoveStudentFromCourse_TheStudentWasRemoved()
        {
            const int groupCapacity = 20;
            
            Student student = _isuService.AddStudent(_isuService.FindGroup("M3206"),
                "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", groupCapacity);

            _ognpService.AddStudentToGroup(student, ognpGroup);
            _ognpService.RemoveStudentFromCourse(student, course);
            CollectionAssert.DoesNotContain(_ognpService.GetStudentsList(ognpGroup), student);
        }

        [Test]
        public void AddStudentWithScheduleIntersection_ThrowException()
        {
            const int groupCapacity = 20;
            
            Student student = _isuService.AddStudent(_isuService.FindGroup("M3206"), "Mr.Test");
            
            OgnpCourse course = _ognpService.AddOgnpCourse("Cyber security", 'K');
            StudyStream stream = _ognpService.AddStudyStream(course, "CYB-1");
            OgnpGroup ognpGroup = _ognpService.AddOgnpGroup(stream, "Group 1", groupCapacity);

            var time = Convert.ToDateTime("12/10/2021 13:30");
            
            ognpGroup.AddNewOgnpLesson(new Lesson("Anton", "228",
                new LessonTime(time))); 
            
            student.Group.AddNewLesson(new Lesson("Anton", "228",
                new LessonTime(time)));

            Assert.Catch<IsuException>(() =>
            {
                _ognpService.AddStudentToGroup(student, ognpGroup);
            });
        }
    }
}
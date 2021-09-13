using Isu.Models;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group testGroup = _isuService.AddGroup("M3206");
            Student testStudent = _isuService.AddStudent(testGroup, "Mr.Test");

            Assert.That(testStudent, !Is.Null);
            Assert.That(testStudent.Group, Is.SameAs(testGroup));
            Assert.That(_isuService.FindStudents(testGroup.GroupName).Contains(testStudent), Is.True);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Group testGroup = _isuService.AddGroup("M3206");
            Student testStudent = _isuService.AddStudent(testGroup, "Mr.Test");
            
            Assert.Catch<IsuException>(() =>
            {
                for (int i = 0; i < 31; i++)
                {
                    _isuService.AddStudent(testGroup, testStudent.Name);
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("G9015");
                _isuService.AddGroup("");
                _isuService.AddGroup(null);
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group currentGroup = _isuService.AddGroup("M3206");
            Group newGroup = _isuService.AddGroup("M3207");
            
            Student testStudent = _isuService.AddStudent(currentGroup, "Mr.Test");
            
            _isuService.ChangeStudentGroup(testStudent, newGroup);
            Assert.That(testStudent.Group, Is.SameAs(newGroup));
            
            Assert.Catch<IsuException>(() =>
            {
                _isuService.ChangeStudentGroup(testStudent, new Group("G3201"));
            });
        }
    }
}
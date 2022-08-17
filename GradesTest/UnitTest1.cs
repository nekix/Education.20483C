using GradesPrototype.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GradesTest
{
    [TestClass]
    public class UnitTest1
    {
        Grade grade;

        [TestInitialize]
        public void Init()
        {
            DataSource.CreateData();
            grade = new Grade(1, "17/08/2022", "Math", "A+", "Beatifull");
        }

        [TestMethod]
        public void TestValidGrade()
        {
            Assert.AreEqual(grade.AssessmentDate, "17.08.2022");
            Assert.AreEqual(grade.SubjectName, "Math");
            Assert.AreEqual(grade.Assessment, "A+");
        }

        [TestMethod]
        public void TestBadDate()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => grade.AssessmentDate = "20/08/2032");
        }

        [TestMethod]
        public void TestDateNotRecognized()
        {
            Assert.ThrowsException<ArgumentException>(() => grade.AssessmentDate = "200/02/2015");
        }

        [TestMethod]
        public void TestBadAssessment()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => grade.Assessment = "G+");
        }

        [TestMethod]
        public void TestBadSubject()
        {
            Assert.ThrowsException<ArgumentException>(() => grade.SubjectName = "Phys");
        }
    }
}

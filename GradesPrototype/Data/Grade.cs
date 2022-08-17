using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GradesPrototype.Data
{

    public class Grade
    {
        public int StudentID { get; set; }

        private string _assessmentDate;
        public string AssessmentDate
        {
            get { return _assessmentDate; }
            set
            {
                if(DateTime.TryParse(value, out DateTime dateTime))
                {
                    if(dateTime > DateTime.Now)
                    {
                        throw new ArgumentOutOfRangeException(nameof(AssessmentDate), "The date must be no later than the current date");
                    }

                    _assessmentDate = dateTime.ToString("d");
                }
                else
                {
                    throw new ArgumentException(nameof(AssessmentDate), "Assessment date is not recognized");
                }
            }
        }

        private string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set
            {
                string subject = (from s in DataSource.Subjects
                          where string.Compare(s, value) == 0
                          select s).FirstOrDefault();

                if(subject is default(string))
                {
                    throw new ArgumentException(nameof(SubjectName), "Subject is not recognized");
                }

                _subjectName = value;
            }
        }

        private string _assessment;
        public string Assessment
        {
            get { return _assessment; }
            set
            {
                Match matchGrade = Regex.Match(value, @"[A-E][+-]?$");
                if (matchGrade.Success)
                {
                    _assessment = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(Assessment), "Assessment grade must be in the range of A+ to E-");
                }
            }
        }

        public string Comments { get; set; }

        public Grade()
        {
            StudentID = 0;
            AssessmentDate = DateTime.Now.ToString("d");
            SubjectName = "Math";
            Assessment = "A";
            Comments = string.Empty;
        }

        public Grade(int studentID, string assessmentDate, string subject, string assessment, string comments)
        {
            StudentID = studentID;
            AssessmentDate = assessmentDate;
            SubjectName = subject;
            Assessment = assessment;
            Comments = comments;
        }
    }
}

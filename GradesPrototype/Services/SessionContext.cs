using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grades.DataModel;

namespace GradesPrototype.Services
{
    // Global context for operations performed by MainWindow
    public static class SessionContext
    {
        public static SchoolGradesDBEntities DBContext = new SchoolGradesDBEntities(new Uri("http://localhost:1655/Services/GradesWebDataService.svc"));

        public static Guid UserID;
        public static string UserName;
        public static Role UserRole;
        public static Student CurrentStudent;
        public static Teacher CurrentTeacher;

        static SessionContext()
        {
            DBContext.MergeOption = System.Data.Services.Client.MergeOption.PreserveChanges;
        }

        public static void Save()
        {
            DBContext.SaveChanges();
        }
    }
}

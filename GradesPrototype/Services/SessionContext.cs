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
        public static SchoolGradesDBEntities DBContext = new SchoolGradesDBEntities();

        public static Guid UserID;
        public static string UserName;
        public static Role UserRole;
        public static Student CurrentStudent;
        public static Teacher CurrentTeacher;

        public static void Save()
        {
            DBContext.SaveChanges();
        }
    }
}

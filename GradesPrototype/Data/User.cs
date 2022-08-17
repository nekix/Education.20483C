using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradesPrototype.Data
{
    // Base class
    public abstract class User
    {
        public string UserName { get; set; }

        // Default password generation
        private string _password = Guid.NewGuid().ToString();
        public string Password
        {
            set { _password = value; } 
        }

        public bool VerifyPassword(string Password)
        {
            return string.Compare(Password, _password, StringComparison.Ordinal) == 0;
        }
    }
}

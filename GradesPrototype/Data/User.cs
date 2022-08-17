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
        protected string _password = Guid.NewGuid().ToString();
        public string Password
        {
            set
            {
                if (SetPassword(value)) { _password = value; return; }
                throw new ArgumentException("Password not complex enough", nameof(Password));
            }
        }

        // String parameter containing the password
        // and return a Boolean value indicating whether
        // the password has been set successfully.
        public abstract bool SetPassword(string password);

        public bool VerifyPassword(string password)
        {
            return string.Compare(password, _password, StringComparison.Ordinal) == 0;
        }
    }
}

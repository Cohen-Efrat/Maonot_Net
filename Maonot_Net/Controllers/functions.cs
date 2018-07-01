using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maonot_Net.Controllers
{
    public class functions
    {

        public bool CheckPassword(string submittedPassword, string hashedPassword)
        {

            bool validPassword = BCrypt.Net.BCrypt.Verify(submittedPassword, hashedPassword);
            return validPassword;
        }
    }
}

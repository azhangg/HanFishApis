using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.CustomExceptions
{
    public class UserOperationException : Exception
    {
        public UserOperationException(string? message) : base(message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.CustomExceptions
{
    public class EncryptException : Exception
    {
        public EncryptException(string? message) : base(message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Exceptions;

public class NullListException : Exception
{
    public NullListException() : base("Returned list is null") { }
}

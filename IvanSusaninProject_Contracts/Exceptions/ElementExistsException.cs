using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Exceptions;

public class ElementExistsException : Exception
{
    public string ParamName { get; private set; }

    public string ParamValue { get; private set; }

    public ElementExistsException(string paramName, string paramValue) : base($"There is already an element with value{paramValue} of parameter {paramName}")
    {
        ParamName = paramName;
        ParamValue = paramValue;
    }
}
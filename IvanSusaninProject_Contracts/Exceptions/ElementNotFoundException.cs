using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Exceptions;

public class ElementNotFoundException : Exception
{
    public string Value { get; private set; }

    public ElementNotFoundException(string value) : base($"Element not found at value = {value}")
    {
        Value = value;
    }
}

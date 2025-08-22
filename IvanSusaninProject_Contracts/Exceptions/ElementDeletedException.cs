using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Exceptions;

public class ElementDeletedException : Exception
{
    public ElementDeletedException(string id) : base($"Cannot modify a deleted item (id: {id})") { }
}

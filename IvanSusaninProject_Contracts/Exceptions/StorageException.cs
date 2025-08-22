using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Exceptions;

public class StorageException : Exception
{
    public StorageException(Exception ex) : base($"Error while working in storage: {ex.Message}", ex) { }
}
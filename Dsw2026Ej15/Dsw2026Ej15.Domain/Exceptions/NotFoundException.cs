using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {

    }
}

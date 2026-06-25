using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Result
{
    public enum ErrorType
    {
            Failure = 0,
            validation = 1,
            NotFound = 2,
            Unauthorized = 3,
            Forbidden = 4,
            InvalidCrendentials = 5,
    }
}

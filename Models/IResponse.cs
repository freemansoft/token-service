using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Model
{
    /// <summary>
    /// Standard response for service calls. Can also be wrapped inside custom exceptions
    /// Implementers of this exist in the Service tier and in the REST controller tier
    /// </summary>
    interface IResponse : IDataVersion, IResponseMessages
    {
    }
}

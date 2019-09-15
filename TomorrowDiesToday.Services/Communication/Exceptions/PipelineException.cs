using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.Exceptions
{
    public class PipelineException : Exception
    {
        public PipelineException(string message) : base(message) { }
    }
}

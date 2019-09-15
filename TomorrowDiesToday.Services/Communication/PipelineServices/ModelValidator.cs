using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// This should validate if the JSON being passed in is able to be converted to existing models.
    /// NOTE:  JsonConvert.Deserialize might produce a null result; OR the library may have a validation method.
    /// Look for existing functionality in the package Newtonsoft.JSON, which is already installed.
    /// </summary>
    internal class ModelValidator : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Communication.Exceptions;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This validates that required values exist in a model (ex: TimeStamp is a required field on all models).
    /// This should not apply specific validation, but rather check the model's IsValid property, as models should validate themselves.
    /// HINT:  Take advantage of the IValidatable interface that models that implement IModel are known to have.
    /// </summary>
    internal class ModelValueValidator : IPipelineService
    {
        public PipelineItem Result { get; private set; }

        public void Process(PipelineItem item)
        {
            PipelineItemStatusResult statusResult;

            if (item == null)
            {
                statusResult = new PipelineItemStatusResult(PipelineItemStatus.Fail, "Pipeline item is null");
            }
            else if (item.Data is IValidatable)
            {
                var validatableItem = item.Data as IValidatable;
                if (validatableItem.IsValid())
                {
                    statusResult = new PipelineItemStatusResult(PipelineItemStatus.Success);
                }
                else
                {
                    statusResult = new PipelineItemStatusResult(PipelineItemStatus.Fail, "Pipeline item failed validation");
                }
            }
            else
            {
                statusResult = new PipelineItemStatusResult(PipelineItemStatus.Fail, "Pipeline item's data is not of type IValidatable");
            }
            
            item.SetStatusResult(statusResult);

            Result = item;
        }
    }
}

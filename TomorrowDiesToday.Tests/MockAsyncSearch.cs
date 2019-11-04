using Amazon.DynamoDBv2.DataModel;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Tests
{
    public class DummyAsyncSearchFactory : DummyFactory<AsyncSearch<PlayerDTO>>
    {
        protected override AsyncSearch<PlayerDTO> Create()
        {
            return (AsyncSearch<PlayerDTO>)System.Runtime.Serialization.FormatterServices
                  .GetUninitializedObject(typeof(AsyncSearch<PlayerDTO>));
        }
    }
}

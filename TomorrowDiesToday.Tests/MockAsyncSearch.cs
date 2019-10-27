using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Tests
{
    public class MockAsyncSearch<T> : AsyncSearch<T>
    {
        public MockAsyncSearch() : base()
        {

        }

        public async Task<List<T>> GetRemainingAsync()
        {
            return await Task.FromResult(Enumerable.Empty<T>().ToList());
        }
    }
}

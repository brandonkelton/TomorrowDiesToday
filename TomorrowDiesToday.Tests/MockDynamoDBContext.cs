using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Tests
{
    public class MockDynamoDBContext : IDynamoDBContext
    {
        public BatchGet<T> CreateBatchGet<T>(DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public BatchWrite<T> CreateBatchWrite<T>(DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public MultiTableBatchGet CreateMultiTableBatchGet(params BatchGet[] batches)
        {
            throw new NotImplementedException();
        }

        public MultiTableBatchWrite CreateMultiTableBatchWrite(params BatchWrite[] batches)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(object hashKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(object hashKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(object hashKey, object rangeKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteBatchGetAsync(BatchGet[] batches, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteBatchWriteAsync(BatchWrite[] batches, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FromDocument<T>(Document document)
        {
            throw new NotImplementedException();
        }

        public T FromDocument<T>(Document document, DynamoDBOperationConfig operationConfig)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents, DynamoDBOperationConfig operationConfig)
        {
            throw new NotImplementedException();
        }

        public AsyncSearch<T> FromQueryAsync<T>(QueryOperationConfig queryConfig, DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public AsyncSearch<T> FromScanAsync<T>(ScanOperationConfig scanConfig, DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public Table GetTargetTable<T>(DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(object hashKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(object hashKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(object hashKey, object rangeKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(T keyObject, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> LoadAsync<T>(T keyObject, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AsyncSearch<T> QueryAsync<T>(object hashKeyValue, DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public AsyncSearch<T> QueryAsync<T>(object hashKeyValue, QueryOperator op, IEnumerable<object> values, DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AsyncSearch<T> ScanAsync<T>(IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig = null)
        {
            throw new NotImplementedException();
        }

        public Document ToDocument<T>(T value)
        {
            throw new NotImplementedException();
        }

        public Document ToDocument<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            throw new NotImplementedException();
        }
    }
}

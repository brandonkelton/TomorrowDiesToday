using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Tests
{
    public class MockAmazonDynamoDB : IAmazonDynamoDB
    {
        private BatchGetItemResponse fakeBatchGetItemResponse = new BatchGetItemResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private BatchWriteItemResponse fakeBatchWriteItemResponse = new BatchWriteItemResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private CreateBackupResponse fakeCreateBackupResponse = new CreateBackupResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private CreateGlobalTableResponse fakeCreateGlobalTableResponse = new CreateGlobalTableResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private CreateTableResponse fakeCreateTableResponse = new CreateTableResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DeleteBackupResponse fakeDeleteBackupResponse = new DeleteBackupResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DeleteItemResponse fakeDeleteItemResponse = new DeleteItemResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DeleteTableResponse fakeDeleteTableResponse = new DeleteTableResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DescribeBackupResponse fakeDescribeBackupResponse = new DescribeBackupResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DescribeContinuousBackupsResponse fakeDescribeContinuousBackupsResponse = new DescribeContinuousBackupsResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        private DescribeEndpointsResponse fakeDescribeEndpointsResponse = new DescribeEndpointsResponse
        {
            HttpStatusCode = HttpStatusCode.OK
        };

        public IClientConfig Config { get; } = new AmazonDynamoDBConfig();

        public async Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeBatchGetItemResponse);
        }

        public async Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeBatchGetItemResponse);
        }

        public async Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeBatchGetItemResponse);
        }

        public async Task<BatchWriteItemResponse> BatchWriteItemAsync(Dictionary<string, List<WriteRequest>> requestItems, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeBatchWriteItemResponse);
        }

        public async Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeBatchWriteItemResponse);
        }

        public async Task<CreateBackupResponse> CreateBackupAsync(CreateBackupRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeCreateBackupResponse);
        }

        public async Task<CreateGlobalTableResponse> CreateGlobalTableAsync(CreateGlobalTableRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeCreateGlobalTableResponse);
        }

        public async Task<CreateTableResponse> CreateTableAsync(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeCreateTableResponse);
        }

        public async Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeCreateTableResponse);
        }

        public async Task<DeleteBackupResponse> DeleteBackupAsync(DeleteBackupRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteBackupResponse);
        }

        public async Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteItemResponse);
        }

        public async Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, ReturnValue returnValues, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteItemResponse);
        }

        public async Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteItemResponse);
        }

        public async Task<DeleteTableResponse> DeleteTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteTableResponse);
        }

        public async Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDeleteTableResponse);
        }

        public async Task<DescribeBackupResponse> DescribeBackupAsync(DescribeBackupRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDescribeBackupResponse);
        }

        public async Task<DescribeContinuousBackupsResponse> DescribeContinuousBackupsAsync(DescribeContinuousBackupsRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDescribeContinuousBackupsResponse);
        }

        public async Task<DescribeEndpointsResponse> DescribeEndpointsAsync(DescribeEndpointsRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(fakeDescribeEndpointsResponse);
        }

        public async Task<DescribeGlobalTableResponse> DescribeGlobalTableAsync(DescribeGlobalTableRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeGlobalTableSettingsResponse> DescribeGlobalTableSettingsAsync(DescribeGlobalTableSettingsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeLimitsResponse> DescribeLimitsAsync(DescribeLimitsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeTableResponse> DescribeTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeTableResponse> DescribeTableAsync(DescribeTableRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(string tableName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(DescribeTimeToLiveRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        public async Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, bool consistentRead, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<GetItemResponse> GetItemAsync(GetItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListBackupsResponse> ListBackupsAsync(ListBackupsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListGlobalTablesResponse> ListGlobalTablesAsync(ListGlobalTablesRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTablesResponse> ListTablesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, int limit, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTablesResponse> ListTablesAsync(int limit, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTablesResponse> ListTablesAsync(ListTablesRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ListTagsOfResourceResponse> ListTagsOfResourceAsync(ListTagsOfResourceRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, ReturnValue returnValues, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PutItemResponse> PutItemAsync(PutItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<RestoreTableFromBackupResponse> RestoreTableFromBackupAsync(RestoreTableFromBackupRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<RestoreTableToPointInTimeResponse> RestoreTableToPointInTimeAsync(RestoreTableToPointInTimeRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ScanResponse> ScanAsync(string tableName, Dictionary<string, Condition> scanFilter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, Dictionary<string, Condition> scanFilter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ScanResponse> ScanAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TagResourceResponse> TagResourceAsync(TagResourceRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UntagResourceResponse> UntagResourceAsync(UntagResourceRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateContinuousBackupsResponse> UpdateContinuousBackupsAsync(UpdateContinuousBackupsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateGlobalTableResponse> UpdateGlobalTableAsync(UpdateGlobalTableRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateGlobalTableSettingsResponse> UpdateGlobalTableSettingsAsync(UpdateGlobalTableSettingsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateTableResponse> UpdateTableAsync(string tableName, ProvisionedThroughput provisionedThroughput, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateTimeToLiveResponse> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using challenge.cloud.register.Interfaces;
using challenge.cloud.register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.cloud.register.Persistence
{
    public class PersistenceManager : IPersistenceManager
    {
        #region attributes
        private readonly AmazonDynamoDBClient _dynamoClient;
        #endregion

        #region constructor
        public PersistenceManager()
        {
            _dynamoClient = new AmazonDynamoDBClient();
        }

        public PersistenceManager(AmazonDynamoDBClient dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
        #endregion

        #region public methods
        public async Task<Document> Insert(string tableName, User user)
        {
            try
            {
                Table table = Table.LoadTable(_dynamoClient, tableName);
                return await table.PutItemAsync(user.ToDynamoDocument());
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { LambdaLogger.Log($"Error> {e.Message}"); }

            return null;
        }

        public async Task<User> Find(string tableName, string email)
        {
            try
            {
                Table table = Table.LoadTable(_dynamoClient, tableName);
                var document = await table.GetItemAsync(email);
                if (document != null)                 
                    return User.ParseFromDynamoDocument(document);                
            }
            catch (ResourceNotFoundException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (AmazonDynamoDBException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (Exception e) { LambdaLogger.Log($"Error> {e.Message}"); }

            return null;
        }

        public async Task<Document> Update(string tableName, User user)
        {
            try
            {
                Table table = Table.LoadTable(_dynamoClient, tableName);
                return await table.UpdateItemAsync(user.ToDynamoDocument());
            }
            catch (AmazonDynamoDBException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (AmazonServiceException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (Exception e) { LambdaLogger.Log($"Error> {e.Message}"); }

            return null;
        }

        public async Task<Document> Delete(string tableName, string email)
        {
            try
            {
                Table table = Table.LoadTable(_dynamoClient, tableName);
                return await table.DeleteItemAsync(email);
            }
            catch (AmazonDynamoDBException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (AmazonServiceException e) { LambdaLogger.Log($"Error> {e.Message}"); }
            catch (Exception e) { LambdaLogger.Log($"Error> {e.Message}"); }

            return null;
        }
        #endregion
    }
}

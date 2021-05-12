using Amazon.DynamoDBv2.DocumentModel;
using challenge.cloud.register.Models;
using System;
using System.Threading.Tasks;

namespace challenge.cloud.register.Interfaces
{
    public interface IPersistenceManager
    {
        Task<Document> Delete(string tableName, string email);
        Task<User> Find(string tableName, string email);
        Task<Document> Insert(string tableName, User user);
        Task<Document> Update(string tableName, User user);
    }
}
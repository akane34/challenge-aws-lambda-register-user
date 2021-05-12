using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.Runtime;
using challenge.cloud.register.Commons;
using challenge.cloud.register.Interfaces;
using challenge.cloud.register.Models;
using challenge.cloud.register.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace challenge.cloud.register
{
    public class Function
    {
        #region attributes
        private readonly IPersistenceManager _persistenceManager;
        #endregion

        #region constructors
        public Function()
        {
            _persistenceManager = new PersistenceManager();
        }
        #endregion

        #region handlers
        public async Task FunctionHandler(SNSEvent events, ILambdaContext context)
        {
            try
            {
                foreach (var record in events.Records)
                {
                    await ProcessMessageAsync(record.Sns);
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error: {ex.Message}");
            }            
        }
        #endregion

        #region provate methods

        private async Task ProcessMessageAsync(SNSEvent.SNSMessage message)
        {
            if (message == null || message.Message == null || message.Message.Contains("Error:")) 
                LambdaLogger.Log($"Error: {message?.Message}"); 

            LambdaLogger.Log($"Processed message {message?.Message}");

            EventMessage eventMessage = JsonConvert.DeserializeObject<EventMessage>(message?.Message);

            await RegisterUser(eventMessage);
        }

        private async Task RegisterUser(EventMessage eventMessage)
        {
            User user = await _persistenceManager.Find(Configuration.USERS_TABLE_NAME, eventMessage.Email);
            if (user == null)
            {
                user = new User()
                {
                    Email = eventMessage.Email,
                    Name = eventMessage.Name,
                    LastName = eventMessage.FamilyName,
                    CreationDateUTC = DateTime.UtcNow                    
                };

                await _persistenceManager.Insert(Configuration.USERS_TABLE_NAME, user);
            }
        }
        #endregion
    }
}

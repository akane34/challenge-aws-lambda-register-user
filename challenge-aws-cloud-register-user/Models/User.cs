using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.cloud.register.Models
{
    public class User
    {
        #region properties
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("creationDateUtc")]
        public DateTime CreationDateUTC { get; set; }

        [JsonProperty("updateDateUtc")]
        public DateTime UpdateDateUTC { get; set; }
        #endregion

        #region public methods
        public JObject ToJson()
        {
            return JObject.FromObject(this);
        }

        public Document ToDynamoDocument()
        {
            var document = new Document();
            document["Email"] = Email;
            document["Name"] = Name;
            document["LastName"] = LastName;
            document["CreationDateUTC"] = CreationDateUTC;
            document["UpdateDateUTC"] = UpdateDateUTC;            

            return document;
        }

        public static User ParseFromDynamoDocument(Document document)
        {
            User user = null;

            if (document != null)
            {
                user = new User();
                user.Email = document["Email"]?.AsPrimitive().Value.ToString();
                user.Name = document["Name"]?.AsPrimitive().Value.ToString();                
                user.LastName = document["LastName"]?.AsPrimitive().Value.ToString();                
                user.CreationDateUTC = DateTime.Parse(document["CreationDateUTC"]?.AsPrimitive().Value.ToString());
                user.UpdateDateUTC = DateTime.Parse(document["UpdateDateUTC"]?.AsPrimitive().Value.ToString());
            }

            return user;
        }
        #endregion
    }
}

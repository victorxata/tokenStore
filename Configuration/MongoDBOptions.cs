namespace Tokens.Configuration
{
    public class MongoDBOptions
    {
        public string Url { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }

        public MongoDBOptions()
        {
        }
    }
}
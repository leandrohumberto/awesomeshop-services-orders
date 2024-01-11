using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;

namespace AwesomeShop.Services.Orders.Infrastructure.Persistence.SerializationProviders
{
    // Referências:
    // https://stackoverflow.com/questions/21386347/how-do-i-detect-whether-a-mongodb-serializer-is-already-registered
    // https://stackoverflow.com/questions/63443445/trouble-with-mongodb-c-sharp-driver-when-performing-queries-using-guidrepresenta
    public class CustomGuidSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            return type == typeof(Guid) ? new GuidSerializer(GuidRepresentation.Standard) : null;
        }
    }
}

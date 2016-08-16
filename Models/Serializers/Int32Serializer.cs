using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;

namespace Models.Serializers
{
    public sealed class Int32Serializer : IBsonSerializer
    {
        public Type ValueType
        {
            get
            {
                return typeof(int);
            }
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            //var reader = context.Reader;
            var t = context.Reader.CurrentBsonType;
            var n = args.NominalType;

            switch (t)
            {
                case BsonType.Null:
                    context.Reader.ReadNull();
                    return null;

                case BsonType.Int32:
                    return context.Reader.ReadInt32();

                default:
                    throw new BsonSerializationException($"Cannot deserialize from int32 to {t.ToString()}");
            }
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value != null)
            {
                context.Writer.WriteInt32((Int32)value);
            }
            else
            {
                context.Writer.WriteInt32(0);
            }
        }
    }
}

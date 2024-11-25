using System.IO;
using System.Threading.Tasks;

namespace Project.Persistence{
    public sealed class JsonSerializer : ISerializer
    {
        readonly Newtonsoft.Json.JsonSerializer m_serializer = new Newtonsoft.Json.JsonSerializer();
        public Task<TData> DeserializeAsync<TData>(Stream reader)
        {   
            using var streamReader = new StreamReader(reader);
            using var jsonReader = new Newtonsoft.Json.JsonTextReader(streamReader);
            TData data = m_serializer.Deserialize<TData>(jsonReader);
            return Task.FromResult(data);
        }

        public Task SerializeAsync<TData>(TData data, Stream writer)
        {
            using var streamWriter = new StreamWriter(writer);
            using var jsonWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);
            try{
                m_serializer.Serialize(jsonWriter, data);
                return Task.CompletedTask;
            }
            catch{
                return Task.FromException(new System.Exception("Failed to serialize"));
            }
        }
    }
}
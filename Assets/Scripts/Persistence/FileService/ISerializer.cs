using System.IO;
namespace Project.Persistence{
    public interface ISerializer{
        System.Threading.Tasks.Task<TData> DeserializeAsync<TData>(Stream reader);
        System.Threading.Tasks.Task SerializeAsync<TData>(TData data, Stream writer);
    }
}
using System.Threading.Tasks;
namespace Project.Persistence{
    public interface IPersistenceService{
        Task<bool> Save<TData>(TData data, string targetPath);
        Task<TData> Load<TData>(string targetPath);
    }
}
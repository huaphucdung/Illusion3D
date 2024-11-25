using System.Threading.Tasks;

namespace Project.Persistence{
    public sealed class FileService : IPersistenceService
    {
        private readonly ISerializer m_serializer;
        private readonly string m_rootFilePath;
        public FileService(string rootFilePath, ISerializer serializer){
            if(string.IsNullOrEmpty(rootFilePath)) throw new System.ArgumentNullException(nameof(rootFilePath));
            m_rootFilePath = rootFilePath;
            m_serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
        }
        public Task<TData> Load<TData>(string targetPath)
        {
            string fullPath = System.IO.Path.Combine(m_rootFilePath, targetPath);
            using var stream = System.IO.File.OpenRead(fullPath);
            return m_serializer.DeserializeAsync<TData>(stream);
        }

        public async Task<bool> Save<TData>(TData data, string targetPath)
        {
            string fullPath = System.IO.Path.Combine(m_rootFilePath, targetPath);
            var stream = System.IO.File.OpenWrite(fullPath);
            bool isSuccess = false;
            try{
                await m_serializer.SerializeAsync(data, stream);
                isSuccess = true;
            }
            catch{
                isSuccess = false;
            }
            finally{
                stream.Dispose();
            }

            return isSuccess;
        }
    }
}
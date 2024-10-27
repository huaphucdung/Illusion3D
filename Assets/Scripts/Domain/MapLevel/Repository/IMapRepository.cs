using System.Collections;

namespace Project.Domain.MapLevel{
    public interface IMapRepository : System.IDisposable{
        IMapTable MapTable { get; }
        IEnumerator FetchMapTable();
    }
}
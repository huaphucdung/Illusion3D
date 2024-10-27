using System.Threading.Tasks;
using UnityEngine;

namespace Project.Domain.Player{
    public sealed class PlayerGatewayAPI : System.IDisposable{
        Task<PlayerData> LoadPlayerDataAsync() {
            return null;
        }
        public void Dispose(){}
    }
}
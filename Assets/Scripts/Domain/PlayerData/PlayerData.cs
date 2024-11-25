namespace Project.Domain.Player{

    public sealed class PlayerData{
        public readonly ushort Id;
        public readonly uint CurrentLevel;

        public PlayerData(){
            Id = 1;
            CurrentLevel = 1;
        }
    }
}
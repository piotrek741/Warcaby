namespace Warcaby
{
    public enum PlayerType { A, B }

    public class Player {
        public PlayerType PlayerType { get; private set; }

        public Player(PlayerType playerType) {
            PlayerType = playerType;
        }
    }
    
}
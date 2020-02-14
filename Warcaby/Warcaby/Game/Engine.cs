using System.Collections.Generic;

namespace Warcaby
{
    internal class Engine {
        internal bool End { get; private set; } = false;
        internal IReadOnlyDictionary<PlayerType, int> Points { get => points; }
        
        readonly private List<Player> Players = new List<Player> { new Player(PlayerType.A), new Player(PlayerType.B) };
        internal Player CurrentPlayer { get; private set; }

        private readonly Dictionary<PlayerType, int> points = new Dictionary<PlayerType, int>() { { PlayerType.A, 0}, { PlayerType.B, 0} };

        public Engine() {
            CurrentPlayer = Players[0];
        }

        internal bool Move(int sourceX, int sourceY, int targetX, int targetY)
        {
            PlayerType enemyPlayerType = CurrentPlayer.PlayerType == PlayerType.A ? PlayerType.B : PlayerType.A;

            if (Board[targetX, targetY].FieldType != FieldType.WarArea) {
                return false;
            } else if (sourceX - targetX > 2 || sourceX - targetX < -2) {
                return false;
            } else if (sourceY - targetY > 2 || sourceY - targetY < -2) {
                return false;
            } else if (sourceX - targetX == 2 || sourceX - targetX == -2 || sourceY - targetY == 2 || sourceY - targetY == -2) {
                // tutaj gdzies trzeba dodac obsuge queen np
                if (targetX == sourceX + 2 && targetY == sourceY + 2 && Board[sourceX - 1, sourceY + 1].IsQueen()) {
                    //obluga krolowej 
                } else if (targetX == sourceX + 2 && targetY == sourceY + 2) {
                    if (IsValidArea(sourceX + 1, sourceY + 1) && Board[sourceX + 1, sourceY + 1].IsEnemy(enemyPlayerType)) {
                        Insert(FieldType.WarArea, sourceX + 1, sourceY + 1);
                    } else {
                        return false;
                    }
                } else if (targetX == sourceX - 2 && targetY == sourceY + 2) {
                    if (IsValidArea(sourceX - 1, sourceY + 1) && Board[sourceX - 1, sourceY + 1].IsEnemy(enemyPlayerType)) {
                        Insert(FieldType.WarArea, sourceX - 1, sourceY + 1);
                    } else {
                        return false;
                    }
                } else if (targetX == sourceX + 2 && targetY == sourceY - 2) {
                    if (IsValidArea(sourceX + 1, sourceY - 1) && Board[sourceX + 1, sourceY - 1].IsEnemy(enemyPlayerType)) {
                        Insert(FieldType.WarArea, sourceX + 1, sourceY - 1);
                    } else {
                        return false;
                    }
                } else if (targetX == sourceX - 2 && targetY == sourceY - 2) {
                    if (IsValidArea(sourceX - 1, sourceY - 1) && Board[sourceX - 1, sourceY - 1].IsEnemy(enemyPlayerType)) {
                        Insert(FieldType.WarArea, sourceX - 1, sourceY - 1);
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                points[CurrentPlayer.PlayerType]++;
            }

            Insert(Board[sourceX, sourceY], targetX, targetY);
            Insert(FieldType.WarArea, sourceX, sourceY);


            if (targetX == 0 && CurrentPlayer.PlayerType == PlayerType.A || targetX == 7 && CurrentPlayer.PlayerType == PlayerType.B) {
                Board[targetX, targetY].MakeAQueen();
            }
            ChangePlayer();

            End = IsEnd();

            return true;
        }

        public bool IsPossibleSource(int x, int y) {
            if (CurrentPlayer.PlayerType == PlayerType.A && Board[x, y].IsEnemy(PlayerType.A)) {
                return true;
            }
            if (CurrentPlayer.PlayerType == PlayerType.B && Board[x, y].IsEnemy(PlayerType.B)) {
                return true;
            }

            return false;
        }

        private bool IsEnd()
        {
            int playerACheckers = 0;
            int playerBCheckers = 0;

            for (int i = 0; i < Config.AREA_COUNT; i++) {
                for (int j = 0; j < Config.AREA_COUNT; j++) {
                    if (Board[i, j].IsEnemy(PlayerType.A)) {
                        playerACheckers++;
                    } else if (Board[i, j].IsEnemy(PlayerType.B)) {
                        playerBCheckers++;
                    }
                }
            }

            return playerACheckers == 0 || playerBCheckers == 0;
        }
        private bool IsValidArea(int x, int y) {
            return x >= 0 && x <= Config.AREA_COUNT && y >= 0 && y <= Config.AREA_COUNT;
        }

        private void Insert(FieldType gameObject, int targetX, int targetY) {
            Board[targetX, targetY] = new Field(gameObject);
        }

        private void Insert(Field field, int targetX, int targetY) {
            Board[targetX, targetY] = field;
        }

        private void ChangePlayer() {
            CurrentPlayer = (CurrentPlayer == Players[0] ? Players[1] : Players[0]);
        }
        
        internal Field[,] Board { get; } = new Field[,]
        {
            { new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB) },
            { new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea) },
            { new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerB) },
            { new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea) },
            { new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea), new Field(FieldType.EmptyArea), new Field(FieldType.WarArea) },
            { new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea) },
            { new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA) },
            { new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea), new Field(FieldType.PlayerA), new Field(FieldType.EmptyArea) }
        };
    }
}
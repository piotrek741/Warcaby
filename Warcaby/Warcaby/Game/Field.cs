using System;

namespace Warcaby
{
    public enum FieldType { EmptyArea, WarArea, PlayerA, PlayerB, PlayerAQueen, PlayerBQueen }

    public class Field {
        public FieldType FieldType { get; private set; }

        public Field (FieldType fieldType) {
            FieldType = fieldType;
        }

        public bool IsEnemy (PlayerType enemyPlayerType) {
            try {
                return GetPlayerType() == enemyPlayerType;
            } catch (NullReferenceException e) {
                return false;
            }
        }

        public void MakeAQueen() {
            FieldType = (FieldType == FieldType.PlayerA ? FieldType.PlayerAQueen : FieldType.PlayerBQueen);
        }

        public bool IsQueen() {
            return FieldType == FieldType.PlayerAQueen || FieldType == FieldType.PlayerBQueen;
        }

        private PlayerType GetPlayerType() {
            FieldType[] playerApossibleFields = { FieldType.PlayerA, FieldType.PlayerAQueen };
            FieldType[] playerBpossibleFields = { FieldType.PlayerB, FieldType.PlayerBQueen };
            if (Array.Exists(playerApossibleFields, element => element == FieldType)) {
                return PlayerType.A;
            }
            if (Array.Exists(playerBpossibleFields, element => element == FieldType)) {
                return PlayerType.B;
            }

            throw new NullReferenceException();
        }
    }
}
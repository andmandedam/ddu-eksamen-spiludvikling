using UnityEngine;

public partial class GameManager
{
    public class RoomObject
    {
        public Vector2Int placement;
        public GameObject enemyPrefab;
        public int enemyAmount;
        public GameObject roomPrefab;
        public RoomTag tags;
        public GameManager.RoomType roomType;

        public RoomObject(RoomType roomType, RoomTag flag, Vector2Int placement, GameManager gameManager)
        {
            this.roomType = roomType;
            this.tags = flag;
            this.placement = placement;

            this.roomPrefab = Util.ChooseRandomElement(gameManager.prefabDictionary[roomType]);
            this.enemyAmount = (int)gameManager.enemySpawnStatsDictionary[roomType];
            if ((this.tags & RoomTag.Hard) != 0)
            {
                this.enemyAmount = (int)(gameManager.hardRoomMultiplier * this.enemyAmount) + 1;
            }

            if ((this.tags & RoomTag.BigEnemies) != 0)
            {
                this.enemyPrefab = Util.ChooseRandomElement(gameManager.prefabDictionary[EnemyType.Big]);
                this.enemyAmount = (int)(this.enemyAmount * 0.5);
            } else {
                this.enemyPrefab = Util.ChooseRandomElement(gameManager.prefabDictionary[EnemyType.Small]);            
            }
        }
    }
}

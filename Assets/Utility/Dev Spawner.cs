using UnityEngine;

public class DevSpawner : MonoBehaviour {
    [Header("Spawner Settings")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] RoomData devRoomData;

    private void Awake() {
        GameObject enemy = Instantiate(enemyPrefab, devRoomData.EnemySpawns[0].position, Quaternion.identity);
        enemy.name = $"{enemyPrefab.name}";
        enemy.GetComponent<EnemyBrain>().Initialize(devRoomData, true, true);
    }
}

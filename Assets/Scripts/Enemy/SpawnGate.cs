using UnityEngine;
using System.Collections;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] Transform spawnPoint;

    PlayerHealth player;

    private void Start() {
        player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(SpawnRoutine());    
    }

    IEnumerator SpawnRoutine(){
        while(player)
        {
            Instantiate(robotPrefab, spawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}

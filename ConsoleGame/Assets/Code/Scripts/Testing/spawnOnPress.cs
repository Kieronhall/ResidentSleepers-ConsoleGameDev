using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnPress : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRadius = 5.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            Vector3 randomSpawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            randomSpawnPoint.y = transform.position.y;
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, randomSpawnPoint, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

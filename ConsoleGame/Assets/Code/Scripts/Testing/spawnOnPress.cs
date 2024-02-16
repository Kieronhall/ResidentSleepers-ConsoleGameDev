using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class spawnOnPress : MonoBehaviour
{
    private ThirdPersonPlayerInputs playerInputs;
    private InputAction spawnAI;
    public GameObject prefabToSpawn;
    public float spawnRadius = 5.0f;

    private void Awake()
    {
        playerInputs = new ThirdPersonPlayerInputs();
    }

    private void OnEnable()
    {
        spawnAI = playerInputs.Player.Spawn;
        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Enable();
    }

    void Update()
    {
        if (spawnAI.triggered)
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

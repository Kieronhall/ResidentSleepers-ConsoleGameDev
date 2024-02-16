using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class spawnOnPress : MonoBehaviour
{
    private ThirdPersonPlayerInputs playerInputs;
    private InputAction spawnAI;
    public GameObject prefabToSpawn;
    public float spawnRadius = 5.0f;
    public TMP_Text debugText;
    public int enemiesCount=0;

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
        //if (spawnAI.triggered)
        //{
        //    SpawnPrefab();
        //}
        if (Input.GetButtonDown("PS4_X_Button"))
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
            enemiesCount++;
            debugText.text = "Spawned extra: " + enemiesCount;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

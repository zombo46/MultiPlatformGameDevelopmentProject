using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject oxygenPrefab;
    public Terrain terrain;
    public int spawnCount = 30;

    [Header("SpawnParameters")]
    public float spawnHeight = 5f;
    public float minY = 10f;
    public float maxAttempts = 60f;
    public float minDistanceBetweenObjects = 2f;
    public LayerMask obstacleMask;

    // Name of the layer to put oxygen collectables on (must exist in Project Settings -> Tags and Layers)
    public string oxygenLayerName = "OxygenCollectable";

    private int instancesSpawned = 0;
    private int oxygenLayerIndex = 0;
    private int oxygenLayerMask = 0;

    void Start()
    {
        if (!terrain || !oxygenPrefab)
        {
            Debug.LogError("OxygenSpawner: Terrain or OxygenPrefab not assigned.");
            return;
        }

        // Resolve layer index and mask, validate existence
        oxygenLayerIndex = LayerMask.NameToLayer(oxygenLayerName);
        if (oxygenLayerIndex < 0)
        {
            Debug.LogError($"OxygenSpawner: Layer '{oxygenLayerName}' not defined.");
            oxygenLayerIndex = 0;
        }

        SpawnOxygen();
    }

    void SpawnOxygen()
    {
        int attempts = 0;

        while (instancesSpawned < spawnCount && attempts < maxAttempts * spawnCount)
        {
            attempts++;

            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0f, z)) + terrain.transform.position.y;

            if (y > spawnHeight)
            {
                continue;
            }

            Vector3 position = new Vector3(x, y + 0.5f, z);

            bool touchesObstacle = Physics.CheckSphere(position, minDistanceBetweenObjects, obstacleMask);
            if (touchesObstacle)
            {
                continue;
            }

            // Check if there are existing oxygen objects nearby using the cached mask
            if (oxygenLayerMask != 0 && Physics.CheckSphere(position, minY, oxygenLayerMask))
            {
                continue;
            }

            GameObject oxygenInstance = Instantiate(oxygenPrefab, position, Quaternion.identity);
            oxygenInstance.layer = oxygenLayerIndex;
            instancesSpawned++;
        }
    }
}

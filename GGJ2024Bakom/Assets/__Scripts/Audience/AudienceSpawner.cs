//Created by Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceSpawner : MonoBehaviour
{
    public GameObject audiencePrefab;
    public Transform spawnLocation;
    public Transform playerTransform;
    private float minDistanceToPlayer = 5.0f;
    private int numberOfAudienceToSpawn = 5;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAudience();
    }

    private void SpawnAudience()
    {
        for (int i = 0; i < numberOfAudienceToSpawn; i++)
        {
            Vector3 randomSpawnPos;

            do
            {
                float randomX = Random.Range(-10, 11);
                float randomZ = Random.Range(-10, 11);

                randomSpawnPos = spawnLocation.TransformPoint(new Vector3(randomX, -1.5f, randomZ));
            } 
            while (IsTooCloseToPlayer(randomSpawnPos));

            GameObject audienceInstance = Instantiate(audiencePrefab, randomSpawnPos, Quaternion.identity);

            // Set the parent of the instantiated audience to the spawnLocation
            audienceInstance.transform.parent = spawnLocation;

            // Assign a random color to the audience
            MeshRenderer audienceRenderer = audienceInstance.GetComponent<MeshRenderer>();
            if (audienceRenderer != null)
            {
                audienceRenderer.material.color = Random.ColorHSV();
            }
        }
    }

    bool IsTooCloseToPlayer(Vector3 position)
    {
        // Check the distance between the spawned position and the player's position
        float distanceToPlayer = Vector3.Distance(position, playerTransform.position);
        return distanceToPlayer < minDistanceToPlayer;
    }
}

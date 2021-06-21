using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvSpawner : MonoBehaviour
{
    [Header("ENV")]
    public GameObject[] env;


    [Header("RESOURCES")]
    public GameObject[] resources;
    public int amountOfResources;
    public int resourceSpawnChance;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        SpawnResources();
    }

    // Update is called once per frame
    void Spawn()
    {
        GameObject randEnv = env[Random.Range(0, env.Length)];
        Instantiate(randEnv, transform.position, Quaternion.identity, transform);
    }

    void SpawnResources() {
        for (int i = 0; i < amountOfResources; i++)
        {
            int randomNum = Random.Range(0, 101);
            if (randomNum < resourceSpawnChance) {
                float randX = Random.Range(-range, range);
                float randY = Random.Range(-range, range);
                GameObject randResource = resources[Random.Range(0, resources.Length)];
                Instantiate(randResource, new Vector2(transform.position.x + randX, transform.position.y + randY), Quaternion.identity, transform);
            }
        }
    }
}

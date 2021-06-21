using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave {
        public Enemy[] enemies;
        public int count;
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;

    private AudioManager audio;
    private bool canSpawn = false;
    private int count = 0;
    private bool spawned = false;

    private void Start() {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update() {
        if (canSpawn) {
            if (waves.Length >= StateCtrl.level) {
                SpawnEnemy(waves[StateCtrl.level - 1]);
            } else {
                // If player runs out of levels, repeat the last 5 (if they exist)
                if (waves.Length >= 5) {
                    SpawnEnemy(waves[waves.Length - Random.Range(1, 6)]);
                } else {
                    SpawnEnemy(waves[waves.Length - 1]);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canSpawn = true;
        }
    }

    private void SpawnEnemy(Wave currentWave) {
        if (spawned == false && count < currentWave.count) {
            spawned = true;
            
            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            int randomNum = Random.Range(0, 101);
            if (randomNum < randomEnemy.spawnChance) {
                Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);
                // audio.Play("spawn");
                count += 1;
            }

            StartCoroutine(SpawnCooldown(currentWave));
        }
    }

    private IEnumerator SpawnCooldown(Wave currentWave) {
        yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        spawned = false;
    }
}

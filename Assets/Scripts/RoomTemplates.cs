using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public int amount;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;
    public List<GameObject> rooms;

    public float waitTime;
    public GameObject boss;
    public bool canSpawn = true;
    public GameObject portal;
    private bool spawnedBoss;

    void Update() {
        if (canSpawn == true && (rooms.Count - 1) >= amount) {
            canSpawn = false;
        }
        else if (canSpawn == false && (rooms.Count - 1) < amount) {
            canSpawn = true;
        }
        

        if (waitTime <= 0 && spawnedBoss == false) {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1) {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    Instantiate(portal, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        } else {
            waitTime -= Time.deltaTime;
        }
    }
}

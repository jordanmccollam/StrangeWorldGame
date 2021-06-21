using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public float waitTime = 4f;
    public bool spawned = false;
    public int openingDirection;
    // 1 -> need bottom door
    // 2 -> need top door
    // 3 -> need left door
    // 4 -> need right door
    // X = 21 Y = 15

    private RoomTemplates templates;
    private int rand;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (spawned == false) {
            if (templates.canSpawn == false) {
                // CLOSE ALL OPEN ROOMS
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, templates.transform);
            } else {
                switch(openingDirection) {
                    case 1:
                        // spawn BOTTOM door
                        rand = Random.Range(0, templates.bottomRooms.Length);
                        Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, templates.transform);
                        break;
                    case 2:
                        // spawn TOP door
                        rand = Random.Range(0, templates.topRooms.Length);
                        Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, templates.transform);
                        break;
                    case 3:
                        // spawn LEFT door
                        rand = Random.Range(0, templates.leftRooms.Length);
                        Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, templates.transform);
                        break;
                    case 4:
                        // spawn RIGHT door
                        rand = Random.Range(0, templates.rightRooms.Length);
                        Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, templates.transform);
                        break;
                    default:
                        break;
                }
            }
            spawned = true;


        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "SpawnPoint") {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false && templates.closedRoom != null) {
                // Already spawned here
                // Spawn walls blocking off any openings
                // templates.rooms.Remove(transform.parent.gameObject);
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, templates.transform);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}

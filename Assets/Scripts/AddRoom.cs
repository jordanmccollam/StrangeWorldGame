using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    public bool isClosedRoom = false;
    private RoomTemplates templates;

    void Start() {
        if (isClosedRoom == false) {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }
    }
}

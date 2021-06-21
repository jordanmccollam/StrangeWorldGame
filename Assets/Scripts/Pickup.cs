using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isWeapon;
    public Weapon weaponToEquip;

    public bool isOre;

    private AudioManager audio;


    void Start() {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            // if (isWeapon == true) {
            //     other.GetComponent<Player>().ChangeWeapon(weaponToEquip);
            //     Destroy(gameObject);
            // } 
            
            if (isOre == true) {
                audio.Play("pickup");
                Inventory.ore += 1;
                Destroy(gameObject);
            }
        }
    }
}

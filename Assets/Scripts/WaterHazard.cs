using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    public int damage;
    private AudioManager audio;

    private void Start() {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            audio.Play("burn");
        }
    }
}

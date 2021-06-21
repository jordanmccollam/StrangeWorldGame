using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRock : MonoBehaviour
{
    public int health;
    public int pickupChance;
    public int minDrop;
    public int maxDrop;
    public GameObject drop;
    public GameObject defaultRock; 
    public GameObject crackedRock; 

    private int fullHealth;
    private bool cracked = false;
    private AudioManager audio;

    void Start() {
        fullHealth = health;
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;
        audio.Play("rock breaking");

        if (health <= 0) {
            OnBreak();
        }
        else if (health <= fullHealth / 2 && cracked == false) {
            cracked = true;
            crackedRock.SetActive(true);
            defaultRock.SetActive(false);
        }
    }

    private void OnBreak() {
        int randomNum = Random.Range(0, 101);
        if (randomNum < pickupChance) {
            for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
            {
                Instantiate(drop, transform.position, transform.rotation);   
            }
        }

        Destroy(gameObject);
    }
}

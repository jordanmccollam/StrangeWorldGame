using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float speed;
    public float sight;
    public int damage;
    public float timeBetweenAttacks;
    public float attackTime;
    public int spawnChance;
    public int pickupChance;
    public GameObject[] pickups;
    public int healthPickupChance;
    public GameObject healthPickup;

    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public AudioManager audio;
    private bool canMakeNoise = true;
    private Rigidbody2D rb;
    private Vector2 vel;

    public virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vel = rb.velocity;
        health += (StateCtrl.level - 1);
    }

    public void TakeDamage(int damageAmount, Vector2 knockback, float knocktime) {
        health -= damageAmount;
        rb.velocity = knockback;

        if (canMakeNoise) {
            // audio.Play("monster hurt");
            canMakeNoise = false;
            StartCoroutine(NoiseCooldown());
        }
        audio.Play("rock breaking");

        if (health <= 0) {
            Die();
        } else {
            StartCoroutine(StunTime(knocktime));
        }
    }

    private IEnumerator StunTime(float knocktime) {
        yield return new WaitForSeconds(knocktime);
        rb.velocity = vel;
    }

    private IEnumerator NoiseCooldown() {
        yield return new WaitForSeconds(.5f);
        canMakeNoise = true;
    }

    private void Die() {
        audio.Play("monster hurt");
        int randomNum = Random.Range(0, 101);
        if (randomNum < pickupChance) {
            GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
            Instantiate(randomPickup, transform.position, transform.rotation);
        }

        int randomHealth = Random.Range(0, 101);
        if (randomHealth < healthPickupChance) {
            Instantiate(healthPickup, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void Update() {
        transform.rotation = Quaternion.Euler(0, player.position.x > transform.position.x ? 0 : 180f, 0);
    }
}

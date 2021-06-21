﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject explosion;

    private Player playerScript;
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        targetPosition = playerScript.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) > .1f) {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        } else {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            playerScript.TakeDamage(damage);
            DestroyProjectile();
        }
    }

    private void DestroyProjectile() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private bool isRunning = false;
    private bool facingRight = false;

    // Update is called once per frame
    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= sight) {
            if (player.position.x > transform.position.x && facingRight == false) {
                facingRight = true;
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            } else if (player.position.x < transform.position.x && facingRight == true) {
                facingRight = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (isRunning == false) {
                isRunning = true;
                audio.Loop("tree steps");
                anim.SetBool("isRunning", true);
            }
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        } else {
            if (isRunning == true) {
                isRunning = false;
                audio.Stop("tree steps");
                anim.SetBool("isRunning", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}

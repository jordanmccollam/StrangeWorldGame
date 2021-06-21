using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float stopDistance;
    public Transform shotPoint;
    public GameObject projectile;

    private bool facingRight = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        audio.Loop("fly");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            audio.Stop("fly");
        }

        if (player != null) {
            if (player.position.x > transform.position.x && facingRight == false) {
                facingRight = true;
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            } else if (player.position.x < transform.position.x && facingRight == true) {
                facingRight = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Vector2.Distance(transform.position, player.position) > stopDistance) {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if (Time.time >= attackTime) {
                attackTime = Time.time + timeBetweenAttacks;
                anim.SetTrigger("shoot");
                Shoot();
            }
        }
    }

    public void Shoot() {
        Vector2 direction = player.position - shotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x > 0 ? direction.x : -direction.x) * Mathf.Rad2Deg;
        shotPoint.rotation = Quaternion.Euler(0, direction.x > 0 ? 0 : 180f, angle);
        
        Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        audio.Play("spit");
    }
}

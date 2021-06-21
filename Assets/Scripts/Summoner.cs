using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{
    public float minDis;
    public float maxDis;
    public float range;

    [Header("Prefabs")]
    public Enemy enemyToSummon;
    public ParticleSystem particles;

    private bool isRunning = false;
    private bool facingRight = false;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            if (player.position.x > transform.position.x && facingRight == false) {
                facingRight = true;
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            } else if (player.position.x < transform.position.x && facingRight == true) {
                facingRight = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }


            if (Vector2.Distance(transform.position, player.position) > maxDis && Vector2.Distance(transform.position, player.position) <= sight) {

                // Chase!
                Run();
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                
            }
            else {
                if (Vector2.Distance(transform.position, player.position) <= sight) {
                    // Summon!
                    StopRunning();
                    transform.position = this.transform.position;
                    if (Time.time >= attackTime) {
                        attackTime = Time.time + timeBetweenAttacks;
                        anim.SetTrigger("summon");
                    }
                }
            }
        }
    }

    public void Summon() {
        if (player != null) {
            Vector2 randomPos = new Vector2(Random.Range(transform.position.x - range, transform.position.x + range), Random.Range(transform.position.y - range, transform.position.y + range));
            Instantiate(enemyToSummon, randomPos, transform.rotation);
            particles.Play();
            audio.Play("spawn");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void Run() {
        if (isRunning == false) {
            isRunning = true;
            audio.Loop("tree steps");
            anim.SetBool("isRunning", true);
        }
    }
    private void StopRunning() {
        if (isRunning == true) {
            isRunning = false;
            audio.Stop("tree steps");
            anim.SetBool("isRunning", false);
        }
    }


}

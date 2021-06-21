using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;
    public bool isMelee;
    public float range;
    public int damage;
    public float knockback;
    public float knocktime;
    public GameObject explosion;
    public LayerMask enemyLayer;
    public GameObject capsule;

    private float shotTime;
    private Player player;
    private bool swinging = false;
    private Animator camera;
    private AudioManager audio;
    private bool canLaunchCapsule = true;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Animator>();
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StateCtrl.isPaused == false) {
            if (swinging == false) {
                // Rotation
                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x > 0 ? direction.x : -direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, direction.x > 0 ? 0 : 180f, angle);
            }

            if (Input.GetMouseButtonDown(1)) {
                LaunchCapsule();
            }

            // On shoot
            if (Input.GetMouseButton(0)) {
                if (Time.time >= shotTime) {
                    camera.SetTrigger("shake");
                    shotTime = Time.time + timeBetweenShots;
                    player.Attack(isMelee);

                    if (isMelee == true) {
                        swinging = true;
                        StartCoroutine(SwingCooldown());
                    } else {
                        Instantiate(projectile, shotPoint.position, transform.rotation);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (swinging == true) {
            // Vector2 difference = other.transform.position - transform.position;
            // difference = difference.normalized * weapon.KNOCKBACK;
            // difference = difference.normalized;

            if (other.tag == "Enemy") {
                Vector2 difference = other.transform.position - transform.position;
                difference = difference.normalized * knockback;
                other.GetComponent<Enemy>().TakeDamage(damage, difference, knocktime);
            }
            
            // if (other.tag == "Boss") {
            //     other.GetComponent<Boss>().TakeDamage(damage);
            // }

            if (other.tag == "Rock") {
                other.GetComponent<BreakableRock>().TakeDamage(damage);
                Instantiate(explosion, other.transform.position, other.transform.rotation);
            }
        }
    }

    private void LaunchCapsule() {
        if (canLaunchCapsule == true) {
            canLaunchCapsule = false;
            audio.Play("throw");
            Instantiate(capsule, transform.position, transform.rotation);
            StartCoroutine(CapsuleCooldown());
        }
    }

    private IEnumerator CapsuleCooldown() {
        yield return new WaitForSeconds(1f);
        canLaunchCapsule = true;
    }

    private IEnumerator SwingCooldown() {
        yield return new WaitForSeconds(.1f);
        swinging = false;
    }

    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(shotPoint.position, range);
    // }
}

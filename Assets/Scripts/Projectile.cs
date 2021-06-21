using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifetime;
    public float knockback;
    public float knocktime;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void DestroyProjectile() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Vector2 difference = other.transform.position - transform.position;
            difference = difference.normalized * knockback;
            other.GetComponent<Enemy>().TakeDamage(damage, difference, knocktime);
            DestroyProjectile();
        }
        
        // if (other.tag == "Boss") {
        //     other.GetComponent<Boss>().TakeDamage(damage);
        //     DestroyProjectile();
        // }

        if (other.tag == "Rock") {
            other.GetComponent<BreakableRock>().TakeDamage(damage);
            DestroyProjectile();
        }
    }
}

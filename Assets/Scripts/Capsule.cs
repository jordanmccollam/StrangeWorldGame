using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public int fillTime;
    public float speed;
    public float range;
    public float waitTime;
    
    private AudioManager audio;
    private bool isMoving = true;
    private int currentTime;
    private bool filling = false;    
    private bool canFill = true;
    private GameObject magic;
    private bool canPickup = false;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        StartCoroutine(StopMovement());
    }

    private void Update() {
        if (isMoving == true) {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        if (filling == true && canFill == true) {
            canFill = false;
            currentTime += 1;
            StartCoroutine(Fill());
        }

        if (currentTime >= fillTime && magic != null) {
            // FULL
            canFill = false;
            canPickup = true;
            GetComponent<SpriteRenderer>().color = Color.green;
            Destroy(magic);
        }
    }

    private IEnumerator StopMovement() {
        yield return new WaitForSeconds(range);
        transform.position = this.transform.position;
        isMoving = false;
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(3f);
        if (filling == false) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && canPickup == true) {
            // Pickup
            audio.Play("pickup");
            Inventory.greenMagic += 1;
            Destroy(gameObject);
        }

        if (other.tag == "Magic") {
            if (filling == false) {
                // Start filling...
                filling = true;
                other.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("fill");
                anim.SetTrigger("fill");
                magic = other.gameObject;

            }
        }
    }

    private IEnumerator Fill() {
        yield return new WaitForSeconds(waitTime);
        canFill = true;
    }
}

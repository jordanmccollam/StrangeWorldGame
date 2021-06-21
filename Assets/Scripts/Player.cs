using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public int health;
    public float jumpCooldown;
    public float jumpSpeed;
    public float immunityTime;

    [Header("Prefabs")]
    public GameObject weapon;
    public GameObject blood;
    public ParticleSystem dust;
    

    [Header("Other")]
    public Transform weaponPos;
    public Transform shadow;

    [Header("UI")]
    public TextMeshProUGUI dayUI;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI oreUI;
    public TextMeshProUGUI greenMagic;
    public Image weaponSprite;

    private Rigidbody2D rb;
    private Vector2 moveAmount;
    private Animator anim;
    private int fullHealth;
    private bool facingRight = true;
    private bool canJump = true;
    private AudioManager audio;
    private bool isWalking = false;
    private Animator camera;
    private bool canCreateDust = true;
    private bool canTakeDamage = true;
    private Weapon weaponComp;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Animator>();
        fullHealth = health;
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        weaponComp = weapon.GetComponent<Weapon>();
        UpdateHealthUI(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (StateCtrl.isPaused == false) {
            HandleMove();
            HandleAim();
        }

        dayUI.text = "Day " + StateCtrl.level.ToString();
        oreUI.text = Inventory.ore.ToString();
        greenMagic.text = Inventory.greenMagic.ToString();
    }

    private void HandleAim() {
        Vector2 aimDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (aimDir.x > 0 && facingRight == false) {
            shadow.rotation = Quaternion.Euler(0, 180f, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
        } else if (aimDir.x < 0 && facingRight == true) {
            shadow.rotation = Quaternion.Euler(0, 180f, 0);
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            facingRight = false;
        }
    }

    private void HandleMove() {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (canCreateDust == true && moveInput != Vector2.zero) {
            CreateDust();
        }

        if (moveInput != Vector2.zero && isWalking == false) {
            anim.SetBool("isWalking", true);
            audio.Loop("walking");
            isWalking = true;
        } else if (moveInput == Vector2.zero && isWalking == true) {
            anim.SetBool("isWalking", false);
            audio.Stop("walking");
            isWalking = false;
        }

        if (Input.GetKeyDown("space") && moveInput != Vector2.zero) {
            Jump();
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }

    public void ChangeWeapon(Weapon weaponToEquip) {
        Destroy(weapon);
        GameObject newWeapon = Instantiate(weaponToEquip.gameObject, weaponPos.position, weaponPos.rotation, weaponPos.parent);
        weapon = newWeapon;
        weaponComp = newWeapon.GetComponent<Weapon>();
        SpriteRenderer newWeaponSprite = weaponToEquip.GetComponent<SpriteRenderer>();
        weaponSprite.sprite = newWeaponSprite.sprite;
        weaponSprite.color = newWeaponSprite.color;
    }

    private void Jump() {
        if (canJump == true) {
            canJump = false;
            audio.Stop("walking");
            isWalking = false;
            gameObject.layer = 8;
            anim.SetTrigger("jump");
            audio.Play("dash");
            speed *= 2;
            StartCoroutine(JumpCooldown());
            StartCoroutine(JumpSpeed());
        }
    }

    private IEnumerator JumpCooldown() {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
    private IEnumerator JumpSpeed() {
        yield return new WaitForSeconds(jumpSpeed);
        gameObject.layer = 0;
        speed /= 2;
    }

    public void Attack(bool isMelee) {
        if (isMelee) {
            anim.SetTrigger("swing");
            audio.Play("swing");
        } else {
            anim.SetTrigger("shoot");
            audio.Play("shoot");
        }
    }

    private void UpdateHealthUI(int currentHealth) {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void TakeDamage(int damageAmount) {
        if (canTakeDamage) {
            canTakeDamage = false;
            health -= damageAmount;
            Instantiate(blood, transform.position, transform.rotation);
            UpdateHealthUI(health);
            audio.Play("player hurt");
            camera.SetTrigger("shake");

            if (health <= 0) {
                Die();
            } else {
                StartCoroutine(Immunity());
            }
        }
    }

    private IEnumerator Immunity() {
        yield return new WaitForSeconds(immunityTime);
        canTakeDamage = true;
    }

    private void Die() {
        Destroy(gameObject);
        // Init game over screen
    }

    private void CreateDust() {
        canCreateDust = false;
        StartCoroutine(SettleDust());
        dust.Play();
    }

    private IEnumerator SettleDust() {
        yield return new WaitForSeconds(1f);
        canCreateDust = true;
    }
}

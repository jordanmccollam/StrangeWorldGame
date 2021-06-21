using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tent : MonoBehaviour
{
    public Resource[] resources;
    public GameObject screen;
    public GameObject preparedScreen;
    public GameObject defaultScreen;
    public Sprite highlightedSprite;
    public Sprite defaultSprite;
    public TextMeshProUGUI resourceAmountUI;
    public int resourceAmount;
    public Image resourceSprite;

    private bool canToggle = false;
    private bool opened = false;
    private SpriteRenderer sr;
    private Resource resource;
    private bool canAdvance = false;
    private Animator camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        ShuffleResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (canToggle == true && Input.GetKeyDown(KeyCode.Tab)) {
            if (opened == true) {
                opened = false;
                screen.SetActive(false);
                Time.timeScale = 1f;
                StateCtrl.isPaused = false;
            } else {
                CheckResources();
                opened = true;
                screen.SetActive(true);
                Time.timeScale = 0f;
                StateCtrl.isPaused = true;
            }
        }
    }

    private void CheckResources() {
        switch(resource.name) {
            case "ore":
                if (Inventory.ore >= resourceAmount) {
                    canAdvance = true;
                    defaultScreen.SetActive(false);
                    preparedScreen.SetActive(true);
                } else {
                    canAdvance = false;
                    preparedScreen.SetActive(false);
                    defaultScreen.SetActive(true);
                }
                break;
            case "greenMagic":
                if (Inventory.greenMagic >= resourceAmount) {
                    canAdvance = true;
                    defaultScreen.SetActive(false);
                    preparedScreen.SetActive(true);
                } else {
                    canAdvance = false;
                    preparedScreen.SetActive(false);
                    defaultScreen.SetActive(true);
                }
                break;
            default:
            break;
        }
    }

    public void Advance() {
        if (canAdvance == true) {
            StateCtrl.level += 1;
            switch(resource.name) {
                case "ore":
                    Inventory.ore -= resourceAmount;
                    break;
                case "greenMagic":
                    Inventory.greenMagic -= resourceAmount;
                    break;
                default:
                break;
            }

            // Play advance sound here
            camera.SetTrigger("shake");

            ShuffleResources();
            CheckResources();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canToggle = true;
            sr.sprite = highlightedSprite;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canToggle = false;
            sr.sprite = defaultSprite;
        }
    }

    public void ShuffleResources() {
        resource = resources[Random.Range(0, resources.Length)];
        resourceSprite.sprite = resource.sprite;
        resourceSprite.color = resource.spriteColor;

        int randInt = Random.Range(3, (3 + StateCtrl.level));
        resourceAmountUI.text = randInt.ToString();
        resourceAmount = randInt;
    }


    [System.Serializable]
    public class Resource
    {
        public string name;

        public Sprite sprite;
        public Color spriteColor;
    }
}

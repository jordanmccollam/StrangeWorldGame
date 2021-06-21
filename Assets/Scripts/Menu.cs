using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Interactions")]
    public static int oreAdded;
    public static int greenMagicAdded;
    

    [Header("Prefabs")]
    public GameObject menu_ui;
    public Image[] oreSlots;
    public Image[] magicSlots;
    public Image resultSlot;
    public Color greenColor;
    public Color fullColor;
    public Color emptyColor;
    public CraftingRecipe[] recipes;

    private bool toggled = false;
    private bool canCraft = false;
    private CraftingRecipe currentRecipe;
    private AudioManager audio;

    private void Start() {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Toggle();
        }
    }

    public void Toggle() {
        if (toggled == true) {
            // CLOSE
            menu_ui.SetActive(false);
            Time.timeScale = 1f;
            toggled = false;
            StateCtrl.isPaused = false;
        } else {
            // OPEN
            menu_ui.SetActive(true);
            Time.timeScale = 0;
            toggled = true;
            StateCtrl.isPaused = true;
        }
    }

    public void AddOre() {
        if (Inventory.ore  > 0) {
            oreAdded += 1;
            Inventory.ore -= 1;
            UpdateCraftingUI();
        }
    }
    public void AddGreenMagic() {
        if (Inventory.greenMagic  > 0) {
            greenMagicAdded += 1;
            Inventory.greenMagic -= 1;
            UpdateCraftingUI();
        }
    }
    public void RemoveOre() {
        if (oreAdded > 0) {
            oreAdded -= 1;
            Inventory.ore += 1;
            UpdateCraftingUI();
        }
    }
    public void RemoveGreenMagic() {
        if (greenMagicAdded > 0) {
            greenMagicAdded -= 1;
            Inventory.greenMagic += 1;
            UpdateCraftingUI();
        }
    }

    private void UpdateCraftingUI() {
        for (int i = 0; i < oreSlots.Length; i++)
        {
            if (i < oreAdded) {
                oreSlots[i].color = fullColor;
            } else {
                oreSlots[i].color = emptyColor;
            }

            if (i < greenMagicAdded) {
                magicSlots[i].color = greenColor;
            } else {
                magicSlots[i].color = emptyColor;
            }
        }

        CheckRecipes();
    }

    private void CheckRecipes() {
        for (int i = 0; i < recipes.Length; i++)
        {
            if (oreAdded == recipes[i].oreCost && greenMagicAdded == recipes[i].greenMagicCost) {
                SpriteRenderer sr = recipes[i].result.GetComponent<SpriteRenderer>();
                resultSlot.sprite = sr.sprite;
                resultSlot.color = sr.color;
                currentRecipe = recipes[i];
                return;
            } else {
                resultSlot.color = emptyColor;
                currentRecipe = null;
            }
        }
    }

    public void OnCraft() {
        if (currentRecipe != null) {
            currentRecipe.Craft(audio);
            UpdateCraftingUI();
        }
    }
}

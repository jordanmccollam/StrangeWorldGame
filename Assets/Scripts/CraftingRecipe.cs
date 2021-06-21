using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    [Range(1, 999)]
    public int greenMagicCost;
    public int oreCost;
    public GameObject result;

    private Player player;
    private Transform frontArm;

    private bool canCraft() {
        if (Menu.oreAdded >= oreCost) {
            return true;
        }
        return false;
    }

    public void Craft(AudioManager audio) {
        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            player = playerObj.GetComponent<Player>();
            frontArm = playerObj.transform.Find("Front Arm");
        }

        // Clicked on weapon sprite
        if (canCraft() == true) {
            Menu.oreAdded -= oreCost;
            audio.Play("craft");
            player.ChangeWeapon(result.GetComponent<Weapon>());
        }
    }
}

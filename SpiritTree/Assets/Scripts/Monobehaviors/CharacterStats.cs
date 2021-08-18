using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterStats_SO charDefinition;

    // create a new variable to store the characters stats from the character_SO file
    // doing this to prevent modification of the scriptable objects created in unity editor.
    [HideInInspector] public CharacterStats_SO charStats;

    [SerializeField] private CharacterStats playerStats;

    public void Start()
    {
        charStats = new CharacterStats_SO();
        charStats.Copy(charDefinition);

        // These don't work for some reason
        //playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterCombat>().GetStats();
        //playerStats = GameObject.Find("Player").GetComponent<CharacterCombat>().GetStats();
    }

    public void TakeDamage(int amount)
    {
        //charDefinition.TakeDamage(amount);
        charStats.TakeDamage(amount);

        if (charStats.health <= 0)
        {
            if (playerStats != null)
            {
                // Add 10 experience to the player when enemy dies.
                playerStats.charStats.AddExperience(10);
            }

            Destroy(gameObject);
        }
    }

    public void EquipWeapon(Weapon_SO weapon)
    {
        charStats.weapon = weapon;

        Debug.Log("Equipped: " + charStats.weapon.itemName);
    }
}

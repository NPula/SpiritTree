using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats/New Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    // REMEBER THAT ANY NEW MEMBER VARIABLES SHOULD BE ADDED TO COPY FUNCTION AT THE BOTTOM OF THIS CLASS.

    public int health = 0;
    public int maxHealth = 0;
   
    public int mana = 0;
    public int maxMana = 0;

    public int defence = 0;
    public int maxDefence = 0;

    public int wealth = 0;
    public int maxWealth = 0;

    public int damage = 0; // damage to inflict. Change so damage is determined by level + Weapons stats + type of attack.

    public int exp;
    public int maxExp;

    public int level = 0;

    public Weapon_SO weapon;

    public void ApplyHealth(int amount)
    {
        // Add health to player. Keep player's health <= maxHealth
        health = health + amount <= maxHealth ? health + amount : maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Death();
        }
    }

    public void AddExperience(int amount)
    {
        if (exp + amount >= maxExp)
        {
            LevelUp();
            exp = (exp + amount) - maxExp; // add spill over points.
        }
        else
        {
            exp += amount;
        }
    }

    public void LevelUp()
    {
        level++;
    }

    public void Death()
    {
        // Kill player
    }

    // Copies values of another CharacterStats obj using deep copy.
    public void Copy(CharacterStats_SO obj)
    {
        this.health = obj.health;
        this.maxHealth = obj.maxHealth;
        this.mana = obj.mana;
        this.maxMana = obj.maxMana;
        this.defence = obj.defence;
        this.maxDefence = obj.maxDefence;
        this.wealth = obj.wealth;
        this.maxWealth = obj.maxWealth;
        this.damage = obj.damage;
        this.exp = obj.exp;
        this.maxExp = obj.maxExp;
        this.level = obj.level;
        this.weapon = obj.weapon;
    }
}

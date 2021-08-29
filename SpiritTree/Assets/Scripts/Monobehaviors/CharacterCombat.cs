using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    struct AttackInfo
    {
        // the weapon that was used to attack.
        public GameObject weapon;

        // the amount of time since the attack was instantiated.
        public float timeSinceAttack;
    }

    private CharacterStats m_stats;
    [SerializeField] private LayerMask m_enemyLayerMask;
    [SerializeField] private GameObject m_weapon;
    [SerializeField] private float m_attackRange = 1;
    private List<AttackInfo> m_weaponInfo; // Holds list of instantiated weapons.

    private void Start()
    {
        m_stats = GetComponent<CharacterStats>();
        m_weaponInfo = new List<AttackInfo>();
    }

    private void Update()
    {
        Debug.Log(" Player EXP: " + m_stats.charStats.exp);
        Debug.Log(" Player LEVEL: " + m_stats.charStats.level);

        // Destroys temporary weapon after some amount of seconds have passed.
        for ( int i = 0; i < m_weaponInfo.Count; i++)
        {
            AttackInfo info = m_weaponInfo[i];
            info.timeSinceAttack += Time.deltaTime;

            if (info.timeSinceAttack >= .1f)
            {
                Destroy(info.weapon);
                m_weaponInfo.Remove(m_weaponInfo[i]);
            }
            else
            {
                m_weaponInfo[i] = info;
            }
        }

        
    }

    public void Attack()
    {
        // Play attack animation

        // store all enemies hit
        Vector3 offset = Vector3.right * (1f * transform.localScale.x);
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position + offset, m_attackRange, m_enemyLayerMask);

        foreach(Collider2D e in enemiesHit)
        {
            //Debug.Log(e.transform.name + ": " + e.transform.position);
            //Debug.Log("Creating tmp Weapon.");

            // Temporary attack animation
            GameObject weaponInstance = Instantiate(m_weapon, transform.position, Quaternion.identity);

            e.transform.GetComponent<CharacterStats>().TakeDamage(m_stats.charStats.damage);

            // Temporary attack stuff
            AttackInfo info;
            info.weapon = weaponInstance;
            info.timeSinceAttack = 0;

            info.weapon.transform.position = e.transform.position;

            m_weaponInfo.Add(info);
        }
    }

    public CharacterStats GetStats()
    {
        return m_stats;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 offset = Vector3.right * (1f * transform.localScale.x);
        Gizmos.DrawWireSphere(transform.position + offset, m_attackRange);
    }
}

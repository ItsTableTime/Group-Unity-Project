using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public float EnemySpeed;
    float Facing = 1;
    float TurnCooldown = 0;
    bool EnemyAi = true;
    public float EnemyRange = 0;
    public float AggressiveRange = 0;
    public float EnemySight;
    public GameObject Player;
    public float EnemyHealth;
    public int JumpRate;
    float KnockbackTime;
    int JumpChance;
    public float JumpCooldown;
    public float KnockbackResistance;
    Rigidbody2D EnemyRigidBody;
    public GateScript LinkedGate;

    void Start()
    {
        EnemyRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (EnemyHealth <= 0)
        {
            LinkedGate.EnemyCount--;
            Destroy(gameObject);
        }
        transform.position = transform.position + new Vector3(Facing * EnemySpeed * Time.deltaTime, 0, 0);
        TurnCooldown -= 1 * Time.deltaTime;
        EnemySight = Vector2.Distance(transform.position, Player.transform.position);
        if (JumpCooldown <= 0)
        {
            if (JumpRate > 0)
            {
                JumpChance = RandomNumberGenerator.GetInt32(1, JumpRate);
                if (JumpChance == 1)
                {
                    EnemyRigidBody.AddForce(transform.up * 350 / KnockbackResistance, ForceMode2D.Impulse);
                    JumpCooldown = 3;
                }
            }
        }
        else
        {
            JumpCooldown -= 1 * Time.deltaTime;
        }
            if (KnockbackTime > 0)
        {
            transform.position = transform.position + new Vector3(-10 * Facing / KnockbackResistance * Time.deltaTime, 0, 0);
            KnockbackTime -= 1 * Time.deltaTime;
        }
        if ((EnemyAi == true) && (EnemySight < EnemyRange))
        {
            if (transform.position.x > Player.transform.position.x)
            {
                Facing = -1;
            }
            else
            {
                Facing = 1;
            }
            EnemyRange = AggressiveRange;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (TurnCooldown <= 0)
            {
                if (Facing == 1)
                {
                    Facing = -1;
                    TurnCooldown = 1;
                }
                else
                {
                    Facing = 1;
                    TurnCooldown = 1;
                }

            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (TurnCooldown <= 0)
            {
                if (Facing == 1)
                {
                    Facing = -1;
                    TurnCooldown = 1;
                }
                else
                {
                    Facing = 1;
                    TurnCooldown = 1;
                }
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BasicAttack")
        {
            EnemyHealth -= 20;
            KnockbackTime = 0.1f;
            Destroy(other.gameObject);
        }
        if (other.tag == "SpikeAttack")
        {
            EnemyHealth -= 40;
            EnemyRigidBody.AddForce(transform.up * 500 / KnockbackResistance, ForceMode2D.Impulse);
        }
        if (other.tag == "SlamAttack")
        {
            EnemyHealth -= 60;
            EnemyRigidBody.AddForce(transform.up * 500 / KnockbackResistance, ForceMode2D.Impulse);
            KnockbackTime = 0.5f;
        }
        if (other.tag == "Wall")
        {
            if (TurnCooldown <= 0)
            {
                if (Facing == 1)
                {
                    Facing = -1;
                    TurnCooldown = 1;
                }
                else
                {
                    Facing = 1;
                    TurnCooldown = 1;
                }

            }
        }
    }
}

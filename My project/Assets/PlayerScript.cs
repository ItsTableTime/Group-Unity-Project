using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Windows;

public class PlayerScript : MonoBehaviour
{
    float HorizontalMovement;
    Rigidbody2D PlayerRigidbody;
    bool AllowJump = true;
    public PlayerInput input;
    public float Speed;
    public float JumpHeight;
    public float Health;
    public float MaxHealth;
    public float Immunity;
    public float PlayerDirection;
    public GameObject AttackProjectile;
    GameObject SummonedAttack;
    public GameObject ItemDrop;
    public string SpellSlot1;
    public string SpellSlot2;
    public string SpellSlot3;
    public float GlobalCooldown;
    Vector2 PlayerTransform;
    bool CanPickup = false;
    float PickupCooldown = 0;
    CollectableScript CollectableInfo;
    GameObject CollectableObject;
    GameObject SummonedCollectable;

    public float DashLength;
    public float DashSpeed;
    public float DashCooldown;
    float DashTime;

    bool AllowBonusJump;
    public float BonusJumpHeight;

    public float WarpLength;
    public float WarpCooldown;
    float WarpAmount;

    public GameObject SpikeAttackProjectile;
    GameObject SummonedSpikeAttack;
    public float SpikeCooldown;

    public GameObject WaterGunAttackProjectile;
    GameObject SummonedWaterGunAttack;
    public float WaterGunCooldown;

    public GameObject SlamAttackProjectile;
    GameObject SummonedSlamAttack;
    public float SlamCooldown;
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        PlayerTransform = transform.position;
        Vector2 temp = PlayerRigidbody.linearVelocity;
        temp.x = (HorizontalMovement * Speed * (1+DashSpeed*DashTime));
        PlayerRigidbody.linearVelocity = (temp.x * transform.right) + (temp.y * transform.up);
        Immunity -= 1 * Time.deltaTime;
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (transform.position.y < -30)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (MaxHealth < Health)
        {
            Health = MaxHealth;
        }
        if (DashTime > 0)
        {
            DashTime -= 1 * Time.deltaTime;
        }
        if ((DashTime < 1) & (DashCooldown > 0))
        {
            DashTime = 0;
            DashCooldown -= 1 * Time.deltaTime;
        }
        if (GlobalCooldown > 0)
        {
            GlobalCooldown -= 1 * Time.deltaTime;
        }
        if (PickupCooldown > 0)
        {
            PickupCooldown -= 1 * Time.deltaTime;
        }
        if (WarpCooldown > 0)
        {
            WarpCooldown -= 1 * Time.deltaTime;
        }
        if (SpikeCooldown > 0)
        {
            SpikeCooldown -= 1 * Time.deltaTime;
        }
        if (WaterGunCooldown > 0)
        {
            WaterGunCooldown -= 1 * Time.deltaTime;
        }
        if (SlamCooldown > 0)
        {
            SlamCooldown -= 1 * Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();

        HorizontalMovement = InputAxis.x;
        if (InputAxis.x != 0)
        {
            PlayerDirection = InputAxis.x;
            WarpAmount = InputAxis.x * WarpLength;
        }
    }
    public void Jump()
    {
        if ((AllowJump == true) & (GlobalCooldown <= 0))
        {
            PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
            AllowJump = false;
            GlobalCooldown = 0.2f;
        }
    }
    public void Attack()
    {
        if (GlobalCooldown <= 0)
        {
            SummonedAttack = Instantiate(AttackProjectile, transform.position, transform.rotation);
            SummonedAttack.GetComponent<AttackMovementScript>().AttackDirection = PlayerDirection;
            GlobalCooldown = 0.2f;

        }

    }
    public void Pickup()
    {
        if ((CanPickup == true) & (PickupCooldown <= 0))
        {
            PickupCooldown = 0.2f;
            if (CollectableInfo.SpellSlot == 1)
            {
                if (SpellSlot1 != "Empty")
                {
                    SummonedCollectable = Instantiate(ItemDrop, transform.position, transform.rotation);
                    SummonedCollectable.GetComponent<CollectableScript>().SpellSlot = 1;
                    SummonedCollectable.GetComponent<CollectableScript>().SpellName = SpellSlot1;
                }
                SpellSlot1 = CollectableInfo.SpellName;
                Destroy(CollectableObject);
            }
            if (CollectableInfo.SpellSlot == 2)
            {
                if (SpellSlot2 != "Empty")
                {
                    SummonedCollectable = Instantiate(ItemDrop, transform.position, transform.rotation);
                    SummonedCollectable.GetComponent<CollectableScript>().SpellSlot = 2;
                    SummonedCollectable.GetComponent<CollectableScript>().SpellName = SpellSlot2;
                }
                SpellSlot2 = CollectableInfo.SpellName;
                Destroy(CollectableObject);
            }
            if (CollectableInfo.SpellSlot == 3)
            {
                if (SpellSlot3 != "Empty")
                {
                    SummonedCollectable = Instantiate(ItemDrop, transform.position, transform.rotation);
                    SummonedCollectable.GetComponent<CollectableScript>().SpellSlot = 3;
                    SummonedCollectable.GetComponent<CollectableScript>().SpellName = SpellSlot3;
                }
                SpellSlot3 = CollectableInfo.SpellName;
                Destroy(CollectableObject);
            }
            
        }
    }
    public void Spell1()
    {
        if ((SpikeCooldown <= 0) & (SpellSlot1 == "EarthSpike") & (GlobalCooldown <= 0))
        {
            SummonedSpikeAttack = Instantiate(SpikeAttackProjectile, (transform.position + new Vector3(1.5f * PlayerDirection, -1.5f)), transform.rotation);
            GlobalCooldown = 0.5f;
            SpikeCooldown = 5;

        }
        if ((WaterGunCooldown <= 0) & (SpellSlot1 == "WaterGun") & (GlobalCooldown <= 0))
        {
            SummonedWaterGunAttack = Instantiate(WaterGunAttackProjectile, transform.position, transform.rotation);
            SummonedWaterGunAttack.GetComponent<AttackMovementScript>().AttackDirection = PlayerDirection;
            GlobalCooldown = 0.5f;
            WaterGunCooldown = 3;
        }

    }
    public void Spell2()
    {
        if ((DashCooldown <= 0) & (SpellSlot2 == "Dash") & (GlobalCooldown <= 0))
        {
            DashTime = DashLength;
            DashCooldown = 5;
            GlobalCooldown = 0.2f;
        }
        if ((AllowBonusJump == true) & (SpellSlot2 == "BonusJump") & (GlobalCooldown <= 0))
        {
            PlayerRigidbody.AddForce(transform.up * BonusJumpHeight, ForceMode2D.Impulse);
            AllowBonusJump = false;
            GlobalCooldown = 0.2f;
        }
        if ((WarpCooldown <= 0) & (SpellSlot2 == "WarpDash") & (GlobalCooldown <= 0))
        {
            transform.position = PlayerTransform + new Vector2(WarpAmount, 0);
            WarpCooldown = 5;
            GlobalCooldown = 0.2f;
        }
    }
    public void Spell3()
    {
        if ((SlamCooldown <= 0) & (SpellSlot3 == "RockSlam") & (GlobalCooldown <= 0))
        {
            SummonedSlamAttack = Instantiate(SlamAttackProjectile, (transform.position + new Vector3(1.5f * PlayerDirection, -1f)), transform.rotation);
            GlobalCooldown = 0.5f;
            SlamCooldown = 15;

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            AllowJump = true;
            AllowBonusJump = true;
        }
        if (collision.gameObject.tag == "DamagePart")
        {
            if (Immunity <= 0)
            {
                Health -= 1;
                PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
                Immunity = 1;
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (Immunity <= 0)
            {
                Health -= 1;
                PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
                Immunity = 1;

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectable")
        {
                CanPickup = true;
                CollectableInfo = other.gameObject.GetComponent<CollectableScript>();
                CollectableObject = other.gameObject;

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
            CanPickup = false;
    }
}
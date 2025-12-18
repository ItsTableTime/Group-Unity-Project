using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public bool BeatLevel = false;
    public float GlobalCooldown;
    public Image GlobalBar;
    Vector2 PlayerTransform;
    bool CanPickup = false;
    float PickupCooldown = 0;
    CollectableScript CollectableInfo;
    GameObject CollectableObject;
    GameObject SummonedCollectable;
    bool TouchingWall = false;
    bool TouchingGround = true;
    public Image HealthBar;
    public Image DashBar;
    public float DashLength;
    public float DashSpeed;
    public float DashCooldown;
    float DashTime;
    bool AllowBonusJump;
    public float BonusJumpHeight;
    public Image WarpDashBar;
    public float WarpLength;
    public float WarpCooldown;
    float WarpAmount;
    public Image EarthSpikeBar;
    public GameObject SpikeAttackProjectile;
    GameObject SummonedSpikeAttack;
    public float SpikeCooldown;
    public Image WaterGunBar;
    public GameObject WaterGunAttackProjectile;
    GameObject SummonedWaterGunAttack;
    public float WaterGunCooldown;
    public Image RockSlamBar;
    public GameObject SlamAttackProjectile;
    GameObject SummonedSlamAttack;
    public float SlamCooldown;
    public Image GaleBar;
    public GameObject GaleProjectile;
    GameObject GaleAttack;
    public float GaleCooldown;
    public Image FirePunchBar;
    public GameObject FirePunchAttackProjectile;
    GameObject SummonedFirePunchAttack;
    public float FirePunchCooldown;
    public Image MendBar;
    public float MendCooldown;
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        PlayerTransform = transform.position;
        Vector2 temp = PlayerRigidbody.linearVelocity;
        temp.x = (HorizontalMovement * Speed * (1 + DashSpeed * DashTime));
        PlayerRigidbody.linearVelocity = (temp.x * transform.right) + (temp.y * transform.up);
        Immunity -= 1 * Time.deltaTime;
        HealthBar.fillAmount = ((float)Health / (float)MaxHealth);
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
            DashBar.fillAmount = (0.33f * (float)DashCooldown);
        }
        else
        {
            DashBar.fillAmount = 0;
        }
        if (GlobalCooldown > 0)
        {
            GlobalCooldown -= 1 * Time.deltaTime;
            GlobalBar.fillAmount = 2.5f * (float)GlobalCooldown;
        }
        else
        {
            GlobalBar.fillAmount = 0;
        }
        if (PickupCooldown > 0)
        {
            PickupCooldown -= 1 * Time.deltaTime;
        }
        if (WarpCooldown > 0)
        {
            WarpCooldown -= 1 * Time.deltaTime;
            WarpDashBar.fillAmount = (0.2f * (float)WarpCooldown);
        }
        else
        {
            WarpDashBar.fillAmount = 0;
        }
        if (SpikeCooldown > 0)
        {
            SpikeCooldown -= 1 * Time.deltaTime;
            EarthSpikeBar.fillAmount = (0.2f * (float)SpikeCooldown);
        }
        else
        {
            EarthSpikeBar.fillAmount = 0;
        }
        if (WaterGunCooldown > 0)
        {
            WaterGunCooldown -= 1 * Time.deltaTime;
            WaterGunBar.fillAmount = (0.33f * (float)WaterGunCooldown);
        }
        else
        {
            WaterGunBar.fillAmount = 0;
        }
        if (SlamCooldown > 0)
        {
            SlamCooldown -= 1 * Time.deltaTime;
            RockSlamBar.fillAmount = (0.06f * (float)SlamCooldown);
        }
        else
        {
            RockSlamBar.fillAmount = 0;
        }
        if (GaleCooldown > 0)
        {
            GaleCooldown -= 1 * Time.deltaTime;
            GaleBar.fillAmount = (0.1f * (float)GaleCooldown);
        }
        else
        {
            GaleBar.fillAmount = 0;
        }
        if (FirePunchCooldown > 0)
        {
            FirePunchCooldown -= 1 * Time.deltaTime;
            FirePunchBar.fillAmount = (1f * (float)FirePunchCooldown);
        }
        else
        {
            FirePunchBar.fillAmount = 0;
        }
        if (MendCooldown > 0)
        {
            MendCooldown -= 1 * Time.deltaTime;
            MendBar.fillAmount = (0.008f * (float)MendCooldown);
        }
        else
        {
            MendBar.fillAmount = 0;
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
            GlobalCooldown = 0.4f;
        }
        else if ((TouchingGround == true) & (GlobalCooldown <= 0))
        {
                PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
                AllowJump = false;
                GlobalCooldown = 0.4f;

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
            GlobalCooldown = 0.2f;
            SpikeCooldown = 5;


        }
        if ((WaterGunCooldown <= 0) & (SpellSlot1 == "WaterGun") & (GlobalCooldown <= 0))
        {
            SummonedWaterGunAttack = Instantiate(WaterGunAttackProjectile, transform.position, transform.rotation);
            SummonedWaterGunAttack.GetComponent<AttackMovementScript>().AttackDirection = PlayerDirection;
            GlobalCooldown = 0.2f;
            WaterGunCooldown = 3;
        }
        if ((FirePunchCooldown <= 0) & (SpellSlot1 == "FirePunch") & (GlobalCooldown <= 0))
        {
            SummonedFirePunchAttack = Instantiate(FirePunchAttackProjectile, transform.position, transform.rotation);
            SummonedFirePunchAttack.GetComponent<AttackMovementScript>().AttackDirection = PlayerDirection;
            GlobalCooldown = 0.2f;
            FirePunchCooldown = 1;
        }
    }
    public void Spell2()
    {
        if ((DashCooldown <= 0) & (SpellSlot2 == "Dash") & (GlobalCooldown <= 0))
        {
            DashTime = DashLength;
            DashCooldown = 3;
            GlobalCooldown = 0.4f;
        }
        if ((AllowBonusJump == true) & (SpellSlot2 == "BonusJump") & (GlobalCooldown <= 0))
        {
            PlayerRigidbody.AddForce(transform.up * BonusJumpHeight, ForceMode2D.Impulse);
            AllowBonusJump = false;
            GlobalCooldown = 0.4f;
        }
        else if ((TouchingGround == true) & (SpellSlot2 == "BonusJump") & (GlobalCooldown <= 0))
        {
            PlayerRigidbody.AddForce(transform.up * BonusJumpHeight, ForceMode2D.Impulse);
            AllowBonusJump = false;
            GlobalCooldown = 0.4f;
        }
        if ((WarpCooldown <= 0) & (SpellSlot2 == "WarpDash") & (GlobalCooldown <= 0))
        {
            transform.position = PlayerTransform + new Vector2(WarpAmount, 0);
            WarpCooldown = 5;
            GlobalCooldown = 0.4f;
        }
    }
    public void Spell3()
    {
        if ((SlamCooldown <= 0) & (SpellSlot3 == "RockSlam") & (GlobalCooldown <= 0))
        {
            SummonedSlamAttack = Instantiate(SlamAttackProjectile, (transform.position + new Vector3(1.5f * PlayerDirection, -1f)), transform.rotation);
            GlobalCooldown = 0.2f;
            SlamCooldown = 15;
        }
        if ((GaleCooldown <= 0) & (SpellSlot3 == "Gale") & (GlobalCooldown <= 0))
        {
            GaleAttack = Instantiate(GaleProjectile, (transform.position + new Vector3(0, 0)), transform.rotation);
            GlobalCooldown = 0.2f;
            GaleCooldown = 10;
        }
        if ((MendCooldown <= 0) & (SpellSlot3 == "Mend") & (GlobalCooldown <= 0))
        {
            if (Health + 1 < MaxHealth)
            {
                Health += 1;
                MaxHealth -= 1;
                MendCooldown = 120;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (GlobalCooldown <= 0)
            {
                AllowJump = true;
                AllowBonusJump = true;
            }
            TouchingGround = true;
        }
        if (collision.gameObject.tag == "DamagePart")
        {
            if (Immunity <= 0)
            {
                Health -= 1;
                PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
                Immunity = 1.5f;
            }
        }
        if (collision.gameObject.tag == "BoostPad")
        {
            if (Immunity <= 0)
            {
                PlayerRigidbody.AddForce(transform.up * 3 * JumpHeight, ForceMode2D.Impulse);
                Immunity = 1.5f;
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (Immunity <= 0)
            {
                Health -= 1;
                PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
                Immunity = 1.5f;
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            TouchingGround = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            TouchingWall = false;
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
        if (other.tag == "Ground")
        {
            if (GlobalCooldown <= 0)
            {
                AllowJump = true;
                AllowBonusJump = true;
            }
            TouchingGround = true;
        }
        if (other.tag == "DeathZone")
        {
            Health = 0;
        }
        if (other.tag == "LevelClear")
        {
            BeatLevel = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Collectable")
        {
            CanPickup = true;
        }
        if (other.tag == "Ground")
        {
            TouchingGround = false;
        }
    }
}
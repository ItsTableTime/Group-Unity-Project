using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public GameObject SummonedAttack;
    public string SpellSlot1;
    public string SpellSlot2;
    public string SpellSlot3;
    public float GlobalCooldown;
    Vector2 PlayerTransform;

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
    public GameObject SummonedSpikeAttack;
    public float SpikeCooldown;
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
        if (WarpCooldown > 0)
        {
            WarpCooldown -= 1 * Time.deltaTime;
        }
        if (SpikeCooldown > 0)
        {
            SpikeCooldown -= 1 * Time.deltaTime;
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
            GlobalCooldown = 0.25f;
        }
    }
    public void Attack()
    {
        if (GlobalCooldown <= 0)
        {
            SummonedAttack = Instantiate(AttackProjectile, transform.position, transform.rotation);
            SummonedAttack.GetComponent<AttackMovementScript>().AttackDirection = PlayerDirection;
            GlobalCooldown = 0.25f;

        }

    }
    public void Spell1()
    {
        if ((SpikeCooldown <= 0) &(SpellSlot1 == "EarthSpike") & (GlobalCooldown <= 0))
        {
            SummonedSpikeAttack = Instantiate(SpikeAttackProjectile, (transform.position + new Vector3(PlayerDirection, -1.5f)), transform.rotation);
            GlobalCooldown = 1;
            SpikeCooldown = 3;

        }

    }
    public void Spell2()
    {
        if ((DashCooldown <= 0) & (SpellSlot2 == "Dash") & (GlobalCooldown <= 0))
        {
            DashTime = DashLength;
            DashCooldown = 5;
            GlobalCooldown = 0.25f;
        }
        if ((AllowBonusJump == true) & (SpellSlot2 == "BonusJump") & (GlobalCooldown <= 0))
        {
            PlayerRigidbody.AddForce(transform.up * BonusJumpHeight, ForceMode2D.Impulse);
            AllowBonusJump = false;
            GlobalCooldown = 0.25f;
        }
        if ((WarpCooldown <= 0) & (SpellSlot2 == "WarpDash") & (GlobalCooldown <= 0))
        {
            transform.position = PlayerTransform + new Vector2(WarpAmount, 0);
            WarpCooldown = 5;
            GlobalCooldown = 0.25f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            AllowJump = true;
            AllowBonusJump = true;
        }
        if (collision.gameObject.tag == "Wall")
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
}
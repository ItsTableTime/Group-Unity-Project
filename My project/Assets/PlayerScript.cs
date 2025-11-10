using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerScript : MonoBehaviour
{
    float HorizontalMovement;
    float VerticalMovement;
    Rigidbody2D PlayerRigidbody;
    bool AllowJump = true;
    bool AllowDash = true;
    float DashTime;
    public float DashLength;
    public float DashSpeed;
    public float DashCooldown;
    public float Speed;
    public float JumpHeight;
    public float Health;
    public float MaxHealth;
    public float Immunity;
    public PlayerInput input;
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
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
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();

        HorizontalMovement = InputAxis.x;
        VerticalMovement = InputAxis.y;
    }
    public void Jump()
    {
        if (AllowJump == true)
        {
            PlayerRigidbody.AddForce(transform.up * JumpHeight, ForceMode2D.Impulse);
            AllowJump = false;
        }
    }    public void Dash()
    {
        if ((AllowDash == true) & (DashCooldown <= 0))
        {
            DashTime = DashLength;
            DashCooldown = 3;
            AllowDash = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            AllowJump = true;
            AllowDash = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            AllowJump = true;
            AllowDash = true;
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
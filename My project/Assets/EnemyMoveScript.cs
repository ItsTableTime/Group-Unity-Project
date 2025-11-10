using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public float EnemySpeed;
    public float Facing = 1;
    float TurnCooldown = 0;
    public bool EnemyAi = false;
    public float EnemyRange = 0;
    public float AggressiveRange = 0;
    public float EnemySight;
    public GameObject Player;

    void Start()
    {

    }

    void Update()
    {
        transform.position = transform.position + new Vector3(Facing * EnemySpeed * Time.deltaTime, 0, 0);
        TurnCooldown -= 1 * Time.deltaTime;
        EnemySight = Vector2.Distance(transform.position, Player.transform.position);
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
}

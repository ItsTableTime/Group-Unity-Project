using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public float EnemySpeed;
    public float Facing = 1;
    float TurnCooldown = 0;
    void Start()
    {

    }

    void Update()
    {
        transform.position = transform.position + new Vector3(Facing * EnemySpeed * Time.deltaTime, 0, 0);
        TurnCooldown -= 1 * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
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
    }
}

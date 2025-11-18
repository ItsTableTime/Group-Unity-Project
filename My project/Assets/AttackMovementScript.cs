using UnityEngine;

public class AttackMovementScript : MonoBehaviour
{
    public float Lifespan;
    public float Speed;
    public float AttackDirection;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(AttackDirection*Speed * Time.deltaTime, 0);
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0)
        {
            Destroy(gameObject);
        }
    }
}

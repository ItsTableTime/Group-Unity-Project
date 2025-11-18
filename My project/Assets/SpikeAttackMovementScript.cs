using UnityEngine;

public class SpikeAttackMovementScript : MonoBehaviour
{
    public float Speed;
    public float Lifespan;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(0, Speed * Time.deltaTime);
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0)
        {
            Destroy(gameObject);
        }
    }
}

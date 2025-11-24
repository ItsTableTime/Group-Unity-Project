using System.Drawing;
using UnityEngine;

public class SlamAttackGrowScript : MonoBehaviour
{
    public float Speed;
    public float Lifespan;
    void Start()
    {
        
    }

    void Update()
    {
        
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0)
        {
            Destroy(gameObject);
        }
    }
}

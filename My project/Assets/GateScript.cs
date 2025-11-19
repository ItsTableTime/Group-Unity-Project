using UnityEngine;

public class GateScript : MonoBehaviour
{
    public int EnemyCount;

    void Update()
    {
        if (EnemyCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}

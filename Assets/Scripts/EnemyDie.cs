using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public new GameObject gameObject;
    float killCount = 0;

    public void Die()
    {
        gameObject.GetComponent<EnemyAI>().Dead();
        killCount += 1;
    }
}

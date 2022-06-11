using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public new GameObject gameObject;
    float killCount = 0;

    public void Die()
    {
        killCount += 1;
        gameObject.GetComponent<EnemyAI>().Dead();
    }
}

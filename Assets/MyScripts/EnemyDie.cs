using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public new GameObject gameObject;

    public void die()
    {
        gameObject.GetComponent<EnemyAI>().dead();
    }
}

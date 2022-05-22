using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Animator anim;
    public NavMeshAgent agent;
    public Transform player; 
    public GameObject enemy;
    public GameObject projectile;
    public Transform rayPoint;
    public LayerMask isGround, isPlayer;
    public int killTime;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool isDead = false;
    public float sightDistance, attackDistance;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance > sightDistance && !isDead)
        {
            anim.SetBool("isWalking", false);
            agent.SetDestination(transform.position);
        }
        else if(distance > attackDistance && !isDead)
        {
            anim.SetBool("isWalking", true);
            agent.SetDestination(player.position);
        }
        else if (!isDead)
        {
            anim.SetBool("isWalking", false);
            agent.SetDestination(transform.position);
            Vector3 playerPosition = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(playerPosition);


            if (!alreadyAttacked)
            {
                Rigidbody rb = Instantiate(projectile, rayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, rayPoint.transform.position)).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
                rb.AddForce(transform.up * 2f, ForceMode.Impulse);
                //Destroy(rb, timeBetweenAttacks);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void dead()
    {
        Destroy(enemy, killTime);
        isDead = true;
        agent.SetDestination(transform.position);
        anim.SetBool("isWalking", false);
        GetComponent<Animator>().enabled = false;
    }
}

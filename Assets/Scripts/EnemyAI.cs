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
    public Transform fireBallRayPoint;
    public Transform spawnRayPoint;
    public LayerMask isGround, isPlayer;
    public float timeBetweenAttacks;
    public float killTime;
    bool alreadyAttacked;
    bool isDead = false;
    public float sightDistance, attackDistance;
    GameObject fire;
    public GameObject fireEffect;
    GameObject globExplode;
    public GameObject globExplodeEffect;
    GameObject goop;
    public GameObject goopEffect;
    GameObject spawnPoint;
    public GameObject newEnemy;

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
            GetComponent<Animator>().enabled = true;
            anim.SetBool("isWalking", false);
            agent.SetDestination(transform.position);
        }
        else if(distance > attackDistance && !isDead)
        {
            GetComponent<Animator>().enabled = true;
            anim.SetBool("isWalking", true);
            agent.SetDestination(player.position);
        }
        else if (!isDead)
        {
            GetComponent<Animator>().enabled = true;
            anim.SetBool("isWalking", false);
            agent.SetDestination(transform.position);
            Vector3 playerPosition = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(playerPosition);


            if (!alreadyAttacked)
            {
                Rigidbody ball = Instantiate(projectile, fireBallRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, fireBallRayPoint.transform.position)).GetComponent<Rigidbody>();
                ball.AddForce(transform.forward * 10f, ForceMode.Impulse);
                ball.AddForce(transform.up * 2f, ForceMode.Impulse);
                fire = Instantiate(fireEffect, ball.transform.position, Quaternion.FromToRotation(Vector3.forward, ball.transform.position)) as GameObject;
                fire.transform.parent = ball.transform;
                Destroy(ball.gameObject, timeBetweenAttacks);

                alreadyAttacked = true;
                Invoke(nameof(resetAttack), timeBetweenAttacks);
            }
        }
    }

    private void resetAttack()
    {
        alreadyAttacked = false;
    }

    public void dead()
    {
        globExplode = Instantiate(globExplodeEffect, spawnRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, spawnRayPoint.transform.position)) as GameObject;
        Destroy(globExplode, killTime);
        Destroy(enemy, killTime);
        // OR MAYBE this.enemy.SetActive(false);
        isDead = true;
        agent.SetDestination(transform.position);
        anim.SetBool("isWalking", false);
        GetComponent<Animator>().enabled = false;
        StartCoroutine(SpawnCountdown());
    }

    IEnumerator SpawnCountdown()
    {
        yield return new WaitForSeconds(2);
        float randomForce = Random.Range(8f, 12f);

        Rigidbody seed = Instantiate(projectile, spawnRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, spawnRayPoint.transform.position)).GetComponent<Rigidbody>();
        seed.AddForce(0, 0, randomForce, ForceMode.Impulse);
        seed.AddForce(transform.up * randomForce, ForceMode.Impulse);
        goop = Instantiate(goopEffect, seed.transform.position, Quaternion.FromToRotation(Vector3.forward, seed.transform.position)) as GameObject;
        goop.transform.parent = seed.transform;

        Rigidbody seed2 = Instantiate(projectile, spawnRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, spawnRayPoint.transform.position)).GetComponent<Rigidbody>();
        seed2.AddForce(randomForce, 0, 0, ForceMode.Impulse);
        seed2.AddForce(transform.up * randomForce, ForceMode.Impulse);
        goop = Instantiate(goopEffect, seed2.transform.position, Quaternion.FromToRotation(Vector3.forward, seed2.transform.position)) as GameObject;
        goop.transform.parent = seed2.transform;

        yield return new WaitForSeconds(2);
        spawnPoint = Instantiate(newEnemy, seed.transform.position, Quaternion.FromToRotation(Vector3.forward, seed.transform.position)) as GameObject;
        Destroy(seed.gameObject, killTime);
        spawnPoint = Instantiate(newEnemy, seed2.transform.position, Quaternion.FromToRotation(Vector3.forward, seed2.transform.position)) as GameObject;
        Destroy(seed2.gameObject, killTime);
    }

}

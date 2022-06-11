using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Animator anim;

    public AudioSource enemyAS;
    public AudioClip projectileAC;

    public NavMeshAgent agent;
    public Transform player; 
    public GameObject enemy;
    public GameObject projectile;
    public Transform fireBallRayPoint;
    public Transform spawnRayPoint;
    public Transform sphereRayPoint;
    public LayerMask isGround, isPlayer;

    public float timeBetweenAttacks;
    public float killTime;
    public float sightDistance, attackDistance;
    bool alreadyAttacked;
    bool isDead = false;

    GameObject fire;
    public GameObject fireEffect;
    GameObject globExplode;
    public GameObject globExplodeEffect;
    GameObject goop;
    public GameObject goopEffect;
    GameObject spawnPoint;
    public GameObject newEnemy;
    GameObject globInflate;
    public GameObject sphere;
    

    private void Awake()
    {
        enemy.tag = "Enemy";
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isDead = false;
        enemy.GetComponentInChildren<Collider>().enabled = true;
        enemy.GetComponentInChildren<Renderer>().enabled = true;
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
                enemyAS.clip = projectileAC;
                enemyAS.pitch = Random.Range(1f, 2f);
                enemyAS.volume = Random.Range(0.3f, 0.4f);
                enemyAS.Play();
                Rigidbody ball = Instantiate(projectile, fireBallRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, fireBallRayPoint.transform.position)).GetComponent<Rigidbody>();
                ball.tag = "EnemyProjectile";
                ball.AddForce(transform.forward * 30f, ForceMode.Impulse);
                ball.AddForce(transform.up * 2f, ForceMode.Impulse);
                fire = Instantiate(fireEffect, ball.transform.position, Quaternion.FromToRotation(Vector3.forward, ball.transform.position)) as GameObject;
                fire.transform.parent = ball.transform;
                Destroy(ball.gameObject, timeBetweenAttacks);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Dead()
    {
        if(enemy.tag != "IgnoreRaycast")
        {
            globExplode = Instantiate(globExplodeEffect, spawnRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, spawnRayPoint.transform.position)) as GameObject;
            Destroy(globExplode, killTime);
            globInflate = Instantiate(sphere, sphereRayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, spawnRayPoint.transform.position)) as GameObject;
            Destroy(globInflate, timeBetweenAttacks);
            Destroy(enemy, killTime);
            // OR MAYBE this.enemy.SetActive(false);
            isDead = true;
            agent.SetDestination(transform.position);
            anim.SetBool("isWalking", false);
            GetComponent<Animator>().enabled = false;
            StartCoroutine(SpawnCountdown());
            enemy.tag = "IgnoreRaycast";
            Shoot.killCount += 1;
        }
    }

    // Spawning 2 seeds for duplication effect
    IEnumerator SpawnCountdown()
    {
        float randomForce = Random.Range(8f, 12f);

        yield return new WaitForSeconds(2);

        enemy.GetComponentInChildren<Collider>().enabled = false;
        enemy.GetComponentInChildren<Renderer>().enabled = false;

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

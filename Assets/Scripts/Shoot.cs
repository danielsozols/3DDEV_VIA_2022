using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class Shoot : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    public TextMeshProUGUI killCountText;
    public static float killCount;

    public static Shoot singleton;

    Animator anim;

    public FirstPersonController firstPersonController;

    public Transform rayPoint;

    public float shootForce = 100f;
    public float shootRange = 50f;
    public LayerMask shootLayer;
    public float shootTimer = 1.5f;
    private float _shootTimer;
    private string tagName;

    public GameObject defaultEffect;
    public GameObject bloodEffect;
    public GameObject flashEffect;
    public GameObject shockEffect;

    public AudioSource foleyAS;
    public AudioClip shootAC;
    public AudioClip reloadAC;
    public AudioClip gunEmptyAC;
    public AudioSource impactSoundsAS;
    public AudioClip gunDefaultImpactAC;
    public AudioClip gunBloodImpactAC;

    int ammo = 10;
    bool isReloading;
    
    void Awake()
    {
        float killCount = 0;
        singleton = this;
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        killCountText.text = "GLOBS KILLED: " + killCount.ToString();

        _shootTimer -= Time.deltaTime;

        if(Input.GetButton("Fire1") && ammo > 0 && !isReloading && !PlayerHealth.isPlayerDead)
        {
            ArmedInput();
        }
        else if(Input.GetKeyDown(KeyCode.R) && ammo >= 0 && ammo != 10 && !isReloading && !PlayerHealth.isPlayerDead)
        {
            foleyAS.clip = reloadAC;
            foleyAS.pitch = Random.Range(0.9f, 1f);
            foleyAS.volume = Random.Range(0.7f, 0.8f);
            foleyAS.Play();
            Reload();
        }
        else if(Input.GetButton("Fire1") && ammo <= 0 && !isReloading && _shootTimer <= 0 && !PlayerHealth.isPlayerDead)
        {
            foleyAS.clip = gunEmptyAC;
            foleyAS.pitch = Random.Range(0.9f, 1f);
            foleyAS.volume = Random.Range(0.7f, 0.8f);
            foleyAS.Play();
            _shootTimer = shootTimer;
        }

        ammoText.text = ammo.ToString();
    }

    public void Reload()
    {
        anim.SetTrigger("Reload");
        StartCoroutine(ReloadCountdown(2f));
    }

    IEnumerator ReloadCountdown(float timer)
    {
        while(timer > 0f)
        {
            isReloading = true;
            timer -= Time.deltaTime;
            yield return null;
        }

        if(timer <= 0f)
        {
            isReloading = false;
            ammo = 10;
        }
    }

    void ArmedInput()
    {
        GameObject flash;

        if(_shootTimer <= 0)
        {
            _shootTimer = shootTimer;
            foleyAS.clip = shootAC;
            foleyAS.pitch = Random.Range(0.9f, 1f);
            foleyAS.volume = Random.Range(0.5f, 0.6f);
            foleyAS.Play();
            flash = Instantiate(flashEffect, rayPoint.transform.position, Quaternion.FromToRotation(Vector3.forward, rayPoint.transform.position)) as GameObject;
            flash.transform.parent = rayPoint.transform;
            Destroy(flash, shootTimer);
            ammo--;
            anim.SetTrigger("Shoot");
            ShootAttack();
        }
    }

    public void ShootAttack()
    {
        Ray ray = new Ray(rayPoint.transform.position, rayPoint.transform.forward);

        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, shootRange, shootLayer))
        {
            EnemyDie enemy = hit.transform.GetComponent<EnemyDie>();
            if (enemy != null) 
            {
                enemy.Die();
            }

            if(hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(shootForce * transform.forward, hit.point);
            }

            tagName = hit.transform.tag;

            GameObject decal;
            GameObject shock;

            switch (tagName)
            {
                case "Enemy":
                    decal = Instantiate(bloodEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
                    shock = Instantiate(shockEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
                    Destroy(decal, shootTimer * 2);
                    Destroy(shock, shootTimer * 2);
                    decal.transform.parent = hit.transform;
                    //shock.transform.parent = hit.transform;
                    impactSoundsAS.clip = gunBloodImpactAC;
                    impactSoundsAS.pitch = Random.Range(0.9f, 1f);
                    impactSoundsAS.volume = Random.Range(0.3f, 0.4f);
                    impactSoundsAS.Play();
                    tagName = "";
                    break;
                case "Untagged":
                    decal = Instantiate(defaultEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
                    shock = Instantiate(shockEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
                    Destroy(decal, shootTimer * 2);
                    Destroy(shock, shootTimer * 2);
                    decal.transform.parent = hit.transform;
                    shock.transform.parent = hit.transform;
                    impactSoundsAS.clip = gunDefaultImpactAC;
                    impactSoundsAS.pitch = Random.Range(0.9f, 1f);
                    impactSoundsAS.volume = Random.Range(0.1f, 0.2f);
                    impactSoundsAS.Play();
                    tagName = "";
                    break;
            }
        }
    }
}


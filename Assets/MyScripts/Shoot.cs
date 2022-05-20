using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class Shoot : MonoBehaviour
{
    public static Shoot singleton;
    Animator anim;

    [Header("GAMEOBJECT THAT HAS FIRSTPERSON CONTROLLER SCRIPT ATTACHED")]
    [SerializeField] FirstPersonController firstPersonController;

    [Header("THE POSITION THE RAY CASTS FROM")]
    [SerializeField] Transform rayPoint;

    [Header("SHOOT")]
    public float shootForce = 100f;
    public float shootRange = 50f;
    public LayerMask shootLayer;
    public float shootTimer = 1.5f;
    private float _shootTimer;
    private string tagName;

    [Header("SHOOT IMPACT EFFECTS")]
    public GameObject defaultEffect;
    public GameObject bloodEffect;
    public GameObject flashEffect;
    public GameObject shockEffect;

    [Header("AUDIO")]
    public AudioSource foleyAS;
    public AudioClip shootAC;
    public AudioSource impactSoundsAS;
    public AudioClip gunDefaultImpactAC;
    public AudioClip gunBloodImpactAC;
    
    void Awake()
    {
        singleton = this;
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        _shootTimer -= Time.deltaTime;

        if(Input.GetButton("Fire1"))
        {
            ArmedInput();
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
                    decal.transform.parent = hit.transform;
                    shock.transform.parent = hit.transform;
                    impactSoundsAS.clip = gunBloodImpactAC;
                    impactSoundsAS.pitch = Random.Range(0.9f, 1f);
                    impactSoundsAS.volume = Random.Range(0.1f, 0.2f);
                    impactSoundsAS.Play();
                    tagName = "";
                    break;
                case "Untagged":
                    decal = Instantiate(defaultEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
                    shock = Instantiate(shockEffect, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.point)) as GameObject;
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


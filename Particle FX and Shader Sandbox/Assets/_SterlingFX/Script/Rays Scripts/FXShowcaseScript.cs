using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXShowcaseScript : MonoBehaviour
{
    public enum ShowCaseType
    {
        shoot,
        follow,
        impact,
        all
    }

    private ShowCaseType previousMode;
    public ShowCaseType currentMode;

    private ParticleTemplate previousBulletType;

    [ReadOnlyField]
    public int currentBulletIndex;
    [ReadOnlyField]
    public ParticleTemplate currentBulletType;

    //public float timeBetweenShots;
    [ReadOnlyField]
    public float timeSinceLastShot;

    [Header("Camera Parameters")]
    public Vector3 followCameraOffset;
    public Vector3 impactCameraOffset;

    [Header("Bullet Data")]
    public List<ParticleTemplate> BulletTemplates;

    public GameObject Wall;

    [ReadOnlyField]
    public Camera mainCamera;
    [ReadOnlyField]
    public GameObject particleInstance;



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        previousMode = currentMode;

        currentBulletType = BulletTemplates[currentBulletIndex];

        previousBulletType = currentBulletType;

        InitializeDemo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentBulletIndex++;

            if (currentBulletIndex >= BulletTemplates.Count)
            {
                currentBulletIndex = 0;
            }

            currentBulletType = BulletTemplates[currentBulletIndex];
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentBulletIndex--;

            if (currentBulletIndex < 0)
            {
                currentBulletIndex = BulletTemplates.Count - 1;
            }

            currentBulletType = BulletTemplates[currentBulletIndex];
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMode = NextShowCaseType();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMode = PreviousShowCaseType();
        }
        

        if (currentMode != previousMode)
        {
            Debug.Log("Mode Changed");
            Destroy(particleInstance);
            InitializeDemo();
            previousMode = currentMode;
        }

        if (currentBulletType != previousBulletType)
        {
            Destroy(particleInstance);
            InitializeDemo();
            previousBulletType = currentBulletType;
        }

        switch (currentMode)
        {
            case ShowCaseType.follow:
                mainCamera.transform.position = particleInstance.transform.position + followCameraOffset;
                break;
            case ShowCaseType.impact:
                timeSinceLastShot += Time.deltaTime;
                if (timeSinceLastShot >= currentBulletType.ImpactMarkLifeSpan)
                {
                    timeSinceLastShot = 0;
                    StartCoroutine(DeathTimer(InstantiateParticle(), currentBulletType.ImpactMarkLifeSpan));
                }
                break;

        }
    }

    private ShowCaseType NextShowCaseType()
    {
        switch (currentMode)
        {
            case ShowCaseType.shoot:
                return ShowCaseType.follow;

            case ShowCaseType.follow:
                return ShowCaseType.impact;

            case ShowCaseType.impact:
                return ShowCaseType.shoot;

            default:
            return ShowCaseType.follow;      
        }
    }

    private ShowCaseType PreviousShowCaseType()
    {
        switch (currentMode)
        {
            case ShowCaseType.shoot:
                return ShowCaseType.impact;

            case ShowCaseType.follow:
                return ShowCaseType.shoot;

            case ShowCaseType.impact:
                return ShowCaseType.follow;

            default:
                return ShowCaseType.follow;
        }
    }

    private GameObject InstantiateParticle()
    {
        switch (currentMode)
        {
            case ShowCaseType.shoot:
                particleInstance = Instantiate(currentBulletType.MuzzleFlash);
                break;

            case ShowCaseType.follow:
                particleInstance = Instantiate(currentBulletType.BulletProjectile);
                break;

            case ShowCaseType.impact:
                particleInstance = Instantiate(currentBulletType.BulletImpactMark);
                break;

            case ShowCaseType.all:
                particleInstance = Instantiate(currentBulletType.MuzzleFlash);
                break;

            default:
                particleInstance = Instantiate(currentBulletType.BulletProjectile);
                break;
        }
        
        return particleInstance;
    }

    private void InitializeDemo()
    {
        switch (currentMode)
        {
            case ShowCaseType.follow:
                Wall.SetActive(false);
                InstantiateParticle().GetComponent<Rigidbody>().AddForce(Vector3.forward * currentBulletType.BulletSpeed);
                mainCamera.transform.position = particleInstance.transform.position + followCameraOffset;
                break;

            case ShowCaseType.impact:
                Wall.SetActive(true);

                StartCoroutine(DeathTimer(InstantiateParticle(), currentBulletType.ImpactMarkLifeSpan));
                timeSinceLastShot = 0;
                mainCamera.transform.position = impactCameraOffset;
                break;
        }
    }

    IEnumerator DeathTimer(GameObject particle, float lifespan)
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(particle);
    }
}

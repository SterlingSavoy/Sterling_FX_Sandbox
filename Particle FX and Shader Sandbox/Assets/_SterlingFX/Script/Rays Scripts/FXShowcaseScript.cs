using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXShowcaseScript : MonoBehaviour
{
    public enum ShowCaseType
    {
        shoot,
        follow,
        impact
    }

    public enum Element
    {
        flame,
        ice,
        shock
    }

    private ShowCaseType previousMode;
    public ShowCaseType currentMode;

    private Element previousElement;
    public Element currentElement;


    public float bulletSpeed;

    [Header("Camera Parameters")]
    public Vector3 followCameraOffset;

    [Header("Bullet Muzzle Flash")]
    public GameObject FlameMuzzleFlashPrefab;
    public GameObject IceMuzzleFlashPrefab;
    public GameObject ShockMuzzleFlashPrefab;

    [Header("Bullet Prefabs")]
    public GameObject FlameBulletPrefab;
    public GameObject IceBulletPrefab;
    public GameObject ShockBulletPrefab;

    [Header("Bullet Impact Prefabs")]
    public GameObject FlameImpactPrefab;
    public GameObject IceImpactPrefab;
    public GameObject ShockImpactPrefab;



    [ReadOnlyField]
    public Camera mainCamera;
    [ReadOnlyField]
    public GameObject BulletInstance;



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        previousMode = currentMode;
        previousElement = currentElement;

        InitializeDemo();



    }

    // Update is called once per frame
    void Update()
    {
        if (currentMode != previousMode)
        {
            Debug.Log("Mode Changed");
        }

        if (currentElement != previousElement)
        {
            Destroy(BulletInstance);
            InitializeDemo();
            previousElement = currentElement;
        }

        switch (currentMode)
        {
            case ShowCaseType.follow:
                mainCamera.transform.position = BulletInstance.transform.position + followCameraOffset;
                break;
        }
    }



    private GameObject InstantiateBullet()
    {
        switch (currentElement)
        {
            case Element.flame:
                BulletInstance = Instantiate(FlameBulletPrefab);
                break;

            case Element.ice:
                BulletInstance = Instantiate(IceBulletPrefab);
                break;

            case Element.shock:
                BulletInstance = Instantiate(ShockBulletPrefab);
                
                break;
        }

        return BulletInstance;
    }

    private void InitializeDemo()
    {
        switch (currentMode)
        {
            case ShowCaseType.follow:
                InstantiateBullet().GetComponent<Rigidbody>().AddForce(Vector3.forward * bulletSpeed);
                mainCamera.transform.position = BulletInstance.transform.position + followCameraOffset;
                break;

        }
    }

}

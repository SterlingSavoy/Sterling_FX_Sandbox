using UnityEngine;
using System.Collections;

public class animController : MonoBehaviour
{

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.Play("Projectile_01");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Particle", menuName = "New Particle Data")]
public class ParticleTemplate : ScriptableObject
{
    [SerializeField]
    private string _particleName;

    [SerializeField]
    private GameObject _bulletMuzzleFlash;

    [SerializeField]
    private GameObject _bulletProjectile;

    [SerializeField]
    private GameObject _bulletImpactMark;

    [SerializeField]
    private float _impactMarkLifeSpan;

    [SerializeField]
    private float _MuzleFlashMarkLifeSpan;

    [SerializeField]
    private float _bulletSpeed;

    public string ParticleName
    {
        get
        {
            return _particleName;
        }
    }

    public GameObject MuzzleFlash
    {
        get
        {
            return _bulletMuzzleFlash;
        }
    }

    public GameObject BulletProjectile
    {
        get
        {
            return _bulletProjectile;
        }
    }

    public GameObject BulletImpactMark
    {
        get
        {
            return _bulletImpactMark;
        }
    }

    public float ImpactMarkLifeSpan
    {
        get
        {
            return _impactMarkLifeSpan;
        }
    }

    public float MuzzleFlashLifeSpan
    {
        get
        {
            return _MuzleFlashMarkLifeSpan;
        }
    }

    public float BulletSpeed
    {
        get
        {
            return _bulletSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private bool damageAOE = false;
    [SerializeField] private bool doForceRigidbodies = true;

    private bool isHitted;
    private Vector3 _prevVector;
    private AudioSource _audioSource;

    [Header("Visual")]
    [SerializeField] private List<GameObject> objectToHideOnHit;
    [SerializeField] private TrailRenderer trail;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionPower = 10f;
    [SerializeField] private float upwardModifier = 0f;

    [Header("DecalAndSound")]
    [SerializeField] private ParticleSystem defaultDecal;
    [SerializeField] private AudioClip defaultHitClip;
    [SerializeField] private List<ProjectileDecalInfo> projectileDecalInfos = new();
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    protected virtual void Update()
    {
        if (isHitted) return;
        _prevVector = transform.position;
        Move();
        HitControl();
    }
    protected virtual void Move()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }
    private void HitControl()
    {
        if (Physics.Linecast(_prevVector, transform.position, out var rayHit, hitLayerMask))
        {
            Hit(rayHit);
        }
    }
    private void Hit(RaycastHit rayHit)
    {
        isHitted = true;

        PlayHitParticle(rayHit, rayHit.collider.transform);
        PlayHitSound(rayHit);

        //Damage to damageable object/objects;
        InflictDamage(rayHit);

        //Force to rigidbodies
        DoForceRigidbodies(rayHit);

        //Hide and destroy
        foreach (var obj in objectToHideOnHit)
        {
            obj.SetActive(false);
        }
        Destroy(gameObject, 1f);
    }

    private void InflictDamage(RaycastHit rayHit)
    {
        if (damageAOE)
        {
            var explosionColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var explosionCollider in explosionColliders)
            {
                // if (rayHit.collider.TryGetComponent<IDamageable>(out var damageable))
                // {
                //     damageable.Damage(damageAmount);
                // }
            }
        }
        else
        {
            //Damage
            // if (rayHit.collider.TryGetComponent<IDamageable>(out var damageable))
            // {
            //     damageable.Damage(damageAmount);
            // }
        }
    }
    private void DoForceRigidbodies(RaycastHit rayHit)
    {
        if (doForceRigidbodies)
        {
            var explosionColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var explosionCollider in explosionColliders)
            {
                if (explosionCollider.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.AddExplosionForce(explosionPower, rayHit.point, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }
        }
    }
    private void PlayHitParticle(RaycastHit rayHit, Transform particleParent = null)
    {
        var selectedParticle = GetHitParticle(rayHit) ?? defaultDecal;
        if (selectedParticle == null) return;
        var pos = rayHit.point - transform.forward * .025f;
        var rot = Quaternion.LookRotation(rayHit.normal);
        var particle = Instantiate(selectedParticle, pos, rot, particleParent);
        particle.Play();
    }
    private ParticleSystem GetHitParticle(RaycastHit rayHit)
    {
        var decalInfo = GetDecalInfo(rayHit);
        if (decalInfo == null)
            return null;
        else
            return decalInfo.decalFx;
    }
    private void PlayHitSound(RaycastHit rayHit)
    {
        var clip = GetHitSound(rayHit) ?? defaultHitClip;
        if (clip == null) return;
        _audioSource.PlayOneShot(clip);
    }
    private AudioClip GetHitSound(RaycastHit rayHit)
    {
        var decalInfo = GetDecalInfo(rayHit);
        if (decalInfo == null)
            return null;
        else
            return decalInfo.hitAudio;
    }
    public ProjectileDecalInfo GetDecalInfo(RaycastHit rayHit)
    {
        var hitLayer = rayHit.collider.gameObject.layer;
        var decalInfo = projectileDecalInfos.FirstOrDefault(projectileDecalInfo =>
                                    ((projectileDecalInfo.hitLayer & (1 << hitLayer)) != 0));
        return decalInfo;
    }
    private void OnDisable()
    {
        if (trail)
        {
            trail.Clear();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    [System.Serializable]
    public class ProjectileDecalInfo
    {
        public LayerMask hitLayer;
        public ParticleSystem decalFx;
        public AudioClip hitAudio;
    }
}
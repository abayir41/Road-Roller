using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableObj : MonoBehaviour, IInteractable
{
    [Header("Collision Configurations")]
    [SerializeField] private int destroyThreshold;
    [SerializeField] private int objectHitPoint = 10;
    [SerializeField] private ObjectType objectType = ObjectType.Small;
    [Tooltip("This is the setting of Delay when dozer collide with a object")]
    [SerializeField] private int delayWhileCollision = 5;
    private bool _isDozer;
    private PlayerController _dozerPlayerController;
    public int DestroyThreshold 
    {
        get
        {
            if (_isDozer)
            {
                destroyThreshold = _dozerPlayerController.TotalCrashPoint;
            }
            return destroyThreshold;
        }
    }
    public int ObjectHitPoint
    {
        get
        {
            if (_isDozer)
            {
                objectHitPoint = _dozerPlayerController.TotalCrashPoint;
            }
            return objectHitPoint;
        }
    }
    public Vector3 ColliderPosition => GetComponent<Collider>().bounds.center;
    public ObjectType ObjectType => objectType;
    
    [Header("Particle Effect Settings")]
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private float particleVolumeMultiplier = 1f;
    [SerializeField] private float particleCount;
    [SerializeField] private float particleSize;
    
    [Header("After Collision Object")]
    [SerializeField] private GameObject crashGameObject;
    [SerializeField] private Transform crashPos;

    [Header("Some Configurations")] 
    [SerializeField] private Transform particleObjSpawnParent;
    [SerializeField] private BoxCollider shapeOfParticleCollider;
    [SerializeField] private List<MeshRenderer> meshRenderers;
    [SerializeField] private List<Collider> disableColliders;
    

    private void Awake()
    {
        if (GetComponent<PlayerController>() != null)
        {
            _isDozer = true;
            _dozerPlayerController = GetComponent<PlayerController>();
        }
    }

    public void Interact(PlayerController playerController)
    {
        if (playerController.TotalCrashPoint > DestroyThreshold)
        {
            playerController.ActionSysCar.ObjectGotHit(this);
            Interaction(playerController);
        }
    }
    
    private void Interaction(PlayerController playerController)
    {
        var delay = 0.03f;
        if (playerController.TotalCrashPoint != 0)
        {
            delay = (float)delayWhileCollision / playerController.TotalCrashPoint;
            if (delay > 0.3f)
            {
                delay = 0.3f;
            }
        }
        StartCoroutine(IEInteraction(delay));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator IEInteraction(float delay)
    {
        var particle = SpawnParticle();
        if (particle == null) yield return null;
        Destroy(particle,2f);
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
        yield return new WaitForSeconds(delay);
        foreach (var disableCollider in disableColliders)
        {
            disableCollider.enabled = false;
        }
        yield return new WaitForSeconds(0.3f - delay);
        SpawnCrash();
        Destroy(gameObject);
    }
    private GameObject SpawnParticle()
    {
        if (particleEffect == null) return null;
        var particle = Instantiate(particleEffect,particleObjSpawnParent);
        particle.transform.localPosition = Vector3.zero;
        particle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        particle.transform.localScale = Vector3.one;
        particle.transform.parent = null;
        
        var particleSys = particle.GetComponent<ParticleSystem>();
        var boxCollider = shapeOfParticleCollider;
        
        var shape = particleSys.shape;
        shape.position = boxCollider.center;
        shape.scale = boxCollider.size * particleVolumeMultiplier;

        var emissionRateOverTime = new ParticleSystem.MinMaxCurve(particleCount - 1, particleCount); 
        var emission = particleSys.emission;
        emission.rateOverTime = emissionRateOverTime;
        
        var starSizeMinMax = new ParticleSystem.MinMaxCurve(particleSize-1,particleSize);
        var particleSysMain = particleSys.main; 
        particleSysMain.startSize = starSizeMinMax;
        
        particleSys.Play();
        return particle;
    }
    private void SpawnCrash()
    {
        if (crashGameObject == null || crashPos == null) return;
        var crash = Instantiate(crashGameObject,crashPos);
        ChangeCrashColor(crash);
        crash.transform.localPosition = Vector3.zero;
        crash.transform.localRotation = Quaternion.Euler(Vector3.zero);
        crash.transform.localScale = Vector3.one;
        crash.transform.parent = null;
    }
    private void ChangeCrashColor(GameObject crash)
    {
        if (GetComponent<IColorChangerRandomly>() == null || crash.GetComponent<IColorChanger>() == null) return;
        var gameController = GameController.Instance;
        var colorInterface = GetComponent<IColorChangerRandomly>();
        var colorChanger = crash.GetComponent<IColorChanger>();
        var colorDict = gameController.RandomlyChangedMaterialsListAndColours[colorInterface];
        var color = colorDict.First().Value;
        colorChanger.ChangeColor(color,2);
    }
    private void OnDestroy()
    {
        ActionSys.ObjectDestroyed?.Invoke(gameObject);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableObj : MonoBehaviour, IInteractable
{
    
    [Header("Collision Configurations")]
    [SerializeField] private ObjectType objectType = ObjectType.Small;

    public int DestroyThreshold 
    {
        get
        {
            if (IsDozer)
            {
                if (_dozerPlayerController.IsAI && _dozerPlayerController.Level == PlayerController.MainPlayer.Level)
                {
                    return 0;
                }

                if (!_dozerPlayerController.IsAI && _dozerPlayerController.Level == PlayerController.MainPlayer.Level)
                {
                    return _dozerPlayerController.Score;
                }
                return _dozerPlayerController.Score;
            }
            else
            {
                return GameController.DestroyThresholds[ObjetTypeIndexMatcher(objectType)];
            }
        }
    }
    public int ObjectHitPoint
    {
        get
        {
            if (IsDozer)
            {
                return _dozerPlayerController.Score;
            }
            else
            {
                return MapController.Instance.mapConfig.ObjectHitPoints[ObjetTypeIndexMatcher(objectType)];
            }
        }
    }

    private int DestroyDelayWhileCollision
    {
        get
        {
            if (IsDozer)
            {
                return 0;
            }
            else
            {
                return MapController.Instance.mapConfig.ObjectDestroyWait[ObjetTypeIndexMatcher(objectType)];
            }
        }
    }

    static int ObjetTypeIndexMatcher(ObjectType type)
    {
        int typeObj;
        switch (type)
        {
            case ObjectType.Small:
                typeObj = 0;
                break;
            case ObjectType.Mid:
                typeObj = 1;
                break;
            case ObjectType.Big:
                typeObj = 2;
                break;
            case ObjectType.Mega:
                typeObj = 3;
                break;
            default:
                typeObj = 0;
                break;
        }

        return typeObj;
    }
    
    //Dozer Check
    public bool IsDozer { get; private set; }
    private PlayerController _dozerPlayerController;
    
    public Vector3 ColliderPosition => GetComponent<Collider>().bounds.center;
    public ObjectType ObjectType => objectType;

    [Header("Particle Effect Settings")] 
    [SerializeField] public bool haveParticleDeath;
    [SerializeField] public GameObject particleEffect;
    [SerializeField] public float particleVolumeMultiplier = 1f;
    [SerializeField] public float particleCount;
    [SerializeField] public float particleSize;
    
    [Header("After Collision Object")]
    [SerializeField] public GameObject crashGameObject;
    [SerializeField] public Transform crashPos;

    [Header("Some Configurations")] 
    [SerializeField] public Transform particleObjSpawnParent;
    [SerializeField] public BoxCollider shapeOfParticleCollider;
    [HideInInspector] public List<MeshRenderer> meshRenderers;
    private List<Collider> _disableColliders;
    

    public void Awake()
    {
        if (GetComponent<PlayerController>() != null)
        {
            IsDozer = true;
            _dozerPlayerController = GetComponent<PlayerController>();
        }

        if (!IsDozer)
        {
            meshRenderers = new List<MeshRenderer>(GetComponents<MeshRenderer>());
            meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        }
        
        _disableColliders = new List<Collider>(GetComponents<Collider>());
        _disableColliders.AddRange(GetComponentsInChildren<Collider>());
    }

    public void Interact(PlayerController playerController)
    {
        if (playerController.Score > DestroyThreshold)
        {
            playerController.ActionSysCar.ObjectGotHit(this);
            Interaction(playerController);
        }
    }
    
    private void Interaction(PlayerController playerController)
    {
        var delay = 0.03f;
        if (playerController.Score != 0)
        {
            delay = (float)DestroyDelayWhileCollision / playerController.Score;
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
        if (particle != null)
            Destroy(particle,2f);
        
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
        yield return new WaitForSeconds(delay);
        foreach (var disableCollider in _disableColliders)
        {
            disableCollider.enabled = false;
        }
        yield return new WaitForSeconds(0.3f - delay);
        SpawnCrash();
        Destroy(gameObject);
    }
    private GameObject SpawnParticle()
    {
        if (!haveParticleDeath) return null;
        if (particleEffect == null) return null;
        var particle = Instantiate(particleEffect,particleObjSpawnParent);
        particle.transform.localPosition = Vector3.zero;
        particle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        particle.transform.localScale = Vector3.one;
        particle.transform.parent = GameInitializer.CurrentMap.transform;
        
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
        crash.transform.parent = GameInitializer.CurrentMap.transform;
    }
    private void ChangeCrashColor(GameObject crash)
    {
        if (GetComponent<IColorChangerRandomly>() == null || crash.GetComponent<IColorChanger>() == null) return;
        var gameController = MapController.Instance;
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

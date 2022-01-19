using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class InteractableObj : MonoBehaviour, IInteractable
{
    [Header("Collision Configurations")]
    [SerializeField] private int destroyThreshold;
    [SerializeField] private int objectHitPoint = 10;
    [SerializeField] private ObjectType objectType = ObjectType.Small;
    [Tooltip("This is the setting of Delay when dozer collide with a object")]
    [SerializeField] private int sizeOfObj = 5;
    
    [Header("Particle Effect Settings")]
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private float particleVolumeMultiplier = 1f;
    [SerializeField] private float particleCount;
    [SerializeField] private float particleSize;
    
    [Header("After Collision Object")]
    [SerializeField] private GameObject crashGameObject;
    [SerializeField] private Transform crashPos;
    
    
    
    
    public ObjectType ObjectType
    {
        get { return objectType; }
    }
    public int ObjectHitPoint
    {
        get { return objectHitPoint; }
    }

    public void Interact()
    {
        defaultDestroyable = true;
        if (defaultDestroyable)
        {
            ActionSys.ObjectGotHit(this);
            Interaction();
        }
    }
    
    private void Interaction()
    {
        var delay = 0.03f;
        if (GameController.Instance.TotalCrashPoint != 0)
        {
            delay = (float)sizeOfObj / GameController.Instance.TotalCrashPoint;
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
        Destroy(particle,2f);
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.3f - delay);
        SpawnCrash();
        Destroy(gameObject);
    }
    private GameObject SpawnParticle()
    {
        if (particleEffect == null) return null;
        var mainTransform = transform;
        var particle = Instantiate(particleEffect,mainTransform);
        particle.transform.localPosition = Vector3.zero;
        particle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        particle.transform.localScale = Vector3.one;
        particle.transform.parent = null;
        
        var particleSys = particle.GetComponent<ParticleSystem>();
        var boxCollider = GetComponent<BoxCollider>();
        
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
        var colorDict = gameController.randomlyChangedMaterialsListAndColours[colorInterface];
        var color = colorDict.First().Value;
        colorChanger.ChangeColor(color,2);
    }


}

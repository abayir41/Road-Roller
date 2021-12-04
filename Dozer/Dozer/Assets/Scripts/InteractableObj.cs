using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Debug = UnityEngine.Debug;

public class InteractableObj : MonoBehaviour, IInteractable
{
    [SerializeField] private bool defaultDestroyable;
    [SerializeField] private float destroyThreshold;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private GameObject crashGameObject;
    [SerializeField] private Transform crashPos;
    [SerializeField] private float particleCount;
    [SerializeField] private float particleSize;

    public void Interact(Collider collider)
    {
        if (defaultDestroyable || collider.bounds.size.magnitude > destroyThreshold)
        {
            Interaction();
        }
    }

    private void Interaction()
    {
        StartCoroutine(IEInteraction(0.15f));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator IEInteraction(float delay)
    {
        SpawnParticle();
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(delay);
        SpawnCrash();
        Destroy(gameObject);
    }
    private void SpawnParticle()
    {
        if (particleEffect == null) return;
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
        shape.scale = boxCollider.size;

        var emissionRateOverTime = new ParticleSystem.MinMaxCurve(particleCount - 1, particleCount); 
        var emission = particleSys.emission;
        emission.rateOverTime = emissionRateOverTime;
        
        var starSizeMinMax = new ParticleSystem.MinMaxCurve(particleSize-1,particleSize);
        var particleSysMain = particleSys.main; 
        particleSysMain.startSize = starSizeMinMax;
        
        particleSys.Play();
    }

    private void SpawnCrash()
    {
        if (crashGameObject == null || crashPos == null) return;
        var crash = Instantiate(crashGameObject);
        ChangeCrashColor(crash);
        crash.transform.position = crashPos.position;
        crash.transform.rotation = crashPos.rotation;
        crash.transform.localScale = crashPos.localScale;
    }

    private void ChangeCrashColor(GameObject crash)
    {
        if (GetComponent<IColorChangerRandomly>() == null || crash.GetComponent<IColorChanger>() == null) return;
        var gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        var colorInterface = GetComponent<IColorChangerRandomly>();
        var colorChanger = crash.GetComponent<IColorChanger>();
        var colorDict = gameController.randomlyChangedMaterialsListAndColours[colorInterface];
        var color = colorDict.First().Value;
        colorChanger.ChangeColor(color,2);
    }


}

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
    [SerializeField] private Transform particlePos;
    
    public void Interact(Collider collider)
    {
        if (defaultDestroyable || collider.bounds.size.magnitude > destroyThreshold)
        {
            Interaction();
        }
    }

    private void Interaction()
    {
        StartCoroutine(IEInteraction(0.3f));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator IEInteraction(float delay)
    {
        SpawnParticle();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(delay);
        SpawnCrash();
        Destroy(gameObject);
    }
    private void SpawnParticle()
    {
        if (particleEffect == null  ||  particlePos == null) return;
        var particle = Instantiate(particleEffect);
        particle.transform.position = particlePos.position;
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

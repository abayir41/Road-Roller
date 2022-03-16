using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorScript : MonoBehaviour
{
    public List<GameObject> Objects;
    
    public GameObject SpecificObject;
    public InteractableObj specifIbtera => SpecificObject.GetComponent<InteractableObj>();

    public GameObject particleEffect;

    public InteractableObj RefParticle;
    

    [ContextMenu("TestParticle")]
    public void TestParticle()
    {
        StartCoroutine(TestInteraction());
    }
    private IEnumerator TestInteraction()
    {
        var particle = SpawnTestParticle();

        var meshRenderers = new List<MeshRenderer>(SpecificObject.GetComponents<MeshRenderer>());
        meshRenderers.AddRange(SpecificObject.GetComponentsInChildren<MeshRenderer>());
        
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }

        yield return new WaitForSeconds(2);
        if (particle != null)
            DestroyImmediate(particle);
        
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }
    
    private GameObject SpawnTestParticle()
    {
        if (particleEffect == null) return null;
        var particle = Instantiate(particleEffect,SpecificObject.transform);
        particle.transform.localPosition = Vector3.zero;
        particle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        particle.transform.localScale = Vector3.one;
        particle.transform.parent = null;
        
        var particleSys = particleEffect.GetComponent<ParticleSystem>();
        var boxCollider = specifIbtera.shapeOfParticleCollider;
        
        var shape = particleSys.shape;
        shape.position = boxCollider.center;
        shape.scale = boxCollider.size * specifIbtera.particleVolumeMultiplier;

        var emissionRateOverTime = new ParticleSystem.MinMaxCurve(specifIbtera.particleCount - 1, specifIbtera.particleCount); 
        var emission = particleSys.emission;
        emission.rateOverTime = emissionRateOverTime;
        
        var starSizeMinMax = new ParticleSystem.MinMaxCurve(specifIbtera.particleSize-1,specifIbtera.particleSize);
        var particleSysMain = particleSys.main; 
        particleSysMain.startSize = starSizeMinMax;
        
        particleSys.Play();
        return particle;
    }

    [ContextMenu("AddColliderSpecific")]
    private void AddColliderSpecific()
    {
        AddColliderAndAttachHouse(SpecificObject);
    }
    
    [ContextMenu("AddColliderAll")]
    private void AddColliderS()
    {
        foreach (var asd in Objects)
        {
            
            if (asd.GetComponent<InteractableObj>() != null)
            {
                var a = AddColliderAndAttachHouse(asd);
                if(asd.GetComponent<InteractableObj>().haveParticleDeath)
                    setParticle(asd,a);
            }
        }
    }
    private BoxCollider AddColliderAndAttachHouse(GameObject obj)
    {
        BoxCollider x;
        if (obj.transform.childCount == 0)
        {
            if(obj.GetComponent<BoxCollider>() == null)
                x = obj.AddComponent<BoxCollider>();
            else
                x = obj.GetComponent<BoxCollider>();
        }
        else
        {
            List<BoxCollider> asd = new List<BoxCollider>();
            
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if(obj.transform.GetChild(i).GetComponent<BoxCollider>() == null)
                    asd.Add(obj.transform.GetChild(i).gameObject.AddComponent<BoxCollider>());
                else
                    asd.Add(obj.transform.GetChild(i).GetComponent<BoxCollider>());
            }

            BoxCollider max = asd[0];
            
            for (int i = 1; i < asd.Count; i++)
            {
                if (asd[i].bounds.size.magnitude > asd[i - 1].bounds.size.magnitude)
                {
                    max = asd[i];
                }
            }

            
            foreach (var boxCollider in asd)
            {
                if(boxCollider != max)
                    DestroyImmediate(boxCollider,true);
            }
            
            x = max;
        }
        
        obj.GetComponent<InteractableObj>().shapeOfParticleCollider = x;

        return x;
    }

    private void setParticle(GameObject obj, BoxCollider refCollider)
    {
        var a = obj.GetComponent<InteractableObj>();
        a.particleEffect = particleEffect;
        var ratio = RefParticle.shapeOfParticleCollider.size.magnitude / refCollider.size.magnitude;

        a.particleObjSpawnParent = refCollider.transform;
        a.particleCount = RefParticle.particleCount / ratio;
        a.particleSize = RefParticle.particleSize / ratio;
        a.particleVolumeMultiplier = 1;
    }
}

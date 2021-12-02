using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static string DozerTag = "Roller";
    
    [SerializeField] private string houseTag = "House";
    [SerializeField] private GameObject dozerGameObject;
    [SerializeField] private List<Transform> dozerFollowers;
    private Dictionary<Transform,Vector3> _dozerFollowersAndFarFromDozer;
    private Vector3 _cameraFarFromDozer;
    private Transform _dozerTrans;
    private Transform _cameraTrans;
    private Camera _camera;
    private void Awake()
    {
        //Paint the all buildings, cars, trees...
        var paintableObjs = FindObjectsOfType<ColorChanger>().ToList();
        foreach (var paintableObj in paintableObjs)
        {
            var materialIndexes = paintableObj.gameObject.GetComponent<IRandomlyPaintedMaterialIndex>().MaterialIndexes;
            foreach (var materialIndex in materialIndexes)
            {
                paintableObj.gameObject.GetComponent<IColorChangerRandomly>().SelectColorRandomly(materialIndex);
            }
        }

        //Getting Camera 
        if (!(Camera.main is null)) _camera = Camera.main; 
        _cameraTrans = _camera.transform;
        
        //Caching
        _dozerTrans = dozerGameObject.transform;
        var dozerTransPosition = _dozerTrans.position;
        _cameraFarFromDozer = _cameraTrans.position - dozerTransPosition;

        _dozerFollowersAndFarFromDozer = new Dictionary<Transform, Vector3>();
        dozerFollowers.ForEach(followerTrans =>
        {
            var dozerFollowerFarFromDozer = followerTrans.position - dozerTransPosition;
            _dozerFollowersAndFarFromDozer.Add(followerTrans,dozerFollowerFarFromDozer);
        });
    }

    private void Update()
    {
        var dozerTransPosition = _dozerTrans.position;
        foreach (var keyValuePair in _dozerFollowersAndFarFromDozer)
        {
            var followerTrans = keyValuePair.Key;
            var distance = keyValuePair.Value;
            ObjectFollower(distance,dozerTransPosition,followerTrans);
        }
        ObjectFollower(_cameraFarFromDozer,dozerTransPosition,_cameraTrans);

        
        var house = Looking_Any_Big_House();
        if (!(house is null))
        {
            Renderer houseRenderer = house.GetComponent<Renderer>();
            var materials = houseRenderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                Color color = material.color;
                color.a = 0.5f;
                var ColorChange = house.GetComponent<IColorChanger>();
                ColorChange.ChangeColor(color,i);
            }
        }
        
    }

    void ObjectFollower(Vector3 distance, Vector3 from, Transform obj)
    {
        obj.position = distance + from;
    }

    
    private GameObject Looking_Any_Big_House()
    {
        GameObject savedGameObject = null;
        foreach (var keyValuePair in _dozerFollowersAndFarFromDozer)
        {
            var followerTrans = keyValuePair.Key;
            var distance = keyValuePair.Value;
            Ray ray = _camera.ScreenPointToRay(_camera.WorldToScreenPoint(followerTrans.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(houseTag))
            {
                savedGameObject = hit.collider.gameObject;
            }
            else
            {
                return null;
            }
               
        }

        return savedGameObject;

    }
    
}

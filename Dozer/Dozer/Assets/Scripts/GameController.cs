using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float power;
    private ISteerSystem _steerSystem;
    private void Awake()
    {

        var paintableObjs = FindObjectsOfType<ColorChanger>().OfType<IColorChangerRandomly>().ToList();
        foreach (var paintableObj in paintableObjs)
        {
            paintableObj.SelectColorRandomly();
        }

        _steerSystem = GetComponent<BasicSteerSystem>();

        
    }

    private void Update()
    {
        Debug.Log(_steerSystem.Angle);
    }
}

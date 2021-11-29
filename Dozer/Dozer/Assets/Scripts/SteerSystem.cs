using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface ISteerSystem
{
    float Angle { get; }
}
public class SteerSystem : MonoBehaviour,ISteerSystem
{
    
    public float Angle
    {
        get { return _angle; }
    }

    private float _angle;


    private Vector2 _startPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPos = touch.position;
            }else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 direction = touch.position - _startPos;
                float angle = Mathf.Atan2(direction.y,direction.x);
                float angleInDegrees = angle * Mathf.Rad2Deg;
                Debug.Log(angleInDegrees);
            }
        }
    }
    
}

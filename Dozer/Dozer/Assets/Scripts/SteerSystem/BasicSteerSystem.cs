using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSteerSystem : MonoBehaviour,ISteerSystem
{
    public float Angle { get { return _angle; } }
    
    
    private float _angle;
    private Vector2 _startPos;
    private int _radiusOfSteer;

    [SerializeField]
    private float radiusOfSteerDivider;

    private void Start()
    {
        _radiusOfSteer = (int)(Screen.width / radiusOfSteerDivider);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 direction = touch.position - _startPos;
                var lengthOfTouch = direction.x;
                if (Math.Abs(lengthOfTouch) > _radiusOfSteer)
                {
                    if (lengthOfTouch > 0)
                    {
                        lengthOfTouch = _radiusOfSteer;
                    }
                    else
                    {
                        lengthOfTouch = -_radiusOfSteer;
                    }
                }

                
                float angle = Mathf.Acos(lengthOfTouch / _radiusOfSteer);;
                float angleInDegrees = angle * Mathf.Rad2Deg;

                _angle = 90 - angleInDegrees;
            }
        }
    }
}

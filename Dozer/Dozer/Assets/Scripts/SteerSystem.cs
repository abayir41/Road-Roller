using System;
using UnityEngine;

public class SteerSystem : MonoBehaviour,ISteerSystem
{
    public float Angle { get { return _angle; } }
    private float _angle;
    private Vector2 _startPos;
    [SerializeField]
    private float maxRot;

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
                if (angleInDegrees > 0)
                {
                    _angle = 90 - angleInDegrees;
                }
                else if (angleInDegrees < 0 && angleInDegrees >= -90)
                {
                    _angle = 90 - angleInDegrees;
                }else if (angleInDegrees < -90)
                {
                    _angle = -270 - angleInDegrees;
                }
            }

            if (Math.Abs(_angle) > maxRot)
            {
                if (_angle > 0)
                {
                    _angle = maxRot;
                }
                else
                {
                    _angle = -maxRot;
                }
            }
        }
    }
}

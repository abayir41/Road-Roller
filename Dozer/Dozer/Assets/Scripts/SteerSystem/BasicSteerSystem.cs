using System;
using UnityEngine;

public class BasicSteerSystem : MonoBehaviour,ISteerSystem
{
    public float Angle { get; private set; }


    private Vector2 _startPos;
    private int _radiusOfSteer;
    private int _registeredTurn;


    [SerializeField] private float radiusOfSteerDivider;
    [SerializeField] private float minThreshold = 1f;

    private void Start()
    {
        _radiusOfSteer = (int)(Screen.width / radiusOfSteerDivider);
    }

    private void Update()
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
                    _registeredTurn = (int) Math.Abs(lengthOfTouch) / _radiusOfSteer;
                    lengthOfTouch = (Math.Abs(lengthOfTouch) % _radiusOfSteer) * Utilities.PosOrNeg(lengthOfTouch);
                }
                else
                {
                    _registeredTurn = 0;
                }

                float angle = Mathf.Acos(lengthOfTouch / _radiusOfSteer);
                float angleInDegrees = angle * Mathf.Rad2Deg;

                Angle = 90 - angleInDegrees;
                Angle += _registeredTurn * 90 * Utilities.PosOrNeg(Angle);
            }
        }
        else
        {
            _registeredTurn = 0;
            Angle = 0;
        }

        if (Math.Abs(Angle) < minThreshold)
        {
            Angle = 0;
        }
    }


}


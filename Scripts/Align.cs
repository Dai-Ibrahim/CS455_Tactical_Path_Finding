using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehavior
{
    public Kinematic character;
    public GameObject target;

    float maxAngularAcceleration = 5f; // 10
    float maxRotation = 10f; // 5 maxSpeed or max angular velocity

    // the radius for arriving at the target
    float targetRadius = 1f; // maybe better word is tolerance

    // the radius for beginning to slow down
    float slowRadius = 10f; // 2

    // the time over which to achieve target speed
    float timeToTarget = 0.1f;  //0.1

    protected virtual float GetTargetAngle()
    {
        return target.transform.eulerAngles.y;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // get the angle to the target
        float rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, GetTargetAngle()); // degrees

        // check if we're close enough and if so don't rotate
        float rotationSize = Mathf.Abs(rotation);
        if (rotationSize < targetRadius)
        {
            return null;
        }

        // if we are outside the slow radius then use max rotation
        float targetRotation = 0.0f; // target angular velocity
        if (rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        // add direction back to our target angular velocity
        targetRotation *= rotation / rotationSize;

        // acceleration tries to get to the targetRotation
        result.angular = targetRotation - character.angular;
        result.angular /= timeToTarget;

        result.linear = Vector3.zero;

        return result;
    }
}

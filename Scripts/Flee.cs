using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehavior
{
    public Kinematic character;
    public GameObject target;

    float maxAcceleration = 100f;


    protected virtual Vector3 getTargetPosition()
    {
        return target.transform.position;
    }

    public override  SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();


        result.linear = character.transform.position - getTargetPosition();
        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }
}

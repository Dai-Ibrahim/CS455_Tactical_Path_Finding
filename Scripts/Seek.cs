using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehavior
{
    public Kinematic character;
    public GameObject target;

    public float maxAcceleration = 2f;

    public bool flee = false;

    protected virtual Vector3 getTargetPosition()
    {
        return target.transform.position;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        
		result.linear = getTargetPosition() - character.transform.position;
        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }
}

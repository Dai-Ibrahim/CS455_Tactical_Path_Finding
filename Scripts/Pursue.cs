using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Arrive
{
    // the maximum prediction time
    float maxPredictionTime = 1f;

    
    protected override Vector3 getTargetPosition()
    {
        Vector3 directionToTarget = target.transform.position - character.transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float mySpeed = character.linear.magnitude;
        float predictionTime; 
        if (mySpeed <= distanceToTarget / maxPredictionTime)
        {
            predictionTime = maxPredictionTime;
        }
        else
        {
          
            predictionTime = distanceToTarget / mySpeed;
        }

        Kinematic myMovingTarget = target.GetComponent(typeof(Kinematic)) as Kinematic;
        if (myMovingTarget == null)
        {
            return base.getTargetPosition();
        }

        return target.transform.position + myMovingTarget.linear * predictionTime;
    }
}

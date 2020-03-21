using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : Arrive
{
    public GameObject[] path;
	float offset;
	int pathIndex;
	float targetRadius = 1f;
	
    // Start is called before the first frame update
    public override SteeringOutput getSteering()
	{
		if (target == null)
		{
			pathIndex = 0;
			
			target = path[pathIndex];

		}
		//Debug.Log(character);

		float distanceToTarget = (character.transform.position - target.transform.position).magnitude;
		if (distanceToTarget < targetRadius)
		{
			pathIndex++;
			if (pathIndex > path.Length-1)
			{
				pathIndex = 0;
			}
			target = path[pathIndex];
			//Debug.Log(target);
		}
		return base.getSteering();
	
	}
}

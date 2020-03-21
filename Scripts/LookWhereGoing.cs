using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereGoing: Align
{
   public SteeringOutput getSteering()
    {
		
		Vector3 velocity = character.linear;
		
		if(velocity.magnitude == 0)
		{
			return null;
		}
		float angle = Mathf.Atan2(velocity.x, velocity.z);
		angle *= Mathf.Rad2Deg;
		character.transform.eulerAngles = new Vector3(0, angle, 0);
		return base.getSteering();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    // Start is called before the first frame update
   //public Kinematic target;

   public SteeringOutput getSteering()
    {

		Vector3 directionToFace = target.transform.position - character.transform.position;
		
		if(directionToFace.magnitude == 0)
		{
			return null;
		}
		
		base.target = target;
		float angle = Mathf.Atan2(directionToFace.x, directionToFace.z);
		angle *= Mathf.Rad2Deg;
		base.target.transform.eulerAngles = new Vector3(0, angle, 0);
		return base.getSteering();
	} 
}

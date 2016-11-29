using UnityEngine;
using System.Collections;

public class Raycast3 : MonoBehaviour {
	public float distance3 = 0.3f;
    
	void  Update ()
    {
		RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            //Debug.DrawLine(transform.position, hit.point, Color.blue);
            distance3 = hit.distance;
        }
        else
            distance3 = 0.3f;
	}
}
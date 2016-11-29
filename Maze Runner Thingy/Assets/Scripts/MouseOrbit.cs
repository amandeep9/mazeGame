using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour {

	public Transform target;
	public float distance = 0.2f;
	private float xSpeed = 120.0f;
	private float ySpeed = 120.0f;

	private float yMinLimit = -40f;
	private float yMaxLimit = 80f;

	private float distanceMin = 0.2f;
	private float distanceMax = 0.3f;

	private Rigidbody rigidbody;

	float x = 0.0f;
	float y = 0.0f;

	float sensitivityX = 0.08f;
	float sensitivityY = 0.1f;

	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		rigidbody = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (rigidbody != null)
		{
			rigidbody.freezeRotation = true;
		}
	}

	void  Update (){

		distance = transform.parent.GetChild(1).gameObject.GetComponent<Raycast3>().distance3;

		//Setting maximum distance so the camera doesnt go too far
		if(distance > distanceMax){
			distance = distanceMax;
		}

		//Setting minimum distance so the camera doesnt go too close
		if (distance < distanceMin)
		{
			distance = distanceMin;
		}

	}

	void LateUpdate () 
	{
		if (target) 
		{
			x += Input.GetAxis("Mouse X") * xSpeed * sensitivityX;
			y -= Input.GetAxis("Mouse Y") * ySpeed * sensitivityY;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

			/*RaycastHit hit;
			if (Physics.Raycast (target.position, transform.position, out hit)) 
			{
				distance = hit.distance;
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }*/

			//Setting maximum distance so the camera doesnt go too far
			if (distance > distanceMax)
			{
				distance = distanceMax;
			}

			//Setting minimum distance so the camera doesnt go too close
			if (distance < distanceMin)
			{
				distance = distanceMin;
			}

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}

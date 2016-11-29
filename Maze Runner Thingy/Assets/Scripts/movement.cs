using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class movement : MonoBehaviour {

	public CharacterController MyController;
	public float speed = 3f;
	public Transform CameraTrans;
	public float jumpSpeed = 10f;
	public float gravStrength =5f;
	public float aerialSpeed =2f;
	bool canJump = false;
	bool onWall = false;
	float vertVelocity;
	Vector3 normal;
	Vector3 velocity;
	Vector3 groundedVel;

	Quaternion inputRotation;

	// Update is called once per frame
	void Update () 
	{
		Vector3 myVector = Vector3.zero;

		//get input from the player
		Vector3 input = Vector3.zero;
		input.x = Input.GetAxis("Horizontal");
		input.z = Input.GetAxis("Vertical");
		input.y = 0;
		input = Vector3.ClampMagnitude(input,1f);

		myVector = input;
		inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(CameraTrans.forward, Vector3.up),Vector3.up);

		if(MyController.isGrounded)
		{
			myVector = inputRotation * myVector;//rotate input relative to camera
			myVector*=speed; 
			//groundedVel = Vector3.ProjectOnPlane(velocity, Vector3.up);
		}
		else
		{
			myVector = inputRotation * myVector;//rotate input relative to camera
			myVector*=aerialSpeed;
			//myVector+= groundedVel;
		}
		myVector = Vector3.ClampMagnitude(myVector,speed);
		myVector=myVector*Time.deltaTime;

		vertVelocity -= gravStrength*Time.deltaTime;
		if(Input.GetButtonDown("Jump"))
		{
			//if(onWall)
				//groundedVel = Vector3.Reflect(velocity , normal).normalized*speed;
			//add jumpspeed to vert velocity
			if(canJump)
				vertVelocity += jumpSpeed;
		}
		myVector.y=vertVelocity*Time.deltaTime; //add speed to old speed

		//use input to move the character
		CollisionFlags flags = MyController.Move(myVector);
		velocity = myVector / Time.deltaTime;

		//use flags to find if character can jump
		if((flags & CollisionFlags.Below) !=0)
		{
			groundedVel = Vector3.ProjectOnPlane(velocity, Vector3.up);
			canJump=true;
			vertVelocity = -3f;
			onWall=false;
		}
		else if((flags & CollisionFlags.Sides)!=0)
		{
			canJump = true;
			onWall=true;
		}
		else
		{
			canJump=false;
			onWall=false;
		}

		if (Input.GetMouseButton(0))
			velocity=myVector*20;

	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		normal = hit.normal;
	}

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.name == "nextLevel")
		{
			SceneManager.LoadScene("MapGenerator");
		}
	}
}

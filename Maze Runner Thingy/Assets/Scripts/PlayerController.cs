using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
//using UnityEditor;

public class PlayerController : MonoBehaviour
{
	public RigidbodyConstraints constraints;
	Quaternion originalRotation;
	Rigidbody myRigidbody;
	GameObject flashlight;

	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody> ();
		flashlight = transform.GetChild (0).gameObject.transform.GetChild (0).gameObject;
	}

    void Update()
	{
		if (Input.GetKeyDown (KeyCode.F)) 
		{
			if (flashlight.activeSelf)
				flashlight.SetActive (false);
			else
				flashlight.SetActive (true);
		}

        var x1 = Input.GetAxis("Horizontal") * Time.deltaTime * 4.0f;
        var z1 = Input.GetAxis("Vertical") * Time.deltaTime * 4.0f;

        var CharacterRotation1 = transform.GetChild(0).transform.rotation;
        CharacterRotation1.x = 0;
        CharacterRotation1.z = 0;

        transform.rotation = CharacterRotation1;

        transform.Translate(0, 0, z1);
        transform.Translate(x1, 0, 0);

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "nextLevel")
        {
            SceneManager.LoadScene("MapGenerator");
        }
    }
}

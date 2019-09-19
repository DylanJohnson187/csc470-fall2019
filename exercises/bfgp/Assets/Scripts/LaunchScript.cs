using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScript : MonoBehaviour
{
	//This needs to be assigned a value in the Unity inspector. The "Rigidbody" component
	//tells Unity to simulate the GameObject that is attached to be use a simulated behavior
	public Rigidbody rigidbody;

	//This (also assigned via the Unity inpector interface) is the UI component that displays
	//the amount that we have charged. (in an unaesthetically pleasing way...)
	public Text chargeEnergyText;

	//This will ultimately scale the force from which the thing being launched is launched
	float chargeEnergy = 0;

	//This controls the rate that chargeEnergy is accumulated
	float chargeRate = 500;

	// Start is called before the first frame update
	void Start()
	{
		//Initialize the UI component that displays the chargeEnergy
		chargeEnergyText.text = "0";
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Space)) {
			//When space is held, accumulate energy
			chargeEnergy += chargeRate * Time.deltaTime;

			//Update the UI element that displays the chargeEnergy
			chargeEnergyText.text = chargeEnergy.ToString();
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			//Modify the rigidbody of the gameObject to use gravity
			//(NOTE: This requires that you assigned the rigidbody variable in the Unity inspector)
			rigidbody.useGravity = true;

			//Apply the launch force!
			rigidbody.AddForce(transform.forward * chargeEnergy);

			//If this game supported it, this would make it so the next launch would 
			//not carry with it the force of the previous charge.
			chargeEnergy = 0;
		}
	}
}

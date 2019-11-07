using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
	public bool selected = false;
	bool hover = false;

	public string name;
	public Sprite portrait;
	public float health;


	Color defautColor;
	public Color hoverColor;
	public Color selectedColor;

	CharacterController cc;
	public Vector3 destination;

	// NOTE: We set this reference in the prefab editor within the Unity editor.
	public Renderer rend;

	GameManager gm;

	// Start is called before the first frame update
	void Start()
	{
		health = Random.Range(40, 100);

		// This isn't the best way to get a reference to the GameManager component, but it
		// demonstrates a useful pattern where you can 1. get a reference to a gameObject using
		// GameObject.Find(), and 2. using the GetComponent() function to get a reference to
		// a component on a GameObject.
		GameObject gmObj = GameObject.Find("GameManagerObject");
		gm = gmObj.GetComponent<GameManager>();

		// Get a reference to the CharacterController component on the gameObject (i.e. unit)
		cc = gameObject.GetComponent<CharacterController>();

		// Initialize the destination to the current position, so units don't move when we 
		// press start.
		destination = transform.position;

		// Store the default color.
		defautColor = rend.material.color;
		// Set the initial color.
		setColorOnMouseState();

		// Give them each a random rotation so they're not all facing the same way.
		transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (destination != null && Vector3.Distance(destination, transform.position) > 0.5f) {
			// If we have a destination, rotate and move towards it.
			destination.y = transform.position.y;
			Vector3 vecToDest = (destination - transform.position).normalized;
			float step = 3 * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDest, step, 1);
			transform.rotation = Quaternion.LookRotation(newDir);

			cc.Move(transform.forward * 5 * Time.deltaTime);
		}
	}

	// This function sets the correct based on the hover and selected variables.
	public void setColorOnMouseState()
	{
		if (selected) {
			rend.material.color = selectedColor;
		} else if (hover) {
			rend.material.color = hoverColor;
		} else {
			rend.material.color = defautColor;
		}
	}


	// The following functions are called by Unity based on what the mouse is doing with
	// regards to the gameObject this script is attached to.
	private void OnMouseOver()
	{
		hover = true;
		setColorOnMouseState();

		gm.healthMeterObject.SetActive(true);
		gm.meterFG.fillAmount = health / 100;
		gm.healthMeterObject.transform.position = Camera.main.WorldToScreenPoint(
																		transform.position + Vector3.up * 2);
	}
	private void OnMouseExit()
	{
		gm.healthMeterObject.SetActive(false);

		hover = false;
		setColorOnMouseState();
	}
	private void OnMouseDown()
	{
		selected = !selected;
		if (selected) {
			gm.selectUnit(this);
		} else {
			gm.selectUnit(null);
		}
		setColorOnMouseState();
	}
}

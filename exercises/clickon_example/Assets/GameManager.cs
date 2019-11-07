using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	// This will hold a reference to whichever Unit was selected last.
	UnitScript selectedUnit;

	// References to a handful of UI elements.
	public GameObject talkBox;
	public Text talkText;
	public ToggleGroup actionSelectToggleGroup;
	public GameObject selectedPanel;
	public Text nameText;
	public Image portraitImage;

	// Set the fillAmount of this Image to a value between 0 and 1 to set the meter.
	public Image meterFG;
	// We will move this object around and turn it on and off (done in UnitScript OnMouse function).
	public GameObject healthMeterObject;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// Input.GetMouseButtonDown(0) is how you detect that the left mouse button has been clicked.
		//
		// The IsPointerOverGameObject makes sure the pointer is over the UI. In this case,
		// we don't want to register clicks over the UI when determining what unit is 
		// selected or deselected.
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			// Create a ray from the mouse position (in camera/ui space) to 3d world space.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// After the Raycast, 'hit' will store information about what the raycast hit.
			RaycastHit hit;
			// The line below actually performs the "raycast". This will 'shoot' a line from the
			// mouse position into the world, and it if hits something marked with the layer 'ground', 
			// return true.
			if (Physics.Raycast(ray, out hit, 9999)) {
				// Check to see if the thing the raycast hit was marked with the layer 'ground'.
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground")) {
					// If so, set the destination of the selectedUnit to the point on the ground
					// that the raycast hit.
					if (selectedUnit != null) {
						selectedUnit.destination = hit.point;
					}
				}
			} else {
				// If we got here, it means that the raycast didn't hit anything, so let's deselect.
				if (selectedUnit != null) {
					selectedUnit.selected = false;
					selectedUnit.setColorOnMouseState();
					selectedUnit = null;

					updateSelectedPanelUI();
				}
			}
		}
	}

	public void selectUnit(UnitScript unit)
	{
		// If we have selected something previously, unselect it and update the color.
		if (selectedUnit != null) {
			selectedUnit.selected = false;
			selectedUnit.setColorOnMouseState();
		}

		// Set selected unit to the one we just passed in.
		selectedUnit = unit;

		if (selectedUnit != null) {
			// If there is a selected unit, update its color.
			selectedUnit.selected = true;
			selectedUnit.setColorOnMouseState();
		}

		updateSelectedPanelUI();
	}

	// This function updates the UI elements based on what was clicked on.
	void updateSelectedPanelUI()
	{
		// Only update the UI is there is a unit selected.
		if (selectedUnit != null) {
			nameText.text = selectedUnit.name;
			portraitImage.sprite = selectedUnit.portrait;
			selectedPanel.SetActive(true);
		} else {
			// If there is no selected unit, turn the panel off.
			selectedPanel.SetActive(false);
		}
	}

	// This function is called by the EventSystem when the player clicks on the PerformActionButton.
	public void TakeAction()
	{
		// Compute the screen position 2 units above the unit and place the talkBox.
		Vector3 pos = selectedUnit.transform.position + Vector3.up * 2;
		pos = Camera.main.WorldToScreenPoint(pos);
		talkBox.transform.position = pos;

		// Figure out which toggle button is selected in the action select toggleGroup
		// and store the text value of the button in a string.
		IEnumerable<Toggle> activeToggles = actionSelectToggleGroup.ActiveToggles();
		string action = "";
		foreach (Toggle t in activeToggles) {
			if (t.IsActive()) {
				action = t.gameObject.GetComponentInChildren<Text>().text;
			}
		}

		// This registers a function with Unity's coroutine system (see notes above the function definition)
		StartCoroutine(displayTalkBoxMessages(new string[] { action, "I'm done talking", "One more thing...", "Nevermind." }));
	}

	// This type of function is registered with Unity's coroutine system. It doesn't run like
	// other functions (from top to bottom), but instead each update cycle is first
	// ran until some "yield return..." command is reached. After that point, the function
	// is "checked in" with automatically starting from the line after the "yield 
	// return...". This happens until the end of the function is reached.
	//
	// This particular coroutine recieves an array of string messages and displays each
	// until the mouse is pressed.
	IEnumerator displayTalkBoxMessages(string[] messages)
	{
		talkBox.SetActive(true);
		for (int i = 0; i < messages.Length; i++) {
			talkText.text = messages[i];

			// Wait for the mouse to be pressed
			while (!Input.GetMouseButtonDown(0)) {
				// Tell the coroutine system that we are done for this update cycle.
				yield return null;
			}

			// If we get here, it means that the mouse was just pressed. Tell the coroutine
			// system that we are done for this update cycle.
			yield return null;
		}
		talkBox.SetActive(false);
	}
}
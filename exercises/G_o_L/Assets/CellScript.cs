using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
	public bool alive = false;
	public bool nextAlive;
	bool prevAlive;
    public bool spawn;
    public bool goal;
	public int x = -1;
	public int y = -1;

	Renderer renderer;

	// Start is called before the first frame update
	void Start()
    {
		prevAlive = alive;
	}

    // Update is called once per frame
    void Update()
    {
		if (prevAlive != alive) {
			updateColor();
		}

		prevAlive = alive;
	}

	public void updateColor()
	{
		if (renderer == null) {
			renderer = gameObject.GetComponent<Renderer>();
		}

        if (this.spawn)
        {
            renderer.material.color = Color.blue;
        }
        else if (this.goal)
        {
            renderer.material.color = Color.green;
        }
		else if (this.alive && !this.spawn) {
			renderer.material.color = Color.magenta;
		} else {
			renderer.material.color = Color.yellow;
		}
	}

	private void OnMouseDown()
	{
		alive = !alive;
	}
}

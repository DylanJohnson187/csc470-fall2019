using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camfollow : MonoBehaviour
{
    // Start is called before the first frame update
    float cameraVel;

    void Start()
    {
        GameObject go;
        go = GameObject.FindGameObjectWithTag("Player");
        cameraVel = go.GetComponent<PlayerScript>().getSpeed();
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, cameraVel);
        //Currently sets camera velocity equal to player velocity
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go;
        go = GameObject.FindGameObjectWithTag("Player");
        cameraVel = go.GetComponent<PlayerScript>().getSpeed();
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, cameraVel);
        float playerz = go.GetComponent<Transform>().position.z;
        //Camera.main.transform.position.z = playerz - 5;
    }
}

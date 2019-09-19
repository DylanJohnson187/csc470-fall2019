using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdoll_script : MonoBehaviour
{
    // Start is called before the first frame update

    //can also reference in game objects through 
    //public game object <object name>

    float speed = 2.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* create a vector 
         * such asvector 3 vet to objec = object name . transform.position - gameobject (this is call to the ragdoll).transform.position;
         * 
         * vect to object = vect to object . normalize
         * 
         * however will need to look at the object you want to head towards so it looks real
         * 
         * this can be done with the look at method
         */


        transform.position = transform.position - transform.forward * speed * Time.deltaTime;
    }
}

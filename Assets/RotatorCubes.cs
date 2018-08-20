using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RotatorCubes : MonoBehaviour {

    public
        bool rotating = true;
	
	// Update is called once per frame
	void Update () {
        if(rotating)
            gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 20f);
	}

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadCheck : MonoBehaviour {
    public GameObject leftHand;
    public GameObject rightHand;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.tag == "DeadPlayer")
        {
            leftHand.transform.localPosition = new Vector3(-0.25f, 1.68f, 0.034f);
            rightHand.transform.localPosition = new Vector3(0.25f, 1.68f, 0.034f);
        }
	}
}

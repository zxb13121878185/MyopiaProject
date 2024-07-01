using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour {

    public GameObject rayObj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D tempHitObj = Physics2D.Raycast(tempRay.origin, tempRay.direction);
        if (null!=tempHitObj.collider)
        {
            Debug.Log(tempHitObj.collider.name);
        }
	}

    public void TestPoint()
    {
        Debug.Log("SSSSSSSSSSSSSSSSSSSSSSSSS");
    }
}

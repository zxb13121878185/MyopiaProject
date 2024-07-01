using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3D_TextScoreInfo : MonoBehaviour {

    public Vector3 offsetRot;

    [SerializeField] float startScale;

    private int scoreVaule;
    private bool isInitSucced = false;
    private GameObject camObj;
    private TextMesh curTextMesh;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInitSucced) return;

        transform.LookAt(Camera_UnBoundedManage.M_Instance.M_SubObjCamObtainRT.transform.position);
        Vector3 tempRot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(tempRot.x+offsetRot.x, tempRot.y+offsetRot.y, tempRot.z +offsetRot.z);
	}

    public void DoTweenAnimComplete_Fade()
    {
        Destroy(gameObject);
    }

    public void Init(Vector3 startPos,int scoreV)
    {
        scoreVaule = scoreV;
        curTextMesh = GetComponent<TextMesh>();
        curTextMesh.text = "+" + scoreVaule;

        transform.position = startPos;
        transform.localScale = Vector3.one * startScale;

        isInitSucced = true;
    }
}

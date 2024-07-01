using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BulletEntity : MonoBehaviour {

    private float moveSpeed;
    private bool isInitSucced = false;
    private bool isMove = true;
    int targetLayer;
    private bool isAutoDestroy = true;
    private Tweener curTweenScale;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInitSucced) return;
    }
    private void FixedUpdate()
    {
        if (!isInitSucced) return;

        if (isMove)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
    //子弹撞到敌人后敌人消失
    void OnCollisionEnter(Collision collision)
    {
      
        if (null != collision.gameObject&&collision.gameObject.layer==targetLayer)
        {
           // Debug.Log(collision.gameObject.name);
            isMove = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
            FlockEntity tempHitObj = collision.gameObject.GetComponent<FlockEntity>();
            if (null!= tempHitObj)
            {
                tempHitObj.BeHit(this);
                isAutoDestroy = false;
                //停止动画
                curTweenScale.Kill(false);
            }
        }

    }
    private IEnumerator AutoDestroyObj(float destroryTime)
    {
        float tempTimeCounter = 0;
        while(isAutoDestroy)
        {
            tempTimeCounter += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            if (tempTimeCounter >= destroryTime)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
    
    public void Init(float speed,float destroyTime,string layerMaskName,float priScale)
    {
        isInitSucced = true;
        moveSpeed = speed;
        targetLayer = LayerMask.NameToLayer(layerMaskName);
        StartCoroutine(AutoDestroyObj(destroyTime));
        transform.localScale = Vector3.one * priScale;
        curTweenScale = transform.DOScale(Vector3.one, 1);
    }
}

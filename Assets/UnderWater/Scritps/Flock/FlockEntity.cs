using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockEntity : MonoBehaviour {

    /// <summary>
    /// 编号
    /// </summary>
    public int M_FlockID
    {
        get
        {
            return flockId;
        }
    }

    /// <summary>
    /// 气泡的锚点
    /// </summary>
    [SerializeField] GameObject bubbleAnchorObj;
    [SerializeField] string moveAnimName = "Motion";
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] int scoreValue = 1;
    private float rotationSpeed = 4.0f;
    Vector3 avarageHeading;
    Vector3 avaragePosition;
    float neighbourDistance = 1.0f;
    //public bool startUpdate = false;
    bool turning = false;
    private Vector3 parentPos;
    [SerializeField]
    private Tweener doTweenAnim;
    private int flockId;
    private Animation curAnim;
    private bool isInitSucced = false;

    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        
    }

    private void Update()
    {
        if (!isInitSucced) return;

        //如果距离母物体的距离超过了指定的范围就设置开始掉头为true;
        float tempDis2ParentPos = Vector3.Distance(transform.position, parentPos);
        turning = (Global_FlockManage.M_Instance.M_SizeArea <= tempDis2ParentPos);

        if (turning)
        {
            Vector3 tempDir = parentPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tempDir), rotationSpeed * Time.deltaTime);
            moveSpeed = Random.Range(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyAIFlockRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// 应用集群规则，包括不碰撞、重叠等
    /// </summary>
    private void ApplyAIFlockRules()
    {
        Vector3 tempVcentre = parentPos;
        Vector3 tempVavoid = parentPos;

        float tempGSpeed = 0.1f;
        float tempDis;
        int tempGroupSize = 0;
        for (int i = 0; i < Global_FlockManage.M_Instance.M_ListShowAllFlockObj.Count; i++)
        {
            FlockEntity tempAnotherObj = Global_FlockManage.M_Instance.M_ListShowAllFlockObj[i];
            //如果是其他物体，那么应该规避它
            if (this != tempAnotherObj)
            {
                tempDis = Vector3.Distance(tempAnotherObj.transform.position, transform.position);
                //找到所有小于预定距离的物体，然后计算它们所有的位置与本物体的位置的方向向量
                //所有的向量相加就可以知道它最后应该朝什么方向向量走就可以规避所有距离小于指定值的方向
                if (tempDis <= neighbourDistance)
                {
                    //计算所有其他范围内鱼的位置，以便计算所有鱼的中心点位置
                    tempVcentre += tempAnotherObj.transform.position;
                    tempGroupSize++;
                    //计算相反的向量，作为规避向量
                    tempVavoid = tempVavoid + transform.position - tempAnotherObj.transform.position;
                    tempGSpeed = tempGSpeed + tempAnotherObj.moveSpeed;
                }
            }
        }
        if(0<tempGroupSize)
        {
            tempVcentre = tempVcentre / tempGroupSize + (Global_FlockManage.M_Instance.M_GoalPos - transform.position);
            moveSpeed = tempGSpeed / tempGroupSize;
            Vector3 tempDir = (tempVcentre + tempVavoid) - transform.position;
            if(Vector3.zero!=tempDir)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tempDir), rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void BeHit(BulletEntity bulletObj)
    {
        //让子弹附着在物体上
        bulletObj.transform.SetParent(bubbleAnchorObj.transform);
        bulletObj.transform.localPosition = Vector3.zero;
        bulletObj.transform.localScale = Vector3.one;
        //让该物体不在能产生碰撞//防止子弹与该物体一直作用
        GetComponent<BoxCollider>().isTrigger = true;

        //激活捕捉的动画
        Vector3 tempShakeRot = new Vector3(30, 30, 30);
        doTweenAnim = transform.DOShakeRotation(1.0f, tempShakeRot,30);
        doTweenAnim.SetLoops(2, LoopType.Incremental).OnComplete(DoTweenAnimComplete_ShakeRot);

        //设置当前鱼的动画
        curAnim[moveAnimName].speed = 5.0f;
        

    }
    /// <summary>
    /// 动画完成之后的处理方式
    /// </summary>
    private void DoTweenAnimComplete_ShakeRot()
    {
        //销毁之后创建加分的效果
        UI3D_TextScoreInfo tempScoreInfoObj = Instantiate(SpwanBulletManage.M_Instance.M_Prefab3DUI_TextScoreInfo);
        tempScoreInfoObj.Init(bubbleAnchorObj.transform.position, scoreValue);
        //并且销毁物体
        Global_FlockManage.M_Instance.DeleteFlockObj(flockId);
        Global_Manage.M_Instance.PlaySound_Score();
    }
    public void Init(int id)
    {
        isInitSucced = true;

        flockId = id;
        parentPos = Global_FlockManage.M_Instance.transform.position;
        moveSpeed = Random.Range(1f, 2);
        curAnim = GetComponent<Animation>();
        curAnim[moveAnimName].speed = Random.Range(0.5f, 2.0f);
    }
}

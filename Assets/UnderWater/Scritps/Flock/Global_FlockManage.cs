using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_FlockManage : BaseGameObj {

    /// <summary>
    /// 集群运动的的范围
    /// </summary>
    public int M_SizeArea
    {
        get
        {
            return sizeArea;
        }
    }
    /// <summary>
    /// 显示的集群物体的数量
    /// </summary>
    public int M_NumShowFlockObj
    {
        get
        {
            return numShowFlockObj;
        }
    }
    /// <summary>
    /// 所有显示的集群的物体
    /// </summary>
    public List<FlockEntity> M_ListShowAllFlockObj
    {
        get
        {
            return listAllShowFlockObjs;
        }
    }
    /// <summary>
    /// 集群的目标位置
    /// </summary>
    public Vector3 M_GoalPos
    {
        get
        {
            return goalPos;
        }
    }
    public static Global_FlockManage M_Instance
    {
        get
        {
            if(null==_instance)
            {
                _instance = FindObjectOfType<Global_FlockManage>();
            }
            return _instance;
        }
    }

    [SerializeField] FlockEntity[] prefabFlockObj;
    [SerializeField] int sizeArea;
    [SerializeField] int numShowFlockObj;
    private List<FlockEntity> listAllShowFlockObjs = new List<FlockEntity>();
    private Vector3 goalPos = Vector3.zero;

    private static Global_FlockManage _instance;
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        ///随机时间改变目标位置，并且目标位置为随机值
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = GetRandPos();
        }
    }

    // Use this for initialization
    void Start () {

        Init();
	}
    private void CreateFlocks(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //随机的位置

            Vector3 tempPos = goalPos + GetRandPos();
            //Debug.Log(tempPos);
            //创建随机物体
            int tempIndex = Random.Range(0, prefabFlockObj.Length);
            FlockEntity tempFE = Instantiate(prefabFlockObj[tempIndex], tempPos, Quaternion.identity,transform);
            tempFE.Init(listAllShowFlockObjs.Count);
            listAllShowFlockObjs.Add(tempFE);
        }
    }
    /// <summary>
    /// 根据区域大小获取随机的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandPos()
    {
        Vector3 tempVec = Vector3.zero;
        float tempX = Random.Range(-sizeArea, sizeArea);
        float tempY = Random.Range(0, sizeArea);
        float tempZ = Random.Range(-sizeArea, sizeArea);
        tempVec = new Vector3(tempX, tempY, tempZ);
        return tempVec;
    }
    public bool DeleteFlockObj(int id)
    {
        int tempIndex = listAllShowFlockObjs.FindIndex(p => p.M_FlockID == id);
        if(-1==tempIndex)
        {
            Debug.Log("找不到id为"+id+"的物体");
            return false;
        }
        FlockEntity tempFE = listAllShowFlockObjs[tempIndex];
        Destroy(tempFE.gameObject);
        listAllShowFlockObjs.RemoveAt(tempIndex);

        if(listAllShowFlockObjs.Count<numShowFlockObj/2)
        {
            CreateFlocks(numShowFlockObj/2);
            Debug.Log("重新生成！");
        }
        return true;
    }
    public override void Init()
    {
        base.Init();
        goalPos = transform.position;
        CreateFlocks(numShowFlockObj);
    }
}

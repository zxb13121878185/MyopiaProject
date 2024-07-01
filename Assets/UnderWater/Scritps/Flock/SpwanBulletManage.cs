using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpwanBulletManage : BaseGameObj
{

    public static SpwanBulletManage M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<SpwanBulletManage>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 3DUI，加分效果text
    /// </summary>
    public UI3D_TextScoreInfo M_Prefab3DUI_TextScoreInfo
    {
        get
        {
            return prefabTexScoreInfo;
        }
    }
    [SerializeField] UI3D_TextScoreInfo prefabTexScoreInfo;

    private static SpwanBulletManage _instance;
    [SerializeField] BulletEntity prefabBE;
    [SerializeField] GameObject fireObj;
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] float destroyTime = 5;
    /// <summary>
    /// 每这么多秒发射一次
    /// </summary>
    [SerializeField] float fireRate;
    [SerializeField] float priScale;
    private float nextFireTime;
    private AudioSource curAudioSource;
    /// <summary>
    /// 子弹作用在哪个层级的名字
    /// </summary>
    [SerializeField] string bulletEffectLayerName;
    bool isRightTriggerPress;
    bool isLeftTriggerPress;
    // Use this for initialization
    void Start()
    {
        Init();
    }
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out isRightTriggerPress);
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out isLeftTriggerPress);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
        if (isLeftTriggerPress || isRightTriggerPress)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            BulletEntity tempBE = Instantiate(prefabBE, fireObj.transform.position, fireObj.transform.rotation, transform);
            tempBE.Init(bulletSpeed, destroyTime, bulletEffectLayerName, priScale);
            curAudioSource.Play();
        }
    }
    public override void Init()
    {
        base.Init();
        curAudioSource = GetComponent<AudioSource>();
    }
}

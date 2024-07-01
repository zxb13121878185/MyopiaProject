using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI2D_ToolTip : MonoBehaviour
{
    public string TipsIndex;//显示的文本索引
    private EventTrigger myET;
    // Start is called before the first frame update
    void Start()
    {
        myET = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        myET.triggers.Add(entry1);
        //这里一定要一个方法添加一次
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        myET.triggers.Add(entry2);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        UI2D_PanelToolTips.M_Instance.Hide();
    }
    private void OnPointerExitDelegate(PointerEventData data)
    {
        UI2D_PanelToolTips.M_Instance.Hide();
        //   Debug.Log("Exit");
    }

    private void OnPointerEnterDelegate(PointerEventData data)
    {
        UI2D_PanelToolTips.M_Instance.Show(TipsIndex, transform.position);
        //  Debug.Log("Enter");
    }

}


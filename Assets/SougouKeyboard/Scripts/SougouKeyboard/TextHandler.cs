using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler,IPointerEnterHandler
{
    public static bool IsFocusOn = false;//焦点是否在该组件上
    [SerializeField] private ImeManager imeManager;
    [SerializeField] private SGImeInputType sgImeInputType;
    [SerializeField] private SGImeTextType sgImeTextType;
    [SerializeField] private ImeDelegateImpl_kbd imeDelegateImpl_Kbd;

    private InputField inputField;
    void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.shouldHideMobileInput = true;
    }

    // Update is called once per frame
    // OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {
        if (null != eventData.lastPress && eventData.lastPress == this)
        {
            //if (imeManager != null)
            //{
            //    imeManager.Hide();
            //}
        }
    }
    private void OnMouseDown(MouseDownEvent evt)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //  Debug.Log("last:"+eventData.lastPress.name+ "rawPointerPress:" + eventData.rawPointerPress.name);
        IsFocusOn = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //  LogEvent("click text", eventData);
        if (imeManager != null)
        {
            imeManager.Show(sgImeInputType, sgImeTextType);
        }

        if (imeDelegateImpl_Kbd != null && inputField != null)
        {
            imeDelegateImpl_Kbd.inputField = inputField;
            //动态设置3D键盘的位置为当前鼠标点击输入框下方指定位置
            Vector3 tempPos = imeDelegateImpl_Kbd.transform.position;
            Vector3 tempScreenPos = Camera.main.WorldToScreenPoint(tempPos);
            Vector3 tempMousePos = new Vector3(eventData.position.x, eventData.position.y, tempScreenPos.z);
            //将鼠标点击坐标转化为世界坐标
            Vector3 tempPointerPos = Camera.main.ScreenToWorldPoint(tempMousePos); //屏幕坐标转世界坐标
                                                                                   //  Debug.Log("eventPos:" + eventData.position + " pos:" + tempPointerPos);
            imeDelegateImpl_Kbd.transform.position = new Vector3(transform.position.x, tempPointerPos.y - Global_Manage.M_HeigitKeyboard, transform.position.z+0.1f);

        }
    }

    private void LogEvent(string prefix, PointerEventData eventData)
    {
        //  Debug.Log(prefix + ": " + eventData.pointerCurrentRaycast.gameObject.name + " x=" + eventData.position.x + ",y=" + eventData.position.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsFocusOn = true;
    }
}

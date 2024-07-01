using UnityEngine;
using UnityEngine.UI;


public class Canvas : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text content;

    public void ShowPassword()
    {
        if (content != null)
        {
            content.text = inputField.text;
        }
    }
}

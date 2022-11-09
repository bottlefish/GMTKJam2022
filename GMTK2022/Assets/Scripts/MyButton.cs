using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyButton : Button
{
    bool _isClicked;
    // Button is Pressed
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _isClicked = true;
        //gameObject.GetComponentInChildren<Text>().text = "Pressed";
    }

    // Button is released
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _isClicked = false;
        //gameObject.GetComponentInChildren<Text>().text = "Released";

    }
    public bool GetIsClicked()
    {
        return _isClicked;
    }
}

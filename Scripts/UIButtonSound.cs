using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("hoverenter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("buttonclick");
    }
}

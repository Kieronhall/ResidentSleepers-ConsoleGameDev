using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollControll : MonoBehaviour, ISelectHandler
{
    private ScrollRect scrollRect;
    private float scrollOffset = 1;
    void Start()
    {
        // from here
        scrollRect = GetComponentInParent<ScrollRect>();
        int childCount = scrollRect.content.transform.childCount - 1;
        int childIndex = transform.GetSiblingIndex();

        childIndex = childIndex < ((float)childCount / 2) ? childIndex - 1 : childIndex;
        scrollOffset = 1 - ((float)childIndex / childCount);
        // to here https://www.youtube.com/watch?v=P8hx343kIGg
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar.value = scrollOffset;
        }
    }

}

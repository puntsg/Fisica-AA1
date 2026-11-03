using UnityEngine;
using UnityEngine.Events;

public class Dragger : MonoBehaviour
{
    public Vector2 dragValue = Vector2.zero;
    public Vector2 startDragPos = Vector2.zero;
    public Vector2 endDragPos = Vector2.zero;

    public UnityEvent<Vector2> onStartDrag, onDrag, onFinishDrag;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startDragPos = Input.mousePosition;
            onStartDrag.Invoke(startDragPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endDragPos = Input.mousePosition;
            dragValue = startDragPos - endDragPos;
            onFinishDrag.Invoke(dragValue);
        }
        else if (Input.GetMouseButton(0))
        {
            dragValue = startDragPos - (Vector2)Input.mousePosition;
            onDrag.Invoke(dragValue);
        }
    }
}

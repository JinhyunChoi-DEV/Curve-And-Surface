using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private LineDraw lineDraw;
    [SerializeField] private GameObject activeObject;

    private bool dragging = false;

    void Start()
    {
        activeObject.SetActive(false);
        dragging = false;
    }

    void Update()
    {
        if (dragging)
            UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (mousePosition.x < min.x || mousePosition.y < min.y)
            return;

        if (mousePosition.x > max.x || mousePosition.y > max.y)
            return;

        gameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        lineDraw.RenderLine();
    }

    void OnMouseUp()
    {
        dragging = false;
        activeObject.SetActive(false);
    }

    void OnMouseDrag()
    {
        dragging = true;
        activeObject.SetActive(true);
    }

    void OnMouseOver()
    {
        activeObject.SetActive(true);
    }

    void OnMouseExit()
    {
        //Debug.Log("Exit");
        if (!dragging)
            activeObject.SetActive(false);
    }
}

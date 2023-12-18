using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PointLocker locker;
    [SerializeField] private DrawDeCasteljauGraph graphDraw;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private RectTransform coordinateRect;
    [SerializeField] private GameObject activeObject;

    private float factorValue;
    private bool isEnter = false;
    private bool isDragging = false;
    private Vector2 validMax;
    private Vector2 validMin;

    public float GetDegreeValue()
    {
        return factorValue;
    }

    void Awake()
    {
        activeObject.SetActive(false);

        SetValidArea();

        // Init Start YPos
        var startY = coordinateRect.rect.max.y / 3;
        var localPos = gameObject.transform.localPosition;
        var newPos = new Vector3(localPos.x, startY, localPos.z);
        gameObject.transform.localPosition = newPos;
        factorValue = 1.0f;
        valueText.text = factorValue.ToString("F2");
    }

    void Update()
    {
        UpdatePointObject();
        UpdateTextValue();

        if (isDragging)
        {
            graphDraw.Draw();
        }
    }

    void UpdatePointObject()
    {
        if (isEnter && !locker.IsLocked)
        {
            if (Input.GetMouseButton(0))
            {
                isDragging = true;
                locker.Set(gameObject);
            }
            else
            {
                isDragging = false;
                locker.Release();
            }

            activeObject.SetActive(true);
        }
        else
        {
            if (!isDragging)
                activeObject.SetActive(false);
        }

        if (isDragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                activeObject.SetActive(false);
                locker.Release();
            }
            else if (locker.IsMe(gameObject))
            {
                Vector3 currentPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

                if (currentPos.y >= validMin.y && currentPos.y <= validMax.y)
                {
                    var newPos = new Vector3(transform.position.x, currentPos.y, transform.position.z);
                    gameObject.transform.position = newPos;
                }
            }
        }
    }

    void UpdateTextValue()
    {
        var maxY = coordinateRect.rect.max.y;
        var currentY = gameObject.transform.localPosition.y;
        var t = (currentY + maxY) / (maxY * 2);

        var result = Mathf.Lerp(-3, 3, t);
        factorValue = result;
        valueText.text = result.ToString("F2");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }

    void SetValidArea()
    {
        var origin = gameObject.transform.position;

        var rect = (RectTransform)gameObject.transform;
        rect.anchoredPosition = coordinateRect.rect.max;
        var maxPoint = Camera.main.WorldToScreenPoint(rect.position);
        validMax = Camera.main.ScreenToWorldPoint(maxPoint);
        validMax.y += 0.5f;

        rect.anchoredPosition = coordinateRect.rect.min;
        var minPoint = Camera.main.WorldToScreenPoint(rect.position);
        validMin = Camera.main.ScreenToWorldPoint(minPoint);
        validMin.y += -0.5f;

        gameObject.transform.position = origin;
    }
}

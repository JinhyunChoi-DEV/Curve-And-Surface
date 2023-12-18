using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Draw2DCoordinate : MonoBehaviour
{
    [SerializeField] private TMP_Text verticalValueText;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private GameObject lineObjectRoot;
    [SerializeField] private GameObject textObjectRoot;
    [SerializeField] private RectTransform coordinateBaseRect;
    [SerializeField] private RectTransform positionBaseRect;

    void Start()
    {
        verticalValueText.gameObject.SetActive(false);

        DrawBasisLine();
        DrawCoordinateLine();
    }

    void DrawBasisLine()
    {
        Color color = new Color(0.3f, 0.3f, 0.3f);
        float lineWidth = 0.02f;

        //Draw Horizontal Basis
        var pos = coordinateBaseRect.rect.min;
        pos.y = 0;
        Vector2 start = GetWorldSpace(pos);

        pos = coordinateBaseRect.rect.max;
        pos.y = 0;
        Vector2 end = GetWorldSpace(pos);
        CreateLine(start, end, lineWidth, color);

        //Draw Vertical Basis
        pos = coordinateBaseRect.rect.min;
        start = GetWorldSpace(pos);

        pos = coordinateBaseRect.rect.max;
        pos.x = coordinateBaseRect.rect.min.x;
        end = GetWorldSpace(pos);
        CreateLine(start, end, lineWidth, color);

        //Draw Vertical Value Text With Offset Line
        float factor = coordinateBaseRect.rect.max.y / 6;
        var offset = new Vector2(-15f, 0f);
        var offsetLineEnd = coordinateBaseRect.rect.min;
        var offsetLineStart = offsetLineEnd + offset;

        for (int i = -6; i <= 6; ++i)
        {
            var tempStart = offsetLineStart;
            tempStart.y = factor * i;
            start = GetWorldSpace(tempStart);

            var tempEnd = offsetLineEnd;
            tempEnd.y = factor * i;
            end = GetWorldSpace(tempEnd);

            var textOffsetPos = tempStart;
            textOffsetPos.x -= 10;
            var value = i * 0.5f;
            CreateLineWithValueText(start, end, lineWidth, color, value, textOffsetPos);
        }
    }

    void DrawCoordinateLine()
    {
        Color color = new Color(0.7f, 0.7f, 0.7f);
        float lineWidth = 0.02f;

        Vector2 start;
        Vector2 end;
        float factor = coordinateBaseRect.rect.max.y / 6;

        for (int i = -6; i <= 6; ++i)
        {
            if (i == 0)
                continue;

            start.x = coordinateBaseRect.rect.min.x;
            end.x = coordinateBaseRect.rect.max.x;

            start.y = factor * i;
            end.y = factor * i;

            start = GetWorldSpace(start);
            end = GetWorldSpace(end);
            CreateLine(start, end, lineWidth, color);
        }
    }

    Vector3 GetWorldSpace(Vector2 targetPos)
    {
        positionBaseRect.anchoredPosition = targetPos;

        var screenPoint = Camera.main.WorldToScreenPoint(positionBaseRect.position);
        var result = Camera.main.ScreenToWorldPoint(screenPoint);

        result.z = 0;
        return result;
    }

    void CreateLineWithValueText(Vector2 p1, Vector2 p2, float lineWidth, Color color, float textValue, Vector2 textOffsetPosition)
    {
        CreateLine(p1, p2, lineWidth, color);

        TMP_Text valueText = Instantiate(verticalValueText);
        valueText.color = color;
        valueText.text = textValue.ToString("F1");
        valueText.gameObject.SetActive(true);
        var rectTransform = (RectTransform)valueText.transform;
        rectTransform.SetParent(textObjectRoot.transform);
        rectTransform.anchoredPosition = textOffsetPosition;
        rectTransform.localScale = Vector3.one;
    }

    void CreateLine(Vector2 p1, Vector2 p2, float lineWidth, Color color)
    {
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        line.transform.SetParent(lineObjectRoot.transform);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.material = lineMaterial;

        lineRenderer.SetPosition(0, new Vector3(p1.x, p1.y, 30));
        lineRenderer.SetPosition(1, new Vector3(p2.x, p2.y, 30));
    }

}

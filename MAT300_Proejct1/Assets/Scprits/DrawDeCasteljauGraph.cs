using UnityEngine;

public class DrawDeCasteljauGraph : MonoBehaviour
{
    [SerializeField] private RectTransform coordinateRect;
    [SerializeField] private RectTransform positionRect;
    [SerializeField] private DrawPoints points;
    [SerializeField] private LineRenderer lineRender;

    public bool IsBB_form;
    private int pointCount = 100;
    private const int maxPoint = 21;
    private int[,] pascalCombination;
    private float[,] NLIfactors;

    void Awake()
    {
        IsBB_form = true;

        SetLineRenderProperties();
        pascalCombination = new int[maxPoint, maxPoint];
        NLIfactors = new float[maxPoint, maxPoint];

        for (int i = 0; i < maxPoint; ++i)
        {
            pascalCombination[i, 0] = 1;
        }

        for (int i = 1; i < maxPoint; ++i)
        {
            for (int j = 1; j < maxPoint; ++j)
            {
                pascalCombination[i, j] = pascalCombination[i - 1, j - 1] + pascalCombination[i - 1, j];
            }
        }
    }

    public void Draw()
    {
        if (IsBB_form)
            DrawBBForm();
        else
            DrawNLI();
    }

    public void ChangeDrawMethod()
    {
        IsBB_form = !IsBB_form;
        Draw();
    }

    private void DrawNLI()
    {
        int degree = points.Degree;
        for (int i = 0; i <= degree; ++i)
        {
            NLIfactors[0, i] = points.GetPointsDegreeValue(i);
        }

        for (int i = 0; i <= pointCount; ++i)
        {
            float t = (float)i / pointCount;
            UpdateNLIFactor(degree, t);
            float yValue = NLIfactors[degree, 0];

            UpdatePosition(yValue, t, i);
        }
    }

    private void DrawBBForm()
    {
        for (int i = 0; i <= pointCount; ++i)
        {
            float t = (float)i / pointCount;
            float yValue = CalculateBBform(t);

            UpdatePosition(yValue, t, i);
        }
    }

    private void UpdatePosition(float yPos, float t, int i)
    {
        var max = coordinateRect.rect.max;
        var min = coordinateRect.rect.min;
        var tValueForY = (yPos + 3.0f) / 6.0f;

        var screenXPos = Mathf.Lerp(min.x, max.x, t);
        var screenYPos = Mathf.Lerp(min.y, max.y, tValueForY);
        positionRect.anchoredPosition = new Vector2(screenXPos, screenYPos);
        var pos = Camera.main.ScreenToWorldPoint(positionRect.transform.position);
        lineRender.SetPosition(i, new Vector3(pos.x, pos.y, 20));
    }

    private void SetLineRenderProperties()
    {
        lineRender.startWidth = 0.02f;
        lineRender.endWidth = 0.02f;
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.red;
        lineRender.positionCount = pointCount + 1;
    }

    private void UpdateNLIFactor(int d, float t)
    {
        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j <= d - i; ++j)
            {
                var a1 = NLIfactors[i - 1, j];
                var a2 = NLIfactors[i - 1, j + 1];

                NLIfactors[i, j] = (1.0f - t) * a1 + t * a2;
            }
        }
    }

    private float CalculateBBform(float t)
    {
        float result = 0.0f;

        for (int k = 0; k <= points.Degree; ++k)
        {
            var factor = points.GetPointsDegreeValue(k);
            int combination = pascalCombination[points.Degree, k];
            result += factor * combination * Mathf.Pow((1.0f - t), points.Degree - k) * Mathf.Pow(t, k);
        }

        return result;
    }
}

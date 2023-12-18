using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private GameObject temp;
    [SerializeField] private DataContainer data;
    [SerializeField] private LineRenderer curveLine;
    [SerializeField] private ShellLineRender shellLine;
    [SerializeField] private LineRenderer outsideLine;

    private List<Vector2> newPoints;
    private List<Vector2> shellLinePoints;
    private List<Vector2> midDivisonLinePoints;
    private Vector2[,] NLIValues;
    private Vector2[,] NLI_Shells;
    private Vector2[,] MidPointsValues;
    private int[,] pascalCombination;
    private int linePointCounts = 100;
    private int midPointIterationCount = 5;

    public void Clear()
    {
        curveLine.gameObject.SetActive(false);
        outsideLine.gameObject.SetActive(false);
        shellLine.Clear();
    }

    public void RenderLine()
    {
        if (data.Degree < 1)
            return;

        curveLine.gameObject.SetActive(true);
        outsideLine.gameObject.SetActive(true);

        outsideLine.positionCount = data.PointCounts;

        for (int i = 0; i <= data.Degree; ++i)
        {
            var value = data.GetPointPosition(i);
            NLIValues[0, i] = value;
            NLI_Shells[0, i] = value;
            outsideLine.SetPosition(i, data.GetPointPosition(i));
        }

        if (data.Type == CalculateType.NLI_Form)
            DrawLine_NLI();
        else if (data.Type == CalculateType.BB_Form)
            DrawLine_BB_Form();
        else if (data.Type == CalculateType.Midpoint_Subdivision)
            DrawLine_Midpoint_Subdivision();
    }

    public void UpdateTValue()
    {
        if (data.Degree < 1)
            return;

        if (data.Type != CalculateType.NLI_Form)
            return;

        UpdateNLIShells(data.Degree);
    }


    void Start()
    {
        var size = data.MaxPoints;
        newPoints = new List<Vector2>();
        shellLinePoints = new List<Vector2>();
        midDivisonLinePoints = new List<Vector2>();
        NLIValues = new Vector2[size, size];
        NLI_Shells = new Vector2[size, size];
        MidPointsValues = new Vector2[size, size];
        pascalCombination = new int[size, size];

        curveLine.startWidth = 0.02f;
        curveLine.endWidth = 0.02f;
        curveLine.startColor = Color.black;
        curveLine.endColor = Color.black;
        curveLine.positionCount = linePointCounts + 1;

        outsideLine.startWidth = 0.02f;
        outsideLine.endWidth = 0.02f;
        outsideLine.startColor = Color.gray;
        outsideLine.endColor = Color.gray;

        for (int i = 0; i < data.MaxPoints; ++i)
        {
            pascalCombination[i, 0] = 1;
        }

        for (int i = 1; i < data.MaxPoints; ++i)
        {
            for (int j = 1; j < data.MaxPoints; ++j)
            {
                pascalCombination[i, j] = pascalCombination[i - 1, j - 1] + pascalCombination[i - 1, j];
            }
        }

        RenderLine();
    }

    void Update()
    { }

    void DrawLine_NLI()
    {
        curveLine.positionCount = linePointCounts + 1;
        shellLine.gameObject.SetActive(true);

        UpdateNLIShells(data.Degree);

        for (int i = 0; i <= linePointCounts; ++i)
        {
            float t = (float)i / linePointCounts;
            UpdateNLIValues(data.Degree, t);
            curveLine.SetPosition(i, NLIValues[data.Degree, 0]);
        }
    }

    void UpdateNLIShells(int d)
    {
        float t = data.TValue;

        newPoints.Clear();
        shellLinePoints.Clear();

        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j <= d - i; ++j)
            {
                var p1 = NLI_Shells[i - 1, j];
                var p2 = NLI_Shells[i - 1, j + 1];

                NLI_Shells[i, j] = (1.0f - t) * p1 + t * p2;
                newPoints.Add(NLI_Shells[i, j]);
            }
        }

        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j < d - i; j++)
            {
                shellLinePoints.Add(NLI_Shells[i, j]);
                shellLinePoints.Add(NLI_Shells[i + 1, j]);
                shellLinePoints.Add(NLI_Shells[i, j + 1]);
            }
        }

        shellLine.Render(newPoints, shellLinePoints);
    }

    void UpdateNLIValues(int d, float t)
    {
        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j <= d - i; ++j)
            {
                var p1 = NLIValues[i - 1, j];
                var p2 = NLIValues[i - 1, j + 1];

                NLIValues[i, j] = (1.0f - t) * p1 + t * p2;
            }
        }
    }

    void DrawLine_BB_Form()
    {
        curveLine.positionCount = linePointCounts + 1;
        shellLine.gameObject.SetActive(false);

        for (int i = 0; i <= linePointCounts; ++i)
        {
            float t = (float)i / linePointCounts;
            UpdateBBValues(i, t);
        }
    }

    void UpdateBBValues(int i, float t)
    {
        Vector2 result = Vector2.zero;

        for (int k = 0; k <= data.Degree; ++k)
        {
            int combination = pascalCombination[data.Degree, k];
            result += data.GetPointPosition(k) * combination * Mathf.Pow((1.0f - t), data.Degree - k) * Mathf.Pow(t, k);
        }

        curveLine.SetPosition(i, new Vector3(result.x, result.y, 0));
    }

    void DrawLine_Midpoint_Subdivision()
    {

        midDivisonLinePoints.Clear();
        shellLine.gameObject.SetActive(false);

        int degree = data.Degree;

        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i <= degree; ++i)
            points.Add(NLIValues[0, i]);

        CalculateMidPoints(degree, midPointIterationCount, points);

        curveLine.positionCount = midDivisonLinePoints.Count;
        for (int i = 0; i < midDivisonLinePoints.Count; ++i)
            curveLine.SetPosition(i, midDivisonLinePoints[i]);
    }

    void CalculateMidPoints(int d, int k, List<Vector2> points)
    {
        UpdateMidpoints(d, points);
        if (k != 0)
        {
            List<Vector2> leftPoints = new List<Vector2>();
            List<Vector2> rightPoints = new List<Vector2>();

            for (int i = 0; i <= d; ++i)
            {
                leftPoints.Add(MidPointsValues[i, 0]);
                rightPoints.Add(MidPointsValues[d - i, i]);
            }

            CalculateMidPoints(d, k - 1, leftPoints);
            CalculateMidPoints(d, k - 1, rightPoints);
        }

        else
        {
            for (int i = 0; i <= d; ++i)
            {
                midDivisonLinePoints.Add(MidPointsValues[i, 0]);
            }

            for (int i = 0; i <= d; ++i)
            {
                midDivisonLinePoints.Add(MidPointsValues[d - i, i]);
            }
        }
    }

    void UpdateMidpoints(int d, List<Vector2> points)
    {
        for (int i = 0; i <= d; ++i)
        {
            MidPointsValues[0, i] = points[i];
        }

        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j <= d - i; ++j)
            {
                var p1 = MidPointsValues[i - 1, j];
                var p2 = MidPointsValues[i - 1, j + 1];

                MidPointsValues[i, j] = 0.5f * p1 + 0.5f * p2;
            }
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private LineRenderer curveLine;
    [SerializeField] private LineRenderer outsideLine;

    public List<int> knots;
    private int linePointCounts = 500;

    public void AddKnot()
    {
        int nextValue = knots.Count;
        knots.Add(nextValue);
    }

    public void Clear()
    {
        knots.Clear();
        int N = data.Degree + 1;
        for (int i = 0; i < N; ++i)
            knots.Add(i);

        curveLine.gameObject.SetActive(false);
        outsideLine.gameObject.SetActive(false);
    }

    public void ChangeDegree()
    {
        knots.Clear();
        int N = data.Degree + 1;
        for (int i = 0; i < N; ++i)
            knots.Add(i);

        for (int i = 0; i < data.PointCounts; ++i)
        {
            int nextValue = knots.Count;
            knots.Add(nextValue);
        }

        RenderLine();
    }

    public void RenderLine()
    {
        curveLine.gameObject.SetActive(true);
        outsideLine.gameObject.SetActive(true);

        outsideLine.positionCount = data.PointCounts;
        double[] xPosition = new double[data.PointCounts];
        double[] yPosition = new double[data.PointCounts];

        for (int i = 0; i < data.PointCounts; ++i)
        {
            xPosition[i] = (double)data.GetPointPosition(i).x;
            yPosition[i] = (double)data.GetPointPosition(i).y;
            outsideLine.SetPosition(i, data.GetPointPosition(i));
        }

        if (data.PointCounts <= data.Degree)
        {
            curveLine.gameObject.SetActive(false);
            return;
        }

        Draw(xPosition, yPosition);
    }


    void Start()
    {
        curveLine.startWidth = 0.04f;
        curveLine.endWidth = 0.04f;
        curveLine.startColor = Color.blue;
        curveLine.endColor = Color.blue;
        curveLine.positionCount = linePointCounts + 1;

        outsideLine.startWidth = 0.02f;
        outsideLine.endWidth = 0.02f;
        outsideLine.startColor = Color.gray;
        outsideLine.endColor = Color.gray;

        knots = new List<int>();
        int N = data.Degree + 1;
        for (int i = 0; i < N; ++i)
            knots.Add(i);
    }

    void Draw(double[] xPositions, double[] yPositions)
    {
        int max = knots.Count - 1 - data.Degree;
        int validRange = max - data.Degree;
        double step = (double)(validRange) / linePointCounts;
        for (int i = 0; i <= linePointCounts; ++i)
        {
            double t = data.Degree + (i * step);
            int j = FindJIndex(t);
            double x = GetResult(xPositions, t, data.Degree, j);
            double y = GetResult(yPositions, t, data.Degree, j);

            curveLine.SetPosition(i, new Vector3((float)x, (float)y, 0));
        }
    }

    int FindJIndex(double t)
    {
        for (int i = data.Degree; i < knots.Count - 1; ++i)
        {
            if (knots[i] <= t && t < knots[i + 1])
                return i;
        }

        return knots.Count - 1;
    }

    double GetResult(double[] positions, double t, int k, int i)
    {
        if (k == 0)
        {
            if (i >= positions.Length)
                return 0;

            return positions[i];
        }

        int index = i + data.Degree - (k - 1);
        return ((t - knots[i]) / (knots[index] - knots[i])) * GetResult(positions, t, k - 1, i) +
               ((knots[index] - t) / (knots[index] - knots[i])) * GetResult(positions, t, k - 1, i - 1);
    }
}

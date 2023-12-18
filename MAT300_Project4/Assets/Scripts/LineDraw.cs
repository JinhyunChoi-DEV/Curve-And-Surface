using System;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private LineRenderer curveLine;
    [SerializeField] private LineRenderer outsideLine;

    private int linePointCounts = 300;

    public void Clear()
    {
        curveLine.gameObject.SetActive(false);
        outsideLine.gameObject.SetActive(false);
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
            outsideLine.SetPosition(i, data.GetPointPosition(i));
        }

        DrawInterpolatingCubicSplines();
    }


    void Start()
    {

        curveLine.startWidth = 0.02f;
        curveLine.endWidth = 0.02f;
        curveLine.startColor = Color.black;
        curveLine.endColor = Color.black;
        curveLine.positionCount = linePointCounts + 1;

        outsideLine.startWidth = 0.02f;
        outsideLine.endWidth = 0.02f;
        outsideLine.startColor = Color.gray;
        outsideLine.endColor = Color.gray;

        RenderLine();
    }

    void DrawInterpolatingCubicSplines()
    {
        var coefficientResult = CurrentEquationResult();
        var result_x = coefficientResult.Item1;
        var result_y = coefficientResult.Item2;

        double step = (double)data.Degree / linePointCounts;
        for (int i = 0; i <= linePointCounts; ++i)
        {
            double t = i * step;
            var position = GetPosition(t, result_x, result_y);
            curveLine.SetPosition(i, new Vector3(position.x, position.y, 0));
        }
    }

    (double[], double[]) CurrentEquationResult()
    {
        double[,] A_x = new double[data.PointCounts + 2, data.PointCounts + 3];
        double[,] A_y = new double[data.PointCounts + 2, data.PointCounts + 3];
        int maxColumn = data.Degree + 3;
        int d = 0;
        double c = 0;

        for (int i = 0; i < data.PointCounts; i++)
        {
            int t = i;
            d = 0;
            c = 0;
            for (int j = 0; j < maxColumn; ++j)
            {
                if (j > 3)
                    c++;

                A_x[i, j] = GetPowerFunctionValue(t, c, d);
                A_y[i, j] = GetPowerFunctionValue(t, c, d);

                d++;
                if (d > 3)
                    d = 3;
            }

            A_x[i, maxColumn] = data.GetPointPosition(i).x;
            A_y[i, maxColumn] = data.GetPointPosition(i).y;
        }

        c = 0;
        d = 0;
        for (int i = 0; i < maxColumn; ++i)
        {
            if (i > 3)
                c++;

            if (d < 2)
            {
                A_x[data.PointCounts, i] = 0;
                A_x[data.PointCounts + 1, i] = 0;
                A_y[data.PointCounts, i] = 0;
                A_y[data.PointCounts + 1, i] = 0;
            }
            else
            {
                A_x[data.PointCounts, i] = d * (d - 1) * GetPowerFunctionValue(0, c, d - 2);
                A_y[data.PointCounts, i] = d * (d - 1) * GetPowerFunctionValue(0, c, d - 2);
                A_x[data.PointCounts + 1, i] = d * (d - 1) * GetPowerFunctionValue(data.Degree, c, d - 2);
                A_y[data.PointCounts + 1, i] = d * (d - 1) * GetPowerFunctionValue(data.Degree, c, d - 2);
            }

            d++;
            if (d > 3)
                d = 3;
        }
        A_x[data.PointCounts, maxColumn] = 0;
        A_y[data.PointCounts, maxColumn] = 0;
        A_x[data.PointCounts + 1, maxColumn] = 0;
        A_y[data.PointCounts + 1, maxColumn] = 0;

        var result_x = Gauss_Jordan_Elimination.GetEliminationResult(A_x, data.PointCounts + 2);
        var result_y = Gauss_Jordan_Elimination.GetEliminationResult(A_y, data.PointCounts + 2);

        return (result_x, result_y);
    }

    double GetPowerFunctionValue(double t, double c, int d)
    {
        if (d == 0)
            return 1;

        if (t < c)
            return 0.0;

        return Math.Pow((t - c), d);
    }

    Vector2 GetPosition(double t, double[] coefficient_x, double[] coefficient_y)
    {
        int d = 0;
        double c = 0;
        double x_result = 0;
        double y_result = 0;

        int maxColumn = data.Degree + 3;
        for (int i = 0; i < maxColumn; ++i)
        {
            if (i > 3)
                c++;

            var value = GetPowerFunctionValue(t, c, d);
            x_result += coefficient_x[i] * value;
            y_result += coefficient_y[i] * value;
            d++;
            if (d > 3)
                d = 3;
        }

        //var value = GetPowerFunctionValue(t, 0, 3);

        return new Vector2((float)x_result, (float)y_result);
    }
}

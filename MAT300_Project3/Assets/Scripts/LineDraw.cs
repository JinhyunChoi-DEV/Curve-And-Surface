using UnityEngine;

struct NewtonData
{
    public int minIndex;
    public int maxIndex;

    public double result;
}

public class LineDraw : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private LineRenderer curveLine;
    [SerializeField] private LineRenderer outsideLine;

    private NewtonData[,] xDatas;
    private NewtonData[,] yDatas;

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
            xDatas[0, i].result = value.x;
            xDatas[0, i].minIndex = i;
            xDatas[0, i].maxIndex = i;

            yDatas[0, i].result = value.y;
            yDatas[0, i].minIndex = i;
            yDatas[0, i].maxIndex = i;
        }

        DrawNewtonFormLine();
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

        xDatas = new NewtonData[data.MaxPoints, data.MaxPoints];
        yDatas = new NewtonData[data.MaxPoints, data.MaxPoints];

        RenderLine();
    }

    void DrawNewtonFormLine()
    {
        int d = data.Degree;

        for (int i = 1; i <= d; ++i)
        {
            for (int j = 0; j <= d - i; ++j)
            {
                UpdateData(xDatas, i, j);
                UpdateData(yDatas, i, j);
            }
        }

        double step = (double)d / linePointCounts;
        for (int i = 0; i <= linePointCounts; ++i)
        {
            double t = i * step;
            var position = GetNewPosition(t);
            curveLine.SetPosition(i, new Vector3(position.x, position.y, 0));
        }
    }

    void UpdateData(NewtonData[,] newtonData, int i, int j)
    {
        var leftData = newtonData[i - 1, j];
        var rightData = newtonData[i - 1, j + 1];

        newtonData[i, j].minIndex = leftData.minIndex;
        newtonData[i, j].maxIndex = rightData.maxIndex;
        newtonData[i, j].result = (rightData.result - leftData.result) / (rightData.maxIndex - leftData.minIndex);
    }

    Vector2 GetNewPosition(double t)
    {
        double xResult = 0.0;
        double yResult = 0.0;
        for (int i = 0; i < data.PointCounts; ++i)
        {
            if (i == 0)
            {
                xResult += xDatas[i, 0].result;
                yResult += yDatas[i, 0].result;
            }
            else
            {
                var tValue = CalculateTValue(t, i);
                xResult += xDatas[i, 0].result * tValue;
                yResult += yDatas[i, 0].result * tValue;
            }
        }

        return new Vector2((float)xResult, (float)yResult);
    }

    double CalculateTValue(double t, int index)
    {
        double result = t;

        if (index == 1)
            return result;

        for (int i = 2; i <= index; ++i)
        {
            result *= (t - (i - 1));
        }
        return result;
    }
}

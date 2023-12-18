using UnityEngine;

public class DrawPoints : MonoBehaviour
{
    public int Degree => degree;

    [SerializeField] private DrawDeCasteljauGraph draw;
    [SerializeField] private Point prefab;
    [SerializeField] private GameObject pointsRoot;
    [SerializeField] private RectTransform coordinateRect;

    private int degree = 1;
    private const int maxDegree = 20;
    private Point[] points;
    private int activePoints => degree + 1;


    public void ChangeDegree(bool isAdd)
    {
        int prevDegree = degree;
        if (isAdd && degree < 20)
        {
            degree++;
        }

        if (!isAdd && degree > 1)
        {
            degree--;
        }

        if (prevDegree != degree)
        {
            UpdateActivePoints();
        }
    }

    public float GetPointsDegreeValue(int index)
    {
        return points[index].GetDegreeValue();
    }

    void Start()
    {
        prefab.gameObject.SetActive(false);
        points = new Point[maxDegree + 1];
        for (int i = 0; i <= maxDegree; ++i)
        {
            points[i] = Instantiate(prefab, pointsRoot.transform);
            points[i].gameObject.SetActive(false);
        }

        UpdateActivePoints();
    }

    void UpdateActivePoints()
    {
        for (int i = 0; i < maxDegree + 1; ++i)
        {
            var obj = points[i].gameObject;

            if (i < activePoints)
            {
                obj.SetActive(true);

                var max = coordinateRect.rect.max;
                var min = coordinateRect.rect.min;

                var localPos = obj.transform.localPosition;
                var t = (float)i / degree;
                var xPos = Mathf.Lerp(min.x, max.x, t);
                var newPos = new Vector3(xPos, localPos.y, localPos.z);
                obj.transform.localPosition = newPos;
            }
            else
            {
                obj.SetActive(false);
            }

        }

        draw.Draw();
    }

}

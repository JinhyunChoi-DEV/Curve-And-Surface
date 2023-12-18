using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PointCreator : MonoBehaviour
{
    [SerializeField] private LineDraw lineDraw;
    [SerializeField] private GameObject pointsHolder;
    [SerializeField] private Point prefab;

    public int Degree => currentDegree;
    public int PointCounts => pointCounts;
    public int MaxPoints => maxPoints;
    public int MaxDegree => maxDegree;

    private int pointCounts;
    private List<Point> points;
    private int maxPoints;
    private int currentDegree;
    private int maxDegree;

    public Vector2 GetPointValue(int index)
    {
        var obj = points[index].gameObject;
        return new Vector2(obj.transform.position.x, obj.transform.position.y);
    }

    public void SetCurrentDegree(int degree)
    {
        currentDegree = degree;
        lineDraw.ChangeDegree();
    }

    public void Clear()
    {
        pointCounts = 0;

        for (int i = 0; i < points.Count; ++i)
            points[i].gameObject.SetActive(false);
    }

    void Awake()
    {
        pointCounts = 0;
        prefab.gameObject.SetActive(false);

        maxPoints = 21;
        currentDegree = 3;
        maxDegree = 20;
        points = new List<Point>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (pointCounts >= MaxPoints)
                return;

            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            pointCounts++;
            if (points.Count < pointCounts)
                CreatePoint(mousePosition);
            else
                ReactivePoint(mousePosition);

            lineDraw.AddKnot();
            lineDraw.RenderLine();
        }
    }

    void CreatePoint(Vector3 position)
    {
        var point = Instantiate(prefab, pointsHolder.transform);
        point.gameObject.SetActive(true);
        point.transform.position = new Vector3(position.x, position.y, 0);
        points.Add(point);
    }

    void ReactivePoint(Vector3 position)
    {
        points[pointCounts - 1].gameObject.SetActive(true);
        points[pointCounts - 1].gameObject.transform.position = new Vector3(position.x, position.y, 0);
    }
}

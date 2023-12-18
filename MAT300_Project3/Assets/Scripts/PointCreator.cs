using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PointCreator : MonoBehaviour
{
    [SerializeField] private LineDraw lineDraw;
    [SerializeField] private GameObject pointsHolder;
    [SerializeField] private Point prefab;
    [SerializeField] private int maxDegree;

    public int MaxDegree => maxDegree;
    public int Degree => pointCounts - 1;
    public int PointCounts => pointCounts;
    public int MaxPoints => maxDegree + 1;

    private int pointCounts;
    private List<Point> points;

    public Vector2 GetPointValue(int index)
    {
        var obj = points[index].gameObject;
        return new Vector2(obj.transform.position.x, obj.transform.position.y);
    }

    public void Clear()
    {
        pointCounts = 0;

        for (int i = 0; i <= maxDegree; ++i)
            points[i].gameObject.SetActive(false);
    }

    void Awake()
    {
        pointCounts = 0;
        prefab.gameObject.SetActive(false);

        if (maxDegree <= 0)
            maxDegree = 20;

        points = new List<Point>(maxDegree + 1);

        for (int i = 0; i <= maxDegree; ++i)
        {
            var point = Instantiate(prefab, pointsHolder.transform);
            point.gameObject.SetActive(false);
            points.Add(point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool canAdd = pointCounts <= maxDegree;

        if (Input.GetMouseButtonDown(1) && canAdd)
        {
            pointCounts++;
            points[pointCounts - 1].gameObject.SetActive(true);
            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            points[pointCounts - 1].gameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            lineDraw.RenderLine();
        }
    }

}

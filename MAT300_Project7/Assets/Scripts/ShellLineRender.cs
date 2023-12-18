using System.Collections.Generic;
using UnityEngine;

public class ShellLineRender : MonoBehaviour
{
    [SerializeField] private GameObject lineHolder;
    [SerializeField] private GameObject pointHolder;
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private GameObject pointPrafab;
    [SerializeField] private DataContainer data;

    private List<GameObject> shellPoints;
    private List<LineRenderer> lineRender;

    public void Clear()
    {
        for (int i = 0; i < lineRender.Count; ++i)
        {
            lineRender[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < shellPoints.Count; ++i)
        {
            shellPoints[i].gameObject.SetActive(false);
        }
    }

    public void Render(List<Vector2> points, List<Vector2> shellLinePoints)
    {
        int activeCount = points.Count;
        int index = 0;

        // Active Points
        for (int i = 0; i < shellPoints.Count; ++i)
        {
            if (i < activeCount)
            {
                shellPoints[i].gameObject.SetActive(true);
                shellPoints[i].gameObject.transform.position = points[i];
            }
            else
            {
                shellPoints[i].gameObject.SetActive(false);
            }
        }

        //Render Line
        for (int i = 0; i < shellLinePoints.Count; i += 3)
        {
            lineRender[index].gameObject.SetActive(true);
            lineRender[index].SetPosition(0, shellLinePoints[i]);
            lineRender[index].SetPosition(1, shellLinePoints[i + 1]);
            lineRender[index].SetPosition(2, shellLinePoints[i + 2]);
            index++;
        }
    }

    void Start()
    {
        lineRender = new List<LineRenderer>();
        shellPoints = new List<GameObject>();

        linePrefab.gameObject.SetActive(false);
        pointPrafab.gameObject.SetActive(false);

        linePrefab.startWidth = 0.02f;
        linePrefab.endWidth = 0.02f;
        linePrefab.startColor = Color.red;
        linePrefab.endColor = Color.red;
        linePrefab.positionCount = 3;
    }
}

using UnityEngine;

public class DataContainer : MonoBehaviour
{
    [SerializeField] private PointCreator pointCreator;
    [SerializeField] private ControlPanel controlPanel;

    public int Degree => pointCreator.Degree;
    public int PointCounts => pointCreator.PointCounts;
    public int MaxDegree => pointCreator.MaxDegree;
    public int MaxPoints => pointCreator.MaxPoints;

    public Vector2 GetPointPosition(int index) => pointCreator.GetPointValue(index);
}

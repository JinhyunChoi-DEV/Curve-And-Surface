using UnityEngine;
using UnityEngine.UI;

public class DegreeChangePanel : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private PointCreator pointCreator;
    [SerializeField] private Button up;
    [SerializeField] private Button down;

    void Start()
    {
        up.onClick.AddListener(() =>
        {
            if (data.Degree >= data.MaxDegree)
                return;

            int newDegree = data.Degree + 1;
            pointCreator.SetCurrentDegree(newDegree);
        });

        down.onClick.AddListener(() =>
        {
            if (data.Degree <= 1)
                return;

            int newDegree = data.Degree - 1;
            pointCreator.SetCurrentDegree(newDegree);
        });
    }
}
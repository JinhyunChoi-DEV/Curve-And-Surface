using TMPro;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private TMP_Text pointText;

    void Update()
    {
        pointText.text = data.PointCounts.ToString();
    }
}

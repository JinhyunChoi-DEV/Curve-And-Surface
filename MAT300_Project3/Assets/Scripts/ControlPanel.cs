using TMPro;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text pointText;

    void Start()
    {
        typeText.text = "Method: NEWTON-Form";

    }

    void Update()
    {
        pointText.text = data.PointCounts.ToString();
    }
}

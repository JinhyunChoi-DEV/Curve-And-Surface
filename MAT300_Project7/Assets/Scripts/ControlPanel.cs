using TMPro;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private TMP_Text degreeText;

    void Update()
    {
        degreeText.text = data.Degree.ToString();
    }
}

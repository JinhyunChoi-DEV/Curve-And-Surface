using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DegreeController : MonoBehaviour
{
    [SerializeField] private Button decrease;
    [SerializeField] private Button increase;
    [SerializeField] private TMP_Text degreeText;
    [SerializeField] private DrawPoints points;

    // Start is called before the first frame update
    void Start()
    {
        decrease.onClick.AddListener(() => { points.ChangeDegree(false); });
        increase.onClick.AddListener(() => { points.ChangeDegree(true); });
    }

    // Update is called once per frame
    void Update()
    {
        degreeText.text = points.Degree.ToString();
    }
}

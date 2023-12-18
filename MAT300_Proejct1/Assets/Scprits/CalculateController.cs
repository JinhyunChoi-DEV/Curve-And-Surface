using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateController : MonoBehaviour
{
    [SerializeField] private DrawDeCasteljauGraph graph;
    [SerializeField] private Button changeButton;
    [SerializeField] private TMP_Text calculateModeText;

    void Start()
    {
        changeButton.onClick.AddListener(() => { graph.ChangeDrawMethod(); });
    }

    void Update()
    {
        if (graph.IsBB_form)
            calculateModeText.text = "BB-form";
        else
            calculateModeText.text = "NLI-form";
    }
}

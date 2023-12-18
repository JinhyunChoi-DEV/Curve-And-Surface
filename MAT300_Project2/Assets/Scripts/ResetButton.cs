using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField] PointCreator points;
    [SerializeField] LineDraw lineDraw;
    [SerializeField] private Button button;

    void Start()
    {
        if (button == null)
            return;

        button.onClick.AddListener(Clear);
    }
    void Clear()
    {
        points.Clear();
        lineDraw.Clear();
    }
}

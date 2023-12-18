using TMPro;
using UnityEngine;

public class KnotSeqTextPanel : MonoBehaviour
{
    [SerializeField] private LineDraw lineDraw;
    [SerializeField] private TMP_Text text;

    void Update()
    {
        text.text = "knot: {";
        for (int i = 0; i < lineDraw.knots.Count; ++i)
        {
            if (i == lineDraw.knots.Count - 1)
            {
                text.text += lineDraw.knots[i] + "}";
            }
            else
            {
                text.text += lineDraw.knots[i] + ", ";
            }
        }
    }
}
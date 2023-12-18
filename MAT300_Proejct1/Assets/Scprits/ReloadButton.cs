using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    [SerializeField] private Button button;

    void Start()
    {
        if (button == null)
            return;

        button.onClick.AddListener(ReloadCurrentScene);
    }
    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

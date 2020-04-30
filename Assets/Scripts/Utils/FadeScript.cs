using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animationFade;

    [Header("SceneManagement")]
    [SerializeField, Tooltip("Enter the scene name")] private string sceneName;

    [Header("Debug")]
    [SerializeField] private bool debugFade;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void FadeIn()
    {
        animationFade.Play("A_FadeIn");
    }

    private void DebugFade()
    {
        if (debugFade)
        {
            animationFade.Play("A_FadeOut");
        }
    }
}

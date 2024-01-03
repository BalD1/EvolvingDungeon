using StdNounou;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private AlphaHandler alphaHandler;

    private LTDescr fadeDescr;

    private void Reset()
    {
        alphaHandler = this.GetComponent<AlphaHandler>();
    }

    public void FadeIn()
        => FadeIn(1);
    public void FadeIn(float duration)
        => PerformFade(duration, 0);

    public void FadeOut()
        => FadeOut(1);
    public void FadeOut(float duration)
        => PerformFade(duration, 1);

    private void PerformFade(float duration, float alphaGoal)
    {
        if (fadeDescr != null)
        {
            LeanTween.cancel(fadeDescr.uniqueId);
        }
        fadeDescr = alphaHandler.LeanAlpha(alphaGoal, duration);
    }
}
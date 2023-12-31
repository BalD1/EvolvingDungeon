using StdNounou;
using UnityEngine;

public class LoadingScreen : MonoBehaviourEventsHandler
{
    [SerializeField] private RectTransform loadingPrompt;
    [SerializeField] private RectTransform loadingCompletedScreen;

    protected override void EventsSubscriber()
    {
        SceneHandlerEvents.OnLoadingCompleted += OnLoadingCompleted;
    }

    protected override void EventsUnSubscriber()
    {
        SceneHandlerEvents.OnLoadingCompleted -= OnLoadingCompleted;
    }

    private void OnLoadingCompleted()
    {
        loadingPrompt.gameObject.SetActive(false);
        loadingCompletedScreen.gameObject.SetActive(true);
    }
}

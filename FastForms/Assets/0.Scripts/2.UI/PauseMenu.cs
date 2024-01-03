using StdNounou;
using UnityEngine;

public class PauseMenu : UIMainScreen
{
    [Header("Pause Menu specifics")]

    [Header("Buttons")]
    [SerializeField] private ButtonBase resumeButton;
    [SerializeField] private ButtonBase optionsButton;
    [SerializeField] private ButtonBase mainMenuButton;

    [Header("Screens")]
    [SerializeField] private UIScreen optionsScreen;

    protected override void EventsSubscriber()
    {
        base.EventsSubscriber();
        PlayerInputsHandlerEvents.OnPausePressed += OnPausePressed;
    }

    protected override void EventsUnSubscriber()
    {
        base.EventsUnSubscriber();
        PlayerInputsHandlerEvents.OnPausePressed -= OnPausePressed;
    }

    private void OnPausePressed()
    {
        if (UIScreensManager.Instance.ScreensStack.Count == 0) this.Open(true);
    }

    protected override void Awake()
    {
        base.Awake();

        resumeButton.AddListenerToOnClick(Resume);
        optionsButton.AddListenerToOnClick(OpenOptionsScreen);
        mainMenuButton.AddListenerToOnClick(BackToMainMenu);
    }

    private void Resume()
    {
        this.Close(true);
    }

    private void OpenOptionsScreen()
    {
        optionsScreen.Open(true);
    }

    private void BackToMainMenu()
    {

    }
}

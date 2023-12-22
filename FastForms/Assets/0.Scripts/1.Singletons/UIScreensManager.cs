using System.Collections.Generic;
using UnityEngine;

namespace StdNounou
{
    public class UIScreensManager : Singleton<UIScreensManager>
    {
        [field: SerializeField] public Canvas MainCanvas {  get; private set; }
        [field: SerializeField, ReadOnly] public UIRootScreen CurrentRootScreen {  get; private set; }
        [field: SerializeField] public bool OpenMainScreenWhenNoneIsOpen { get; private set; }
        [field: SerializeField] public UIMainScreen MainScreen { get; private set; }
        public static Vector2 CanvasSize { get; private set; }

        public Stack<UIScreen> ScreensStack { get; private set; }

        protected override void EventsSubscriber()
        {
            UIScreenEvents.OnRootScreenOpened += OpenRootScreen;
            UIScreenEvents.OnRootScreenWillOpen += ReplaceOldRootScreen;

            UIScreenEvents.OnRootScreenClosed += OnRootScreenClosed;
            UIScreenEvents.OnRootScreenWillClose += OnRootScreenClosed;

            UIScreenEvents.OnSubScreenStateChanged += OnSubscreenStateChanged;
        }

        protected override void EventsUnSubscriber()
        {
            UIScreenEvents.OnRootScreenOpened -= OpenRootScreen;
            UIScreenEvents.OnRootScreenWillOpen -= ReplaceOldRootScreen;

            UIScreenEvents.OnRootScreenClosed -= OnRootScreenClosed;
            UIScreenEvents.OnRootScreenWillClose -= OnRootScreenClosed;

            UIScreenEvents.OnSubScreenStateChanged -= OnSubscreenStateChanged;
        }

        protected override void Awake()
        {
            ScreensStack = new Stack<UIScreen>();
            if (MainCanvas == null)
            {
                this.LogError("Main Canvas was not set.");
                MainCanvas = GameObject.FindFirstObjectByType<Canvas>();
            }
            if (MainCanvas != null)
            {
                Rect canvasRect = MainCanvas.GetComponent<RectTransform>().rect;
                CanvasSize = new Vector2(canvasRect.width, canvasRect.height);
            }
            base.Awake();
            MainScreen?.Open(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseYoungest();
            }
        }

        private void CloseYoungest()
        {
            if (!ScreensStack.TryPeek(out UIScreen screen)) return;
            if (OpenMainScreenWhenNoneIsOpen && screen == MainScreen) return;
            screen.Close(true);
        }

        private void OpenRootScreen(UIRootScreen newScreen)
        {
            if (ScreensStack.TryPeek(out UIScreen result))
            {
                if (result != newScreen) ScreensStack.Push(newScreen);
            }
            else ScreensStack.Push(newScreen);
            ReplaceOldRootScreen(newScreen);
        }

        private void ReplaceOldRootScreen(UIRootScreen newScreen)
        {
            if (newScreen == null) return;
            if (CurrentRootScreen == newScreen) return;
            UIRootScreen oldRootScreen = CurrentRootScreen;
            CurrentRootScreen = newScreen;
            oldRootScreen?.Close(true);
        }

        private void OnRootScreenClosed(UIRootScreen screen)
        {
            if (screen != CurrentRootScreen) return;

            ScreensStack.TryPop(out _);
            CurrentRootScreen = null;
            if (OpenMainScreenWhenNoneIsOpen)
            {
                MainScreen?.Open(true);
                CurrentRootScreen = MainScreen;
            }
        }

        private void OnSubscreenStateChanged(UISubScreen screen)
        {
            if (screen.IsOpened)
            {
                ScreensStack.Push(screen);
                return;
            }
            if (ScreensStack.Count == 0) return;
            if (ScreensStack.Peek() != screen)
            {
                Stack<UIScreen> tmpScreensQueue = new Stack<UIScreen>();
                do tmpScreensQueue.Push(ScreensStack.Pop());
                while (ScreensStack.Count > 0 && ScreensStack.Peek() != screen);
                if (ScreensStack.Count > 0) ScreensStack.Pop();

                while (tmpScreensQueue.Count > 0)
                    ScreensStack.Push(tmpScreensQueue.Pop());

                return;
            }
            ScreensStack.Pop();
        }

    }
}
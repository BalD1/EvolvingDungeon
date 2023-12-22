
using com.cyborgAssets.inspectorButtonPro;

namespace StdNounou
{
    public class UIRootScreen : UIScreen
    {
        public override void Open(bool playTween)
        {
            if (IsOpened) return;
            if (playTween) this.RootScreenWillOpen();
            base.Open(playTween);
        }

        public override void Close(bool playTween)
        {
            if (!IsOpened) return;
            if (playTween) this.RootScreenWillClose();
            base.Close(playTween);
        }

        protected override void SequenceEnded()
        {
            base.SequenceEnded();
            if (IsOpened) this.RootScreenOpened();
            else this.RootScreenClosed();
        }

#if UNITY_EDITOR
        [ProButton]
        protected override void ED_SearchSubscreens() { base.ED_SearchSubscreens(); }
#endif
    }
}
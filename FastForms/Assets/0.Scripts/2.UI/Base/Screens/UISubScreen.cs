
using com.cyborgAssets.inspectorButtonPro;

namespace StdNounou
{
    public class UISubScreen : UIScreen
    {
        public void Open(bool playTween, bool callDelege)
        {
            if (IsOpened) return;
            base.Open(playTween);
            if (callDelege) this.SubScreenStateChanged();
        }
        public override void Open(bool playTween)
            => Open(playTween, true);

        public void Close(bool playTween, bool callDelege)
        {
            if (!IsOpened) return;
            base.Close(playTween);
            if (callDelege) this.SubScreenStateChanged();
        }
        public override void Close(bool playTween)
            => Close(playTween, true);

#if UNITY_EDITOR
        [ProButton]
        protected override void ED_SearchSubscreens() { base.ED_SearchSubscreens(); }
#endif
    }
}
using UnityEngine;

namespace StdNounou
{
    public class UISceneChanger : SceneChanger
    {
        [SerializeField] private ButtonBase button;

        private void Reset()
        {
            button = this.GetComponent<ButtonBase>();
        }

        private void Awake()
        {
            button.AddListenerToOnClick(ChangeScene);
        }
    } 
}

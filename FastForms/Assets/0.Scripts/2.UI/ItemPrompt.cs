using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrompt : MonoBehaviour
{
    [SerializeField] private Image promptImage;
    [SerializeField] private TextMeshProUGUI promptTitleTxt;
    [SerializeField] private TextMeshProUGUI promptDescrTxt;
    [SerializeField] private TextMeshProUGUI inputTxt;

    [SerializeField] private string promptFormat;

    public void Setup(SO_WeaponData weaponData)
    {
        promptImage.sprite = weaponData.WeaponSprite;
        promptTitleTxt.text = weaponData.WeaponID;
        promptDescrTxt.text = weaponData.RichDescription;
    }
}

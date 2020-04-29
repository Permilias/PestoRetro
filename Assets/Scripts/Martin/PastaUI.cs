using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PastaUI : MonoBehaviour
{
    public TextMeshProUGUI[] pastaTexts;
    public TextMeshProUGUI munitionsText;
    public Image munitionsImage;
    public Image cookingBar;
    public Image reloadImage;
    Color baseCookingColor;

    private void Awake()
    {
        baseCookingColor = cookingBar.color;
    }
    private void Update()
    {
        for(int i = 0;i  < pastaTexts.Length; i++)
        {
            pastaTexts[i].text = PastaManager.Instance.pastas[i].config.pastaName + "s: " + PastaManager.Instance.pastaAmounts[i].ToString();
        }

        cookingBar.fillAmount = PastaGun.Instance.cookingCount / PastaManager.Instance.pastas[PastaGun.Instance.currentSelectedPasta].config.cookingSpeed;
        if (cookingBar.fillAmount >= 1f) cookingBar.color = Color.red;
        else cookingBar.color = baseCookingColor;

        munitionsText.text = "x" + PastaManager.Instance.pastaAmounts[PastaGun.Instance.currentSelectedPasta].ToString();

        munitionsImage.sprite = PastaManager.Instance.pastas[PastaGun.Instance.currentSelectedPasta].config.iconSprite;

        if(PastaGun.Instance.reloadCount > 0f)
        {
            reloadImage.fillAmount = PastaGun.Instance.reloadCount / PastaGun.Instance.reloadSpeed;
        }
        else
        {
            reloadImage.fillAmount = 0f;
        }

    }
}

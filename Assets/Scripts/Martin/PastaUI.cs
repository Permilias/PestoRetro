using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PastaUI : MonoBehaviour
{
    public int playerLife;

    public TextMeshProUGUI munitionsText;
    public Image[] munitionsImage;
    public Image cookingBar;
    public Image reloadImage;
    Color baseCookingColor;

    public GameObject[] lives;

    private void Awake()
    {
        baseCookingColor = cookingBar.color;
    }
    private void Update()
    {
      /*  for(int i = 0;i  < munitionsImage.Length; i++)
        {
            munitionsImage[i].color = new Color(1, 1, 1, 0.2f);

            if(i == PastaGun.Instance.currentSelectedPasta)
            {
                munitionsImage[i].color = new Color(1, 1, 1, 1f);
            }
        }

        cookingBar.fillAmount = PastaGun.Instance.cookingCount / PastaManager.Instance.pastas[PastaGun.Instance.currentSelectedPasta].config.cookingSpeed;
        if (cookingBar.fillAmount >= 1f) cookingBar.color = Color.red;
        else cookingBar.color = baseCookingColor;

        munitionsText.text = "x" + PastaManager.Instance.pastaAmounts[PastaGun.Instance.currentSelectedPasta].ToString();

        if(PastaGun.Instance.reloadCount > 0f)
        {
            reloadImage.fillAmount = PastaGun.Instance.reloadCount / PastaGun.Instance.reloadSpeed;
        }
        else
        {
            reloadImage.fillAmount = 0f;
        }


       */
    }
}

using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingRodUpgrade : MonoBehaviour, IInteractable
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text recipeText;
    [SerializeField] Button upgradeButton;
    [SerializeField] List<Recipe> recipes;

    int level = 0;
    bool isShow = false;
    private void Awake()
    {
        upgradeButton.onClick.AddListener(Upgrade);

        SetText();
    }
    public void Interact()
    {
        isShow = !isShow;

        canvasGroup.DOFade(isShow ? 1f : 0f, 0.2f);
        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;

        if (isShow)
            PlayerFishingManager.Instance.DisableFishing();
        else
            PlayerFishingManager.Instance.EnableFishing();


        SetButton();
    }
    void SetButton()
    {
        if (recipes.Count <= level)
        {
            upgradeButton.interactable = false;

            upgradeButton.GetComponentInChildren<TMP_Text>().text = "최대 강화";
        }
        else
        {
            upgradeButton.interactable = recipes[level].IsCraftable();
        }
    }
    void SetText()
    {
        if (recipes.Count <= level)
        {
            recipeText.text = string.Empty;
        }
        else
        {
            recipeText.text = recipes[level].ToString();
        }
    }
    public void Release()
    {
        if (isShow)
        {
            isShow = false;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            PlayerFishingManager.Instance.EnableFishing();

            canvasGroup.DOFade(0f, 0.2f);
        }
    }
    public void Upgrade()
    {
        PlayerInventory.Instance.UseItem(recipes[level]);

        FishingData.SetFishingLevel(level + 2);

        level += 1;

        SetButton();
        SetText();
    }
    public Vector3 GetPosition() => transform.position;
}

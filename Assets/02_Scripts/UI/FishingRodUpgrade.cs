using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingRodUpgrade : UIPanel, IInteractable
{
    [SerializeField] CanvasGroup canvasGroup_2;
    [SerializeField] TMP_Text recipeText;
    [SerializeField] Button upgradeButton;
    [SerializeField] List<Recipe> recipes;

    int level = 0;
    bool isShow = false;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.onClick.AddListener(Upgrade);

        SetText();
    }
    public override void Hide(bool trigger = true)
    {
        if (trigger) UIManager.OnShow(this);

        canvasGroup_2.DOFade(0f, 0.2f);
        canvasGroup_2.interactable = false;
        canvasGroup_2.blocksRaycasts = false;

        isShow = false;
    }
    public override void Show(bool trigger = true)
    {
        if (trigger) UIManager.OnHide(this);

        canvasGroup_2.DOFade(1f, 0.2f);
        canvasGroup_2.interactable = true;
        canvasGroup_2.blocksRaycasts = true;

        isShow = true;

        SetButton();
    }
    public void Interact()
    {
        if (!isShow) Show();
        
        else Hide();
    }
    void SetButton()
    {
        if (recipes.Count <= level)
        {
            upgradeButton.interactable = false;

            upgradeButton.GetComponentInChildren<TMP_Text>().text = "최대 강화";
        }
        else
            upgradeButton.interactable = recipes[level].IsCraftable();
    }
    void SetText()
    {
        if (recipes.Count <= level)
            recipeText.text = string.Empty;
        else
            recipeText.text = recipes[level].ToString();
    }
    public void Release()
    {
        if (isShow)
        {
            isShow = false;
            canvasGroup_2.interactable = false;
            canvasGroup_2.blocksRaycasts = false;

            canvasGroup_2.DOFade(0f, 0.2f);
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

    public void Select()
    {

    }
}

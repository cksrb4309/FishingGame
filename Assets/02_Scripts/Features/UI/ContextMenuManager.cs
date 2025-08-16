using System.Collections.Generic;
using UnityEngine;

public class ContextMenuManager : MonoBehaviour
{
    public static ContextMenuManager Instance { get; private set; } = null;

    [SerializeField] RectTransform menuRect;
    [SerializeField] CanvasGroup menuCanvasGroup;

    [SerializeField] List<ItemContextMenu> contextMenuList;
    void Awake()
    {
        Instance = this;
    }
    public void OpenMenu(Item item, Vector2 screenPosition)
    {
        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;

        for (int i = 0; i < contextMenuList.Count; i++) contextMenuList[i].Setup(item);

        menuRect.transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
    }
    public void CloseMenu()
    {
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
    }
}

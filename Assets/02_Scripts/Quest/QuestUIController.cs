using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quest
{
    public class QuestUIController : MonoBehaviour
    {
        public static QuestUIController Instance { get; private set; } = null;
        
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] TMP_Text questNameText;
        [SerializeField] TMP_Text questObjectiveText;

        [SerializeField] List<QuestItemSlot> rewardItemSlots;

        [SerializeField] Button acceptButton;
        [SerializeField] Button declineButton;

        public static QuestInfo quest = null;
        public void ShowQuestUI()
        {
            canvasGroup.alpha = 1f;

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            questNameText.text = quest.questName;
            questObjectiveText.text = quest.questDescription;

            acceptButton.onClick.AddListener(() => QuestManager.Instance.AcceptQuest(quest));
            acceptButton.onClick.AddListener(HideQuestUI);

            declineButton.onClick.AddListener(HideQuestUI);

            for (int i = 0; i < rewardItemSlots.Count; i++)
            {
                if (i < quest.rewardItems.Count)
                {
                    rewardItemSlots[i].SettingQuestItem(
                        item: quest.rewardItems[i].targetItem,
                        itemCount: quest.rewardItems[i].itemCount);
                }
                else
                {
                    rewardItemSlots[i].Clear();
                }
            }
        }
        public void HideQuestUI()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            acceptButton.onClick.RemoveAllListeners();
            declineButton.onClick.RemoveAllListeners();
        }
        private void Awake()
        {
            Instance = this;
        }
    }
}

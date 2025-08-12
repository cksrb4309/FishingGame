using Quest;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO 버그 발생 경우 고려 : 대화 진행 중 다른 UI 요소가 띄워질 때의 영향 고쳐야함
namespace Dialogue
{
    public class DialogueUIController : UIPanel
    {
        public static DialogueUIController Instance { get; private set; }

        [Header("UI 참조")]
        [SerializeField] private DialogueUI[] dialogueUis; // 0: 왼쪽, 1: 오른쪽

        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private Button dialogueAdvanceButton;

        private System.Action onDialogueEnd;

        private DialogueNode currentNode;
        private int currentLineIndex;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            foreach (var button in choiceButtons)
                button.gameObject.SetActive(false);
        }

        public void StartDialogue(DialogueTree tree, System.Action onEnd = null)
        {
            if (tree == null || tree.startNode == null)
            {
                Debug.LogWarning("DialogueTree가 비어 있음");
                return;
            }

            onDialogueEnd = onEnd;
            Show();

            ShowNode(tree.startNode);
        }
        private void ShowNode(DialogueNode node)
        {
            currentNode = node;
            currentLineIndex = 0;

            ShowCurrentLine();
        }
        private void ShowCurrentLine()
        {
            if (currentNode.lines == null || currentLineIndex >= currentNode.lines.Count)
            {
                dialogueAdvanceButton.gameObject.SetActive(false);  // 대화 끝나면 숨기기
                ShowChoices(currentNode.choices);
                HandleNodeAction(currentNode.actionType);
                return;
            }

            var line = currentNode.lines[currentLineIndex];

            var npcData = Npc.NpcDatabase.Instance.GetNpcData(line.speakerNpcId);

            dialogueUis[(int)line.position].Show(npcData);

            dialogueText.text = line.text;

            dialogueAdvanceButton.gameObject.SetActive(true);
            dialogueAdvanceButton.onClick.RemoveAllListeners();
            dialogueAdvanceButton.onClick.AddListener(AdvanceLine);

            ClearChoiceButtons();  // 선택지는 라인 출력 중엔 숨김
        }

        private void AdvanceLine()
        {
            currentLineIndex++;
            ShowCurrentLine();
        }

        //private void ShowChoices(List<DialogueChoice> choices)
        //{
        //    dialogueAdvanceButton.gameObject.SetActive(false);

        //    ClearChoiceButtons();

        //    if (choices == null || choices.Count == 0)
        //    {
        //        choiceButtons[0].gameObject.SetActive(true);
        //        choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "끝내기";
        //        choiceButtons[0].onClick.RemoveAllListeners();
        //        choiceButtons[0].onClick.AddListener(() => EndDialogue());
        //        return;
        //    }

        //    for (int i = 0; i < choices.Count && i < choiceButtons.Length; i++)
        //    {
        //        var choice = choices[i];
        //        var button = choiceButtons[i];

        //        button.gameObject.SetActive(true);
        //        button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

        //        button.onClick.RemoveAllListeners();
        //        button.onClick.AddListener(() =>
        //        {
        //            choice.onChoiceSelected?.Invoke();
        //            if (choice.nextNode != null)
        //                ShowNode(choice.nextNode);
        //            else
        //                EndDialogue();
        //        });
        //    }
        //}
        private void ShowChoices(List<DialogueChoice> choices)
        {
            dialogueAdvanceButton.gameObject.SetActive(false);

            ClearChoiceButtons();

            if (choices == null || choices.Count == 0)
            {
                EndDialogue();
                return;
            }

            for (int i = 0; i < choices.Count && i < choiceButtons.Length; i++)
            {
                var choice = choices[i];
                var button = choiceButtons[i];

                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    choice.onChoiceSelected?.Invoke();
                    if (choice.nextNode != null)
                        ShowNode(choice.nextNode);
                    else
                        EndDialogue();
                });
            }
        }
        private void HandleNodeAction(DialogueActionType actionType)
        {
            switch (actionType)
            {
                case DialogueActionType.StartQuest:
                    Debug.Log("퀘스트 시작");
                    QuestUIController.Instance.ShowQuestUI(); break;

                case DialogueActionType.GiveGift:
                    Debug.Log("아이템 지급"); break;

                case DialogueActionType.IncreaseAffinity:
                    Debug.Log("호감도 증가"); break;

                case DialogueActionType.OpenShop:
                    Debug.Log("상점 열기");
                    PlayerInventory.Instance.ShowShop(); break;

                case DialogueActionType.CompleteQuest:
                    Debug.Log("퀘스트 완료");
                    QuestManager.Instance.CompleteQuest(); break;
            }
        }

        private void EndDialogue()
        {
            Hide();
            dialogueUis[0].Close();
            dialogueUis[1].Close();
            onDialogueEnd?.Invoke();
        }

        private void ClearChoiceButtons()
        {
            foreach (var button in choiceButtons)
                button.gameObject.SetActive(false);
        }

        public bool IsDialogueActive => canvasGroup.alpha > 0.9f;
    }
}
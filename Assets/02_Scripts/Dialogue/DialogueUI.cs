using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        public NpcName? currentNpcId;
        public GameObject nameObj;
        public TMP_Text nameTextUI;
        public Image portraitImage;
        public GameObject inactiveObj;
        public DialogueUI pairDialogueUI;
        public void Show(Npc.NpcData npcData)
        {
            if (pairDialogueUI.currentNpcId == npcData.npcNameType)
                Swap(pairDialogueUI);

            if (currentNpcId == npcData.npcNameType)
                return;

            currentNpcId = npcData.npcNameType;

            if (!nameObj.activeSelf) nameObj.SetActive(true);
            if (!portraitImage.gameObject.activeSelf) portraitImage.gameObject.SetActive(true);
            if (inactiveObj.activeSelf) inactiveObj.SetActive(false);

            pairDialogueUI.Hide();

            portraitImage.sprite = npcData.npcPortrait;
            nameTextUI.text = npcData.npcName;
        }
        public void Swap(DialogueUI targetUI)
        {
            targetUI.nameTextUI.text = nameTextUI.text;
            targetUI.portraitImage.sprite = portraitImage.sprite;
            targetUI.currentNpcId = currentNpcId;
        }
        public void Hide()
        {
            if (nameObj.activeSelf) inactiveObj.SetActive(true);
        }
        public void Close()
        {
            nameObj.SetActive(false);
            portraitImage.gameObject.SetActive(false);
            inactiveObj.SetActive(false);

            currentNpcId = null;
        }
    }
}
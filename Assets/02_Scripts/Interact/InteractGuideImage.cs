using System.Collections.Generic;
using UnityEngine;

public class InteractGuideImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer guideImage;

    List<Sprite> imageList = new List<Sprite>();

    public void Show(Sprite sprite)
    {
        guideImage.color = Color.white;

        guideImage.sprite = sprite;

        imageList.Add(sprite);
    }
    public void Hide(Sprite sprite)
    {
        int index = imageList.LastIndexOf(sprite);

        if (index == -1) return;

        imageList.RemoveAt(index);

        if (imageList.Count == 0)
        {
            guideImage.sprite = null;
            guideImage.color = new Color(0, 0, 0, 0); return;
        }

        if (guideImage.sprite.Equals(sprite))
        {
            guideImage.sprite = imageList[imageList.Count - 1];
        }
    }
}

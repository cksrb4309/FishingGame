using UnityEngine;
using System.Collections.Generic;

public class UIManager
{
    static UIManager instance = new();
    public static bool IsUIOpen
    {
        get => instance.activePanels.Count > 0;
    }

    List<UIPanel> activePanels = new();

    public static void OnShow(UIPanel uiPanel)
    {
        List<UIPanel> requiredPanels = uiPanel.RequiredPanels;
        List<UIPanel> activePanels = instance.activePanels;

        if (activePanels.Contains(uiPanel))
        {
            for (int i = activePanels.Count - 1; i >= 0; i--)
            {
                if (activePanels[i].Equals(uiPanel)) break;

                else
                {
                    activePanels[i].Hide(false);

                    activePanels.RemoveAt(i);
                }
            }
        }
        else
        {
            for (int i = 0; i < requiredPanels.Count; i++)
            {
                if (!activePanels.Contains(requiredPanels[i]))
                {
                    if (i > 0)
                    {
                        int lastIndex = activePanels.FindIndex(p => p == requiredPanels[i]);

                        for (int j = lastIndex + 1; j < activePanels.Count; j++)
                            activePanels[j].Hide();

                        if (lastIndex < activePanels.Count - 1)
                            activePanels.RemoveRange(lastIndex + 1, activePanels.Count - lastIndex + 1);
                    }
                }
            }

            for (int i = 0; i < requiredPanels.Count; i++)
            {
                if (!activePanels.Contains(requiredPanels[i]))
                {
                    activePanels.Add(requiredPanels[i]);

                    requiredPanels[i].Show(false);
                }
            }
            activePanels.Add(uiPanel);
        }
        instance.activePanels = activePanels;
    }
    public static void OnHide(UIPanel uiPanel)
    {
        List<UIPanel> activePanels = instance.activePanels;

        for (int i = activePanels.Count - 1; i >= 0; i--)
        {
            activePanels[i].Hide(false);

            if (activePanels[i].Equals(uiPanel))
            {
                activePanels.RemoveAt(i);

                break;
            }
            else
            {
                activePanels.RemoveAt(i);
            }
        }
        instance.activePanels = activePanels;
    }
    public static void OnHideAll()
    {
        List<UIPanel> activePanels = instance.activePanels;

        for (int i = activePanels.Count - 1; i >= 0; i--)
        
            activePanels[i].Hide(false);

        instance.activePanels = new();
    }
}

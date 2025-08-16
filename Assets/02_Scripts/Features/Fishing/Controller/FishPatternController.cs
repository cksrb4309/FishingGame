using System.Collections;
using TMPro;
using UnityEngine;

public class FishPatternController : MonoBehaviour
{
    public static FishPatternController Instance { get; private set; } = null;
    [SerializeField] float patternStartDelay = 1f;
    [SerializeField] TMP_Text fishHpTextUI;
    FishingMethodData_Game_4 gameData;
    Coroutine patternCoroutine = null;
    bool isAlive = false;
    int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0) fishHpTextUI.text = "0";
            else fishHpTextUI.text = hp.ToString();

            if (hp <= 0) Complete();
        }
    }
    int hp;
    private void Awake()
    {
        Instance = this;
    }
    public void ReceiveDamage(int damage)
    {
        Hp -= damage;
    }
    public void Setting()
    {
        isAlive = true;

        gameData = (FishingMethodData_Game_4)FishingData.MethodData;

        gameData.fishPattern.Init();

        Hp = gameData.maxHp;

        patternCoroutine = StartCoroutine(PatternCoroutine());
    }
    IEnumerator PatternCoroutine()
    {
        yield return new WaitForSeconds(patternStartDelay);

        FishPattern fishPattern = gameData.fishPattern;

        while (true) yield return new WaitForSeconds(fishPattern.SpawnPattern());
    }
    public void Cancel()
    {
        gameData.fishPattern.Reset();

        if (patternCoroutine != null) StopCoroutine(patternCoroutine);
    }
    void Complete()
    {
        if (!isAlive) return;

        isAlive = false;

        Fishing_Game.fishing_Game.Complete();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() => Inst = this;
    
    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] EturnMode eTurnMode;

    [SerializeField] [Tooltip("카드 배분이 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")] 
    public bool isLoading;
    public bool myTurn;
    enum EturnMode {My, other}
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> onAddCard;

    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);
        
        switch (eTurnMode)
        {
            case EturnMode.My:
                myTurn = true;
                break;
            case EturnMode.other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;
        
        for (int i = 0; i < startCardCount; i++)
        {
            //yield return delay05;
            //onAddCard?.Invoke(false);
            yield return delay05;
            onAddCard?.Invoke(true);
        }

        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;
        yield return delay07;
        onAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
    }
}

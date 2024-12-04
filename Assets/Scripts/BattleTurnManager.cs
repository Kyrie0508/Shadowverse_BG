using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleTurnManager : MonoBehaviour
{
    public static BattleTurnManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Settings")]
    [SerializeField] float turnDelay = 1.5f; // 턴 간 대기 시간
    [SerializeField] float attackDelay = 0.5f; // 공격 간 대기 시간

    [Header("References")]
    [SerializeField] CardManager cardManager;
    public static Action<bool> OnAddCard;

    void Start()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        for (int i = 0; i < 5; i++)
        {
            new WaitForSeconds(0.7f);
            OnAddCard?.Invoke(false);
        }
        while (true)
        {
            yield return StartCoroutine(ExecuteTurn(true)); // 플레이어의 턴
            yield return new WaitForSeconds(turnDelay);
            yield return StartCoroutine(ExecuteTurn(false)); // 적의 턴
            yield return new WaitForSeconds(turnDelay);
        }
    }

    IEnumerator ExecuteTurn(bool isPlayerTurn)
    {
        List<Card> attackerCards = isPlayerTurn ? cardManager.myCards : cardManager.otherCards;
        List<Card> targetCards = isPlayerTurn ? cardManager.otherCards : cardManager.myCards;

        foreach (var attacker in attackerCards)
        {
            if (attacker == null || attacker.IsDead()) continue;

            // 랜덤한 대상 선택
            if (targetCards.Count == 0) break; // 대상 카드가 없으면 종료
            Card target = targetCards[Random.Range(0, targetCards.Count)];

            // 공격 처리
            yield return Attack(attacker, target);

            // 대상이 죽었는지 확인
            if (target.IsDead())
            {
                targetCards.Remove(target);
                Destroy(target.gameObject);
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    IEnumerator Attack(Card attacker, Card target)
    {
        Debug.Log($"{attacker.name} attacks {target.name} for {attacker.GetAttackPower()} damage!");
        target.TakeDamage(attacker.GetAttackPower());

        // 시각적 효과나 애니메이션 처리 (Optional)
        yield return new WaitForSeconds(0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseCardManager : MonoBehaviour
{
    public static PurchaseCardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("References")]
    [SerializeField] GameObject cardPrefab; // 구매 가능한 카드의 프리팹
    [SerializeField] Transform purchaseArea; // 구매 카드가 표시될 영역
    [SerializeField] Transform leftAlign; // 정렬 시작 지점
    [SerializeField] Transform rightAlign; // 정렬 끝 지점
    [SerializeField] int maxDisplayCards = 6; // 최대 표시 카드 수

    List<Card> purchaseCards = new List<Card>();
    public Card selectCard;

    void Start()
    {
        SetupPurchaseDeck(); // 구매 더미 초기화
        AlignPurchaseCards(); // 카드 정렬
    }

    void SetupPurchaseDeck()
    {
        // 예시: 10개의 카드 생성
        for (int i = 0; i < maxDisplayCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, purchaseArea);
            Card card = cardObj.GetComponent<Card>();
            card.Setup(CardManager.Inst.PopItem(), isMine: false);
            purchaseCards.Add(card);
        }
    }

    public void AlignPurchaseCards()
    {
        if (purchaseCards.Count == 0) return;

        // 카드 간 간격 계산
        float spacing = Mathf.Clamp(1f / (maxDisplayCards - 1), 0.1f, 0.3f);
        float[] lerps = new float[purchaseCards.Count];

        // 카드 위치 계산
        for (int i = 0; i < purchaseCards.Count; i++)
        {
            lerps[i] = Mathf.Clamp01((float)i / (purchaseCards.Count - 1));
            Vector3 position = Vector3.Lerp(leftAlign.position, rightAlign.position, lerps[i]);
            purchaseCards[i].MoveTransform(new PRS(position, Utils.QI, Vector3.one * 1.3f), true, 0.5f);
        }
    }

    public bool TryPurchaseCard(Card card)
    {
        if (!purchaseCards.Contains(card)) return false;

        // 카드 구매 처리
        selectCard = card;
        purchaseCards.Remove(card);
        CardManager.Inst.AddCard(true); // 플레이어의 패에 추가
        Destroy(card.gameObject); // 구매한 카드 제거
        AlignPurchaseCards(); // 남은 카드 정렬
        if (purchaseCards.Count == 0)
        {
            Start();
        }
        return true;
    }
}
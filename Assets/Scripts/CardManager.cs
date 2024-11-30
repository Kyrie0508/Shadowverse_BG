using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CardManager : MonoBehaviour {
    public static CardManager Inst { get; private set; } // 매니저는 하나만 존재하기 때문에 싱글톤으로 선언.
    void Awake() => Inst = this;
 
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] private List<Card> Mycards;
    [SerializeField] private List<Card> Othercards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    List<Item> itemBuffer;
 
    public Item PopItem() {
        if (itemBuffer.Count == 0) { // 만약 카드를 다 뽑아 버퍼가 가지고 있는 카드의 갯수가 0개가 되면
            SetupItemBuffer(); // 다시 새로 버퍼에 100장의 카드를 셋팅
        }
 
        Item item = itemBuffer[0]; // 버퍼 맨 앞에 있는 카드를 뽑는다.
        itemBuffer.RemoveAt(0); // 뽑은 카드를 버퍼에서 지운다.
        return item; // 카드를 뽑아낸다.
    }
 
    void SetupItemBuffer() {
        itemBuffer = new List<Item>(100);
        for(int i = 0; i < itemSO.items.Length; i++) { // item 배열에 담겨있는 10개의 카드
            Item item = itemSO.items[i]; // 10개의 카드를 가져온다.
            for(int j = 0; j < item.num; j++) { // 각각의 카드만큼의 퍼센트 만큼 반복시킨다
                itemBuffer.Add(item); // 총 100장의 카드가 들어가며 각가의 카드 갯수는 각 카드의 퍼센트만큼 들어간다.
            }
        }
 
        for(int i = 0; i < itemBuffer.Count; i++) { // 순서대로 들어가있는 카드를 랜덤하게 섞어준다.
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }
 
    void Start() {
        SetupItemBuffer();
        TurnManager.onAddCard += Addcard;
    }

    void OnDestroy()
    {
        TurnManager.onAddCard -= Addcard;
    }
 
    void Update() 
    {
    
        
    }
 
    void Addcard(bool isMine) {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem());
        (isMine ? Mycards : Othercards).Add(card);
        
        SetOriginOrder(isMine);
        CardAlignment(isMine);
    }

    void SetOriginOrder(bool isMine)
    {
        int count = isMine ? Mycards.Count : Othercards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? Mycards[i] : Othercards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment(bool isMine)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundingAlignment(myCardLeft, myCardRight, Mycards.Count, 0.5f, Vector3.one);
        var targetCards = isMine ? Mycards : Othercards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundingAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.27f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                //targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    } 
    
}
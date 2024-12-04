using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] Sprite cardFront;

    public Item item;
    bool isFront;
    public PRS originPRS;


    public void Setup(Item item, bool isMine)
    {
        this.item = item;
        
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        attackTMP.text = this.item.attack.ToString();
        healthTMP.text = this.item.health.ToString();
        costTMP.text = this.item.cost.ToString();

    }

    void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        CardManager.Inst.CardMouseDown();
    }

    void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp();
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public int GetAttackPower()
    {
        return this.item.attack;
    }

    public void TakeDamage(int damage)
    {
        this.item.health -= damage;
        Debug.Log($"{this.item.name} took {damage} damage. Remaining health: {this.item.health}");
    }

    public bool IsDead()
    {
        return this.item.health <= 0;
    }
}

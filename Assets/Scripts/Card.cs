using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text attack;
    [SerializeField] TMP_Text HP;

    public Item item;
    public PRS originPRS;

    public void Setup(Item item)
    {
        this.item = item;
        character.sprite = item.sprite;
        name.text = item.name;
        attack.text = item.attack.ToString();
        HP.text = item.health.ToString();

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


}

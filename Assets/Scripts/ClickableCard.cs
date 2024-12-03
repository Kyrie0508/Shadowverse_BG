using UnityEngine;

public class ClickableCard : MonoBehaviour
{
    private System.Action onClick;

    public void Init(System.Action onClickAction)
    {
        onClick = onClickAction;
    }

    void OnMouseUpAsButton()
    {
        onClick?.Invoke();
    }
}
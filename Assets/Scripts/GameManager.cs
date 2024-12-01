using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;

    
    [SerializeField] NotificationPanel notificationPanel;

    void Start()
    {
        StartGame();
    }

    void Update()
    { 
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) 
            TurnManager.onAddCard?.Invoke(true);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            TurnManager.onAddCard?.Invoke(false);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            TurnManager.Inst.EndTurn();
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }
}

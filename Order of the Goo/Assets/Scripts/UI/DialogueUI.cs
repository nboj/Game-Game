using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    private DialogueConversant dialogueConversant;
    
    private void Start() {
        dialogueConversant = GameObject.FindWithTag("Player").GetComponent<DialogueConversant>();
        dialogueText.text = dialogueConversant.GetText();
        nextButton.onClick.AddListener(Next);
    } 

    private void Next() {
        dialogueText.text = dialogueConversant.Next();
    }
}

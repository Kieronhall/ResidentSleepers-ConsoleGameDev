using UnityEngine;
using TMPro;

namespace ThirdPerson
{
    public class PlayerUI : MonoBehaviour
    {
        // Text element to display prompt messages
        [SerializeField] private TextMeshProUGUI promptText;

        // Method to update the prompt text
        public void UpdateText(string promptMessage)
        {
            // Set the text to the provided prompt message
            promptText.text = promptMessage;
        }
    }
}
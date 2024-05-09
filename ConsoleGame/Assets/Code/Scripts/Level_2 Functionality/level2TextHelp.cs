using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class level2TextHelp : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    public bool levelComplete;

    public openGate opengate;
    public rhythmGameManager rhythmgamemanager;
    public lootKeyToHelicopter lootkeytohelicopter;
    
    public string textOne = "We've gotta figure out a way to get your friend out of his cell!";
    public string textTwo = "The alarm locked the doors! Is there a way to bypass these locks?";
    public string textThree = "We need to make it to the top, the guards seemed to be protecting something on the first floor.";
    public string textFour = "Time to get out of here!";

    void Start()
    {
        tmpText.text = textOne;
    }
    
    void Update()
    {
        if (opengate.hasTriggered)
        {
            tmpText.text = textTwo;
        }
        if (rhythmgamemanager.gameComplete)
        {
            tmpText.text = textThree;
        }
        if (lootkeytohelicopter.keyComplete)
        {
            tmpText.text = textFour;
        }
    }
}

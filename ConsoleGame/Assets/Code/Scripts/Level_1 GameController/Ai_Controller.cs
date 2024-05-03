using UnityEngine;

public class Ai_Controller : MonoBehaviour
{
    public GameObject Exterior_Agents;

    public GameObject Interior_Agents;

    public GameObject Security_Agents;

    // Start is called before the first frame update
    void Start()
    {
        Interior_Agents.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsideAgentsOn()
    {
        Interior_Agents.SetActive(true);
        Exterior_Agents.SetActive(false);
    }

    public void ChasePlayer()
    {
        Security_Agents.SetActive(true);
    }
}

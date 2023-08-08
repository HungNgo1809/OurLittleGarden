using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetNPCInteract : MonoBehaviour
{
    public List<NPCChat> chatList;
    public NPCChat closestNPC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindClosestNPC(GameObject player)
    {
        float closestDistance = Mathf.Infinity;

        foreach (NPCChat npcChat in chatList)
        {
            float distance = Vector3.Distance(player.transform.position, npcChat.NPC.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNPC = npcChat;
            }
        }

        if (closestNPC != null && closestNPC.NPC.name != "Emma")
        {
            if(closestNPC.NPC.name == "Isabella" && UiManager.Instance.inventory.activeSelf != true)
            {
                closestNPC.chatPanel.SetActive(true);
            }
            else
            {
                closestNPC.chatPanel.SetActive(true);
            }
          
            // You can perform additional actions for the closest NPC here
        }
        Debug.Log("here");  
    }
    [System.Serializable]
    public class NPCChat
    {
        public GameObject NPC;
        public GameObject chatPanel;
    }
}

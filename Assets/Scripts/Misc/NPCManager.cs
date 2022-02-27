using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private int _npcCount = 0;
    private int _livingNPCs = 0;
    private int _savedNPCs = 0;

    public void AddLivingNPC()
    {
        _npcCount++;
        _livingNPCs++;
    }

    public void OnNPCDeath()
    {
        _livingNPCs--;
        Debug.LogFormat("NPC died. {0} left to save", _npcCount - _livingNPCs - _savedNPCs);
    }

    public void OnNPCSaved()
    {
        _savedNPCs++;
        Debug.LogFormat("NPC saved. {0} left to save", _npcCount - _livingNPCs - _savedNPCs);
    }
}

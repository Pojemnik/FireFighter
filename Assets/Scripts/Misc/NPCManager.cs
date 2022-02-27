using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private int _npcCount = 0;
    private int _npcsLeft = 0;
    private int _savedNPCs = 0;

    public void AddLivingNPC()
    {
        _npcCount++;
        _npcsLeft++;
    }

    public void OnNPCDeath()
    {
        _npcsLeft--;
        Debug.LogFormat("NPC died. {0} left to save", _npcsLeft);
    }

    public void OnNPCSaved()
    {
        _savedNPCs++;
        _npcsLeft--;
        Debug.LogFormat("NPC saved. {0} left to save", _npcsLeft);
    }
}

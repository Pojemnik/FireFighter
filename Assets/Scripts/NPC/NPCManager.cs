using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private int _npcCount = 0;
    private int _npcsLeft = 0;
    private int _savedNPCs = 0;

    public UnityEngine.Events.UnityEvent<(int,int)> GameOverEvent;

    public void AddLivingNPC()
    {
        _npcCount++;
        _npcsLeft++;
    }

    public void OnNPCDeath()
    {
        _npcsLeft--;
        Debug.LogFormat("NPC died. {0} left to save", _npcsLeft);
        if(_npcsLeft == 0)
        {
            GameOverEvent.Invoke((_savedNPCs, _npcCount));
        }
    }

    public void OnNPCSaved()
    {
        _savedNPCs++;
        _npcsLeft--;
        Debug.LogFormat("NPC saved. {0} left to save", _npcsLeft);
        if (_npcsLeft == 0)
        {
            GameOverEvent.Invoke((_savedNPCs, _npcCount));
        }
    }
}

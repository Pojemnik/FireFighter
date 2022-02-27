using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _statsLabel;

    public void OnGameOver((int saved, int total) stats)
    {
        _statsLabel.text = string.Format("You saved {0} out of {1} people. Congratulations!", stats.saved, stats.total);
    }
}

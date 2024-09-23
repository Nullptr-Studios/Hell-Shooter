using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class UpgradeButtons : MonoBehaviour
{
    public StatID statId;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void LevelUp()
    {
        player.BroadcastMessage("StatLevelUp", statId);
    }
    public void LevelDown()
    {
        player.BroadcastMessage("StatLevelDown", statId);
    }
}

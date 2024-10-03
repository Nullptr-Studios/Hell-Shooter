using ToolBox.Serialization;
using UnityEngine;

// Most useful code file on the entire project -x
public class Reinforcer : MonoBehaviour
{
    public Sprite baseLevelTexture;
    public Sprite maxLevelTexture;

    private void Start()
    {
        if (DataSerializer.Load<float>(SaveDataKeywords.healthLevel) >= 3)
            GetComponent<SpriteRenderer>().sprite = maxLevelTexture;
    }
}

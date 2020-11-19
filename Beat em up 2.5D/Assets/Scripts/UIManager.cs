using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Player player;

    public Slider slider;
    public Image playerImage;
    public Text lives;

    public Image bossUnderHealthBar;
    public Image bossUpperHealthBar;
    public Text bossName;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

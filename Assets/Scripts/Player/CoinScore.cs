using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScore : MonoBehaviour, IScore
{
    [SerializeField] private int score = 100;
    [SerializeField] AudioSource audioS;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int AddScore()
    {
        audioS.Play();
        col.enabled = false;
        spriteRenderer.sprite = null;
        return score;
    }
    
}

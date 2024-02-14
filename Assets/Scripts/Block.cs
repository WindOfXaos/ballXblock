using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Config Parameters
    [SerializeField] AudioClip[] breakSound;
    [SerializeField] float volume;
    [SerializeField] bool boss;
    [SerializeField] GameObject blockSparkleVFX;
    [SerializeField] Sprite[] hitSprites;

    //Cached Refrences
    Level level;
    Vector2 pos;

    //States
    float signal = 1;
    int bossSpeed = 30 * 60;
    int frame = 0;
    [SerializeField] int timesHit; //only for debugging

    void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (CompareTag("Breakable"))
        {
            level.CountBlocks();
        }
    }

    void Update()
    {
        if ((boss) && (frame % bossSpeed == 0))
        {
            Move();
        }
        frame++;
    }

    private void Move()
    {
        pos = gameObject.transform.position;
        AlternateMove();
        gameObject.transform.position = pos;
    }

    private void AlternateMove()
    {
        pos.x += signal;
        signal *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag("Breakable"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block " + gameObject.name + " sprite is missing from array!");

        }
    }

    private void DestroyBlock()
    {
        FindObjectOfType<GameSession>().AddToScore();
        level.BlockDestroyed();
        PlayBlockDestroyVFX();
        TriggerSparklesVFX();
        Destroy(gameObject);
    }

    private void PlayBlockDestroyVFX()
    {
        AudioClip clip = breakSound[UnityEngine.Random.Range(0, breakSound.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    private void TriggerSparklesVFX()
    {
        Color blockColor = gameObject.GetComponent<SpriteRenderer>().color;
        ParticleSystem.MainModule settings = blockSparkleVFX.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(blockColor);
        GameObject sparkles = Instantiate(blockSparkleVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}

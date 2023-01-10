using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteHandler : MonoBehaviour
{
    private static Object[] bodies;
    private static Object[] eyesArr;
    private static Object[] hairArr;
    private static Object[] outfits;

    private Texture2D spriteTextureMap;
    private Sprite[] idle = new Sprite[24];
    private Sprite[] walk = new Sprite[24];
    private SpriteRenderer spr;

    private float prevAnimUpdate = 0;
    private int currentFrame = 0;

    [SerializeField]
    public bool walking = false;
    [SerializeField]
    public int facingDirection = 0;

    private void Awake()
    {
        if (bodies == null)
        {
            bodies = Resources.LoadAll("Bodies");
            eyesArr = Resources.LoadAll("Eyes");
            hairArr = Resources.LoadAll("Hair");
            outfits = Resources.LoadAll("Outfit");
        }
    }

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        CreateSprite();
        DivideTexture();

        spr.sprite = idle[0];
    }

    void Update()
    {
        if(Time.time > prevAnimUpdate + 0.2)
        {
            prevAnimUpdate = Time.time;
            if (currentFrame >= 5)
            {
                currentFrame = 0;
            }
            else
            {
                currentFrame += 1;
            }
        }
        
        if(walking)
        {
            spr.sprite = walk[facingDirection * 6 + currentFrame];
        } else
        {
            spr.sprite = idle[facingDirection * 6 + currentFrame];
        }
    }

    void CreateSprite()
    {
        
        Texture2D body = bodies[Random.Range(0, bodies.Length)] as Texture2D;
        Texture2D eyes = eyesArr[Random.Range(0, eyesArr.Length)] as Texture2D;
        Texture2D hair = hairArr[Random.Range(0, hairArr.Length)] as Texture2D;
        Texture2D outfit = outfits[Random.Range(0, outfits.Length)] as Texture2D;

        Color[] c = body.GetPixels(0, 0, 896, 656);
        body = new Texture2D(896, 656);
        body.SetPixels(c);
        body.Apply();

        spriteTextureMap = body.AlphaBlend(outfit).AlphaBlend(hair).AlphaBlend(eyes);
        spriteTextureMap.filterMode = FilterMode.Point;
    }

    void DivideTexture()
    {
        for(int i = 1; i < 3; i++)
        {
            for (int j = 0; j < 24; j++)
            {
                Sprite newSprite = Sprite.Create(spriteTextureMap, new Rect(j * 16, 656 - 32 - i * 32, 16, 32), new Vector2(0.5f, 0.5f), 16);
                if(i == 1)
                {
                    idle[j] = newSprite;
                }
                else
                {
                    walk[j] = newSprite;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sprite_spawner : MonoBehaviour
{
    private Sprite appleSprite, cheeseSprite, cookieSprite, baconSprite;
    private SpriteRenderer sr;
    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
    }

    void Start()
    {
        appleSprite = Resources.Load<Sprite> ("Food/Apple");
        cheeseSprite = Resources.Load<Sprite> ("Food/Cheese");
        cookieSprite = Resources.Load<Sprite> ("Food/Cookie");
        baconSprite = Resources.Load<Sprite> ("Food/Bacon");
    }

    public void LoadSprite(Food food, float queue_number, float order_number)
    {
        transform.position = new Vector3(order_number, queue_number, 1.0f);
        if (food == Food.Apple)
        {
            sr.sprite = appleSprite;
        }
        else if (food == Food.Cheese) {
            sr.sprite = cheeseSprite;
        }
        else if (food == Food.Cookie) {
            sr.sprite = cookieSprite;
        }
        else if (food == Food.Bacon) {
            sr.sprite = baconSprite;
        }
        sr.enabled = true;
    }

    public void DeloadSprite(){
        sr.enabled = false;
    }
}
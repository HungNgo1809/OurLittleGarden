using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{
    public Sprite cursorSprite; // The sprite to use for the cursor

    private SpriteRenderer spriteRenderer; // The sprite renderer component

    void Start()
    {
        spriteRenderer =  GetComponent<SpriteRenderer>();
        // Create a new game object to hold the cursor sprite
        spriteRenderer.sprite = cursorSprite;

        // Hide the hardware cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Move the cursor sprite to the mouse position
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.transform.position = cursorPos;
    }
}

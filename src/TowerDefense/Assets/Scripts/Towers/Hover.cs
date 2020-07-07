using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer rangeSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    /// <summary>
    /// Follows the cursor with the current sprite
    /// </summary>
    private void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    /// <summary>
    /// Renders the sprite of current tower
    /// </summary>
    /// <param name="sprite">Tower sprite</param>
    /// <param name="size">Tower size</param>
    public void Show(Sprite sprite, Vector3 size)
    {
        this.spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        rangeSpriteRenderer.enabled = true;
        rangeSpriteRenderer.gameObject.transform.localScale = size;
    }

    /// <summary>
    /// Removes effects on hover
    /// </summary>
    public void Hide()
    {
        this.spriteRenderer.enabled = false;
        this.rangeSpriteRenderer.enabled = false;
    }
}

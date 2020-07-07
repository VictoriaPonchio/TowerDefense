using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Tiles : MonoBehaviour
{
    [SerializeField]
    private int position;
    public bool isEmpty { get; set; }
    private Color32 red = new Color32(255, 118, 118, 255);
    private Color32 green = new Color32(96, 255, 90, 255);
    private SpriteRenderer spriteRenderer;
    private Tower childTower;
    private ProjectileTower projTower;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /// <summary>
    /// Create a tower on level
    /// </summary>
    private void PlaceTower()
    {
        if (GameManager.Instance.Currency >= GameManager.Instance.ClickedTower.Cost)
        {
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedTower.TowerPrefab, spriteRenderer.bounds.center, Quaternion.identity);
            tower.transform.SetParent(transform);
            this.isEmpty = false;
            this.childTower = tower.transform.GetChild(0).GetComponent<Tower>();

            //Check sprite for area damage/debuff
            if (childTower.transform.childCount > 0)
            {
                var childSpriteRender = childTower.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                childSpriteRender.enabled = true;
            }
           
            spriteRenderer.color = Color.white;
            GameManager.Instance.BuyTower(GameManager.Instance.ClickedTower.Cost);
        }
    }

    #region events
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTower != null)
        {
            if (isEmpty && gameObject.tag != "EnemyPath")
            {
                spriteRenderer.color = green;
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            else
            {
                spriteRenderer.color = red;
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (childTower != null)
            {
                GameManager.Instance.SelectPlacedTower(childTower);
            }
            else
            {
                GameManager.Instance.DeselectPlacedTower();
            }
        }

    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    #endregion

}

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
public class Matchable : Movable
{
    [SerializeField] private Vector2Int dimensions = Vector2Int.one;
    private MatchablePool pool;
    private MatchableGrid grid;
    private GameManager gManager;

    public GameObject particleObj;

    private int type;
    public int Type
    {
        get
        {
            return type;
        }
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        gManager = GameManager.instance;
    }
    public static bool IsClickable = true;
    private void OnMouseDown()
    {
        if (grid.aniScale != null)
        {
            StopCoroutine(grid.aniScale);
        }

        if (!IsClickable) return;
        GameObject gameObjSlot = this.gameObject;

        Vector3 selectedScaleGameObj = new Vector3(1.3f, 1.3f);//bigger
        gameObject.GetComponent<Transform>().localScale = selectedScaleGameObj;

        //Color selectedColorGameObj = new Color(1, 1, 0, 1);//yellow
        //gameObject.GetComponent<SpriteRenderer>().color = selectedColorGameObj;

        Vector3 selectSlots = new Vector3();
        Debug.Log("clicked: " + gameObject.transform.localPosition);

        Sprite gameobjectSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log("this sprite is: " + gameobjectSprite);
        selectSlots = gameObject.transform.position;
        Debug.Log("PositionXY "+"x: " + gameObject.transform.position.x + "y: " + gameObject.transform.position.y);
        Debug.Log("list selected " + gManager.posSelectedList.Count);

        gManager.posSelectedList.Add(selectSlots);
        gManager.spriteSlotList.Add(gameobjectSprite);
        gManager.slotsSelectedList.Add(gameObjSlot);

        if (gManager.posSelectedList.Count == 2)
        {
            if(!CheckSprite(gameobjectSprite))
            {
                ResetColorNScale();
                ClearSelectedSlot();
                Debug.Log("Clear 2 false");
            }
        }
        else if (gManager.posSelectedList.Count >= 3)
        {
            if(!CheckSlot(gManager.posSelectedList.Count, gameObject.transform.position.x, gameObject.transform.position.y)
                ||!CheckSprite(gameobjectSprite)
                ||!CheckRowColumStraight(gameObject.transform.position.x, gameObject.transform.position.y))
            {
                ResetColorNScale();
                ClearSelectedSlot();
                Debug.Log("Clear 3 false");
            }
            else
            {
                if (gManager.posSelectedList.Count == 4)
                {
                    gManager.min_x = (int)gManager.posSelectedList[0].x;
                    gManager.max_x = (int)gManager.posSelectedList[0].x;
                    gManager.min_y = (int)gManager.posSelectedList[0].y;
                    gManager.max_y = (int)gManager.posSelectedList[0].y;
                    for (int i = 1; i < gManager.posSelectedList.Count; i++)
                    {
                        if (gManager.min_x > gManager.posSelectedList[i].x)
                        {
                            gManager.min_x = (int)gManager.posSelectedList[i].x;
                        }

                        if (gManager.min_y > gManager.posSelectedList[i].y)
                        {
                            gManager.min_y = (int)gManager.posSelectedList[i].y;
                        }
                    }

                    for (int i = 1; i < gManager.posSelectedList.Count; i++)
                    {
                        if (gManager.posSelectedList[i].x > gManager.max_x)
                        {
                            gManager.max_x = (int)gManager.posSelectedList[i].x;
                        }

                        if (gManager.posSelectedList[i].y > gManager.max_y)
                        {
                            gManager.max_y = (int)gManager.posSelectedList[i].y;
                        }
                    }

                    grid.EatSquare(gManager.min_x, gManager.min_y, gManager.max_x, gManager.max_y);
                    gManager.isEatSquare = true;
                    IsClickable = false;

                    Debug.Log("min_x: " + gManager.min_x + ", min_y: " + gManager.min_y + ", max_x: " + gManager.max_x + ", max_y: " + gManager.max_y);

                    Debug.Log("Clear 4 OK");
                }

                if(gManager.posSelectedList.Count == 5)
                {
                    ResetColorNScale();
                    ClearSelectedSlot();
                }
            }
        }
    }


    private void ResetColorNScale()
    {
        for(int i = 0; i < gManager.slotsSelectedList.Count; i++)
        {
            gManager.slotsSelectedList[i].GetComponent<Transform>().localScale = new Vector3(1f, 1f);
            gManager.slotsSelectedList[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }


    private void ClearSelectedSlot()
    {
        gManager.spriteSlotList.Clear();
        gManager.posSelectedList.Clear();
    }

    private bool CheckSlot(int pos, float x, float y)
    {
        for(int i = 0; i< pos - 1;i++)
        {
            if ((gManager.posSelectedList[i].x == x && gManager.posSelectedList[i].y != y)
                || (gManager.posSelectedList[i].x != x && gManager.posSelectedList[i].y == y))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRowColumStraight(float x, float y)
    {
        if ((gManager.posSelectedList[0].x == x && gManager.posSelectedList[1].x == x)
                || (gManager.posSelectedList[0].y == y && gManager.posSelectedList[1].y == y))
        {
            return false;
        }
        return true;
    }

    private bool CheckSprite(Sprite sprite)
    {
        for(int i = 0; i < gManager.spriteSlotList.Count; i++)
        {
            if (gManager.spriteSlotList[i] != sprite) return false;
            //else return true;
        }
        return true;
    }

    public void SetType(int type,Sprite sprite,Color color)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    private SpriteRenderer spriteRenderer;
}

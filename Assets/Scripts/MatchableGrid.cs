using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MatchableGrid : GridSystem<Matchable>
{
    private MatchablePool pool;
    Matchable matchableOrigin0;
    public List<Matchable> matchableSlotsList = new List<Matchable>();
    public IEnumerator aniScale;

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
    }

    public void PopulateGrid()
    {
        Matchable newMatchable;

        //int x0 = Random.Range(0, 5);
        //int y0 = Random.Range(0, 3);

        //int x1 = Random.Range(x0+1,x0+6);
        //int y1 = y0;

        //int x2 = x0;
        //int y2 = Random.Range(y0+1,y0+4);

        //int x3 = x1;
        //int y3 = y2;
        for(int y=0;y!=Dimension.y;++y)
            for(int x = 0; x != Dimension.x; ++x)
            {
                // get a matchable from the pool
                newMatchable = pool.GetRandomMatchable();
                // position the matchable on screen
                newMatchable.transform.position = transform.position + new Vector3(x,y);
                // activate the matchable
                newMatchable.gameObject.SetActive(true);

                //if (x == x0 && y == y0)
                //{
                //    matchableOrigin0 = newMatchable;
                //}
                //else if ((x == x1 && y == y1)|| (x == x2 && y == y2) || (x == x3 && y == y3))
                //{
                //    newMatchable.gameObject.GetComponent<SpriteRenderer>().sprite = matchableOrigin0.gameObject.GetComponent<SpriteRenderer>().sprite;
                //}
                // place the matchable in the grid
                PutItemAt(newMatchable, x, y);
                matchableSlotsList.Add(newMatchable);
            }
    }

    public IEnumerator CheckResetBoard()
    {
        List<Matchable> guideBlockList = null;

        for (int ii = 0; ii < pool.guideBlockTypeList.Count; ++ii)
        {
            guideBlockList = matchableSlotsList.FindAll(f => f.gameObject.GetComponent<SpriteRenderer>().sprite == pool.guideBlockTypeList[ii]);
            Debug.Log("EndofFrame1A");
yield return new WaitForEndOfFrame();
            Debug.Log("EndofFrame1B");
            if (guideBlockList != null && guideBlockList.Count >= 4)
            {
                for (int index = 0; index < guideBlockList.Count; ++index)
                {
                    Matchable firstBlock = guideBlockList[index];

                    List<Matchable> xPassList = guideBlockList.FindAll(f => f.transform.position != firstBlock.transform.position && f.transform.position.y == firstBlock.transform.position.y);
                    List<Matchable> yPassList = guideBlockList.FindAll(f => f.transform.position != firstBlock.transform.position && f.transform.position.x == firstBlock.transform.position.x);
                    Debug.Log("EndofFrame2A");
                    yield return new WaitForEndOfFrame();
                    Debug.Log("EndofFrame2B");
                    for (int xPass = 0; xPass < xPassList.Count; ++xPass)
                    {
                        for (int yPass = 0; yPass < yPassList.Count; ++yPass)
                        {
                            Matchable lastBlock = guideBlockList.Find(f => f.transform.position.x == xPassList[xPass].transform.position.x && f.transform.position.y == yPassList[yPass].transform.position.y);
                            Debug.Log("lastBlock");
                            if (lastBlock != null)
                            {
                                Debug.Log("KeepPlay");
                                yield break;
                            }
                        }
                    }
                    //yield return new WaitForEndOfFrame();
                }
            }
        }
        GameManager.instance.isResetBoard = true;
        Debug.Log("IsResetBoard=true");
        yield break ;
    }

    public IEnumerator CheckHint()
    {
        List<Matchable> guideBlockList = null;

        for (int ii = 0; ii < pool.guideBlockTypeList.Count; ++ii)
        {
            guideBlockList = matchableSlotsList.FindAll(f => f.gameObject.GetComponent<SpriteRenderer>().sprite == pool.guideBlockTypeList[ii]);
            if (guideBlockList != null && guideBlockList.Count >= 4)
            {
                for (int index = 0; index < guideBlockList.Count; ++index)
                {
                    Matchable firstBlock = guideBlockList[index];


                    List<Matchable> xPassList = guideBlockList.FindAll(f => f.transform.position != firstBlock.transform.position && f.transform.position.y == firstBlock.transform.position.y);
                    List<Matchable> yPassList = guideBlockList.FindAll(f => f.transform.position != firstBlock.transform.position && f.transform.position.x == firstBlock.transform.position.x);

                    for (int xPass = 0; xPass < xPassList.Count; ++xPass)
                    {
                        for (int yPass = 0; yPass < yPassList.Count; ++yPass)
                        {
                            Matchable lastBlock = guideBlockList.Find(f => f.transform.position.x == xPassList[xPass].transform.position.x && f.transform.position.y == yPassList[yPass].transform.position.y);
                            Debug.Log("lastBlock");
                            if (lastBlock != null)
                            {
                                GameManager.instance.isHinting = true;
                                firstBlock.GetComponent<Matchable>().particleObj.SetActive(true);
                                xPassList[xPass].GetComponent<Matchable>().particleObj.SetActive(true);
                                yPassList[yPass].GetComponent<Matchable>().particleObj.SetActive(true);
                                lastBlock.GetComponent<Matchable>().particleObj.SetActive(true);

                                //selectedGuideBlockList.Add(firstBlock);
                                //selectedGuideBlockList.Add(xPassList[xPass]);
                                //selectedGuideBlockList.Add(yPassList[yPass]);
                                //selectedGuideBlockList.Add(lastBlock);

                                yield break;
                            }
                        }
                    }
                    //yield return new WaitForEndOfFrame();
                }
            }
        }
        yield break;
    }

    public void ThrowGameObj_ToGrid_AfterEat(float min_x, float min_y, float max_x, float max_y)
    {
        Matchable newMatchable;

        //for (int y = 0; y != Dimension.y; y++)
        //    for (int x = 0; x != Dimension.x; x++)
        //    {
        //        if ((x >= min_x)
        //        && (y >= min_y)
        //        && (x <= max_x)
        //        && (y <= max_y))
        //        {
        //            // get a matchable from the pool
        //            newMatchable = pool.GetRandomMatchable();
        //            // position the matchable on screen
        //            newMatchable.transform.position = transform.position + new Vector3(x, y);
        //            // activate the matchable
        //            newMatchable.gameObject.SetActive(true);
        //            PutItemAt(newMatchable, x, y);
        //        }
        //        // place the matchable in the grid

        //    }
        for (int i = 0; i < matchableSlotsList.Count; i++)
        {
            if ((matchableSlotsList[i].gameObject.transform.position.x >= min_x)
                && (matchableSlotsList[i].gameObject.transform.position.y >= min_y)
                && (matchableSlotsList[i].gameObject.transform.position.x <= max_x)
                && (matchableSlotsList[i].gameObject.transform.position.y <= max_y))
            {
                // get a matchable from the pool
                newMatchable = pool.GetRandomMatchable();
                // position the matchable on screen
                newMatchable.transform.position = matchableSlotsList[i].gameObject.transform.position;
                // activate the matchable
                newMatchable.gameObject.SetActive(true);
                PutItemAt(newMatchable, (int)matchableSlotsList[i].gameObject.transform.position.x, (int)matchableSlotsList[i].gameObject.transform.position.y);
            }
        }
        Debug.Log("Throw gameobj back to grid done");
    }

    public void EatSquare(float min_x, float min_y, float max_x, float max_y)
    {
        for (int i =0; i < matchableSlotsList.Count; i++)
        {
            if ((matchableSlotsList[i].gameObject.transform.position.x >= min_x)
                && (matchableSlotsList[i].gameObject.transform.position.y >= min_y)
                && (matchableSlotsList[i].gameObject.transform.position.x <= max_x)
                && (matchableSlotsList[i].gameObject.transform.position.y <= max_y))
            {
                ReturnGameObject(matchableSlotsList[i]);
                GameManager.instance.scoreEat++;
            }
        }
        for (int i = 0; i < GameManager.instance.slotsSelectedList.Count; i++)
        {
            GameManager.instance.slotsSelectedList[i].GetComponent<Transform>().localScale = new Vector3(1f, 1f);
            GameManager.instance.slotsSelectedList[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        GameManager.instance.spriteSlotList.Clear();
        GameManager.instance.posSelectedList.Clear();
        if (GameManager.instance.scoreEat >= GameManager.instance.scoreTarget)
        {
            GameManager.instance.scoreTarget += 100;
            GameManager.instance.scoreTargetTxt.text = GameManager.instance.scoreTarget.ToString();
        }
        GameManager.instance.timeRemaining = 60.0f;
        GameManager.instance.scoreTxt.text = GameManager.instance.scoreEat.ToString();
    }

    public void ReturnGameObject(Matchable matchable)
    {
        pool.ReturnGameobjecttoPool(matchable);
    }

    public void ReturnGameObject()
    {
        for(int i = 0; i < matchableSlotsList.Count; i++) 
        {
            pool.ReturnGameobjecttoPool(matchableSlotsList[i]);
        }
        matchableSlotsList.Clear();
    }
}

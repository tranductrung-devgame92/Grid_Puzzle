using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchablePool : ObjectPool<Matchable>
{
    [SerializeField] private int howManyTypes;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Color[] colors;
    public List<Sprite> guideBlockTypeList = new List<Sprite>();

    public void RandomizeType(Matchable toRandomize)
    {
        int random = Random.Range(0, howManyTypes);
        toRandomize.SetType(random, sprites[random], colors[random]);
        
        AddBlockTypeList();
    }

    public void AddBlockTypeList()
    {
        for(int i = 0; i < howManyTypes; i++)
        {
            guideBlockTypeList.Add(sprites[i]);
        }
        Debug.Log("TypeList");
    }

    public Matchable GetRandomMatchable()
    {
        Matchable randomMatchable = GetPooledObject();

        RandomizeType(randomMatchable);

        return randomMatchable;
    }

}

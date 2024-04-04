using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private MatchablePool pool;
    private MatchableGrid grid;

    public Button tryAgainBtn;
    public Button hintCountBtn;
    public Button unPauseBtn;

    public Text scoreTxt;
    public Text timeRemainTxt;
    public Text scoreTargetTxt;
    public Text hintCountTxt;

    public GameObject gameOverPanel;
    public GameObject pausePanel;


    public float timeRemaining = 60.0f;
    public int scoreEat = 0;
    public int scoreTarget = 100;
    public int hintCountNum = 1000;

    public float min_x;
    public float max_x;
    public float min_y;
    public float max_y;

    [SerializeField] private Vector2Int dimensions = Vector2Int.one;
    [SerializeField] private Text gridOutput;
    public bool isEatSquare = false;
    public bool isResetBoard = false;
    public bool isHinting = false;

    public List<Vector3> posSelectedList = new List<Vector3>();
    public List<Sprite> spriteSlotList = new List<Sprite>();

    public List<GameObject> slotsSelectedList = new List<GameObject>();

    bool isPaused = false;

    void OnGUI()
    {
        if (isPaused)
        {
            //pausePanel.SetActive(true);
            //Matchable.IsClickable = false;
            //hintCountBtn.interactable = false;
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }

    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        tryAgainBtn.onClick.AddListener(OnRetryClick);
        hintCountBtn.onClick.AddListener(OnCountClick);
        unPauseBtn.onClick.AddListener(OnUnPause);
        scoreTxt.text = "0";
        timeRemainTxt.text = "60.0s";
        scoreTargetTxt.text = scoreTarget.ToString();
        hintCountTxt.text = "Hint : "+ hintCountNum.ToString();
        timeRemaining = 60.0f;
        StartCoroutine(Setup());
        StartCoroutine(TimeRemainingCount());
    }
    private float timer = 0.0f;

    private float timehinting = 0.0f;
    private void Update()
    {
        if (isEatSquare)
        {
            hintCountBtn.interactable = false;
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                Debug.Log("Eated");
                grid.ThrowGameObj_ToGrid_AfterEat(min_x, min_y, max_x, max_y);
                StopCoroutine(grid.CheckResetBoard());
                StartCoroutine(grid.CheckResetBoard());
                isEatSquare = false;
                Matchable.IsClickable = true;
                hintCountBtn.interactable = true;
                timer = 0.0f;
            }
        }

        if (isHinting)
        {
            hintCountBtn.interactable = false;
            timehinting += Time.deltaTime;
            if(timehinting >= 5.0f)
            {
                hintCountBtn.interactable = true;
                timehinting = 0.0f;
                isHinting = false;
            }
        }

    }

    private void FixedUpdate()
    {
        if (isResetBoard)
        {
            isResetBoard = false;
            grid.ReturnGameObject();
            StartCoroutine(Setup());
            Debug.Log("ResetBoard");
        }
    }

    public IEnumerator TimeRemainingCount()
    {
        while (true)
        {
            timeRemaining = timeRemaining - Time.deltaTime;
            timeRemainTxt.text = timeRemaining.ToString("n0") + "s";
            if (timeRemaining <= 0f)
            {
                gameOverPanel.SetActive(true);
                hintCountBtn.interactable = false;
                grid.ReturnGameObject();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Setup()
    {    
        pool.PoolObjects(dimensions.x*dimensions.y*2);
        grid.InitializeGrid(dimensions);
        yield return null;
        grid.PopulateGrid();
        //StartCoroutine(grid.PopulateGrid());
    }

    void OnRetryClick()
    {
        gameOverPanel.SetActive(false);
        hintCountBtn.interactable = true;
        timeRemaining = 60.0f;
        scoreEat = 0;
        scoreTarget = 100;

        scoreTargetTxt.text = scoreTarget.ToString();
        scoreTxt.text = scoreEat.ToString();
        StartCoroutine(TimeRemainingCount());
        StartCoroutine(Setup());
    }

    void OnCountClick()
    {
        if (hintCountNum >= 1)
        {
            hintCountNum--;
            StartCoroutine(grid.CheckHint());
            hintCountTxt.text = "Hint : " + hintCountNum.ToString();
        }else if(hintCountNum <= 0)
        {
            hintCountTxt.text = "Hint : 0";
        }

    }

    void OnUnPause()
    {
        //pausePanel.SetActive(false);
        //Matchable.IsClickable = true;
        //hintCountBtn.interactable = true;
    }
}

using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public List<Chip> boardChips;
    [SerializeField] private List<Sprite> chipSprites;
    public Dropdown dropdown;
    public int chipChecked = 0;
    private Chip firstChecked;
    public bool GameActive = false;
    public GameObject Congratulations;
    public GameObject Losing;
    public int score = 0;
    public Text counter;
    public GameObject Counters;
    public bool CountersVis = false;
    public bool ShowBoard = false;
    public Text Timer;
    private float time;
    public void SetChipList()
    {
        Congratulations.SetActive(false);
        Losing.SetActive(false);
        chipChecked = 0;
        score = 0;
        int value = int.Parse(dropdown.captionText.text);
        List<Chip> AllboardChips = new List<Chip>(GetComponentsInChildren<Chip>());
        boardChips = new List<Chip>(GetComponentsInChildren<Chip>());

        for (int i = 0; i < AllboardChips.Count; i++)
        {
            AllboardChips[i].Hide();
            AllboardChips[i].OpeningCounter = 0;
            if (int.Parse(AllboardChips[i].name.Split('-')[0]) > value)
            {
                AllboardChips[i].Counter.color = new Color(0, 0, 0, 0);
                AllboardChips[i].GetComponent<SpriteRenderer>().color = new Color(225,225,225,0);
                boardChips.Remove(AllboardChips[i]);
            }    
            else
            {
                AllboardChips[i].Counter.color = new Color(0, 0, 0, 1);
                AllboardChips[i].Counter.text = "0";
                AllboardChips[i].GetComponent<SpriteRenderer>().color = new Color(225, 225, 225, 1);
                AllboardChips[i].boardController = gameObject.GetComponent<BoardController>();
            }
            if (CountersVis)
            {
                AllboardChips[i].Counter.color = new Color(0, 0, 0, 0);
            }
        }
        boardChips = InitRandomParis(boardChips);
        if (GameActive && ShowBoard)
        {
            StartCoroutine(ShowBoardMethod(value));
            score -= 50;
        }
        counter.text = $"{score}/{value * 10}";
    }
    public void SwitchActiveGame(bool S)
    {
        GameActive = S;
        if (GameActive)
        {
            time = 0;
            SetChipList();
        }
        else
        {
            time = -1;
        }
    }
    public IEnumerator ShowBoardMethod(int value)
    {
        GameActive = false;
        for (int i = 0; i < boardChips.Count; i++)
        {
            boardChips[i].Open();
        }
        yield return new WaitForSeconds(value/15+0.5f);
        for (int i = 0; i < boardChips.Count; i++)
        {
            boardChips[i].Hide();
        }
        GameActive = true;
    }
    public void CounterVisibility()
    {
        CountersVis = !CountersVis;
        SetChipList();
    }
    public void ShowBoardSwitch()
    {
        ShowBoard = !ShowBoard;
    }
    public IEnumerator CheckChip(Chip chipFrom)
    {
        if (chipChecked == 0)
        {
            firstChecked = chipFrom;
            firstChecked.Open();

            chipChecked = 1;
        }
        else if (chipChecked == 1)
        {
            
            if (firstChecked.boardNumber == chipFrom.boardNumber && firstChecked != chipFrom)
            {
                chipChecked = 2;
                chipFrom.Open();
                firstChecked.Destroy();
                chipFrom.Destroy();
                score += 10;
                if (boardChips.Count == 0)
                {
                    Congratulations.SetActive(true);
                    GameActive = false;
                    time = -1;
                }
                counter.text = $"{score}/{dropdown.captionText.text}0";
                chipChecked = 0;
            }
            else if (firstChecked.boardNumber != chipFrom.boardNumber)
            {
                chipChecked = 2;
                chipFrom.Open();
                yield return new WaitForSeconds(0.5f);
                firstChecked.Hide();
                chipFrom.Hide();
                score = score - firstChecked.OpeningCounter - chipFrom.OpeningCounter;
                firstChecked.OpeningCounter += 1;
                chipFrom.OpeningCounter += 1;
                firstChecked.Counter.text = $"{firstChecked.OpeningCounter}";
                chipFrom.Counter.text = $"{chipFrom.OpeningCounter}";
                counter.text = $"{score}/{dropdown.captionText.text}0";
                chipChecked = 0;
                if (score < -600)
                {
                    Losing.SetActive(true);
                    GameActive = false;
                    time = -1;
                }
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
    private List<Chip> InitRandomParis(List<Chip> inList)
    {
        List<Sprite> openSprites = chipSprites.ConvertAll(p => p);
        List<Chip> shuffleList = new List<Chip>();

        int count = inList.Count;

        System.Random random = new System.Random();

        for (int i = 0; i < count / 2; i++)
        {
            int sprite = random.Next(openSprites.Count);

            shuffleList.Add(inList[0]);
            inList[0].boardNumber = i;
            inList[0].SetOpenImage(openSprites[sprite]);
            inList.RemoveAt(0);

            int pair = random.Next(inList.Count);

            shuffleList.Add(inList[pair]);
            inList[pair].boardNumber = i;
            inList[pair].SetOpenImage(openSprites[sprite]);
            inList.RemoveAt(pair);

            openSprites.RemoveAt(sprite);
        }
        return shuffleList;
    }
    private void Update()
    {
        if (time != -1)
        {
            time += Time.deltaTime;
            Timer.text = $"{(int)time / 60}:{(int)time % 60}";
        }
        
    }
    public void Exit()
    {
        Application.Quit();
    }
}

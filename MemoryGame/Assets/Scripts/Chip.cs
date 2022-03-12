using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour
{
    public int boardNumber;

    public Sprite hideImage;
    public Sprite openImage;

    public BoardController boardController;

    public int OpeningCounter = 0;
    public Text Counter;

    private void Start()
    {
        Counter = GameObject.Find(gameObject.name).GetComponent<Text>();
    }
    private void OnMouseUpAsButton()
    {
        if (boardController == null || !boardController.boardChips.Contains(this) || boardController.chipChecked == 2 || !boardController.GameActive)
          return;
        StartCoroutine(boardController.CheckChip(gameObject.GetComponent<Chip>()));
    }
    public void SetOpenImage(Sprite sprite)
    {
        openImage = sprite;
    }
    public void Destroy()
    {
        boardController.boardChips.Remove(this);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
    }
    public void Hide()
    {
        GetComponent<SpriteRenderer>().sprite = hideImage;
    }
    public void Open()
    {
        GetComponent<SpriteRenderer>().sprite = openImage;
    }
}

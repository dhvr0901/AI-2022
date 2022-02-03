using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scorekeeper : MonoBehaviour
{
    [SerializeField]
    private TMP_Text mScoreText;
    private int mScore = 0;

    public void Update()
    {
        mScoreText.text = "Concerning Collisions: " + mScore;
    }

    public void IncreaseScore()
    {
        mScore++;
    }

}

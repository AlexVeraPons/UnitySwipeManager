using System;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("&lt;https://github.com/AlexVeraPons/UnitySwipeManager")] 


public class SwipeManager : MonoBehaviour
{

    public static Action<string> OnSwipe;
    public static Action<Dictionary<string, float>> OnOnSwipePercentage;


    private bool m_endSwipe = false;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_minDistancePercent = 15; // when this percentage of the screen is exceeded the swipe will trigger
    // percentage is applied to smallest distance width or height
    private float m_minDistance; // the minimum distance to swipe

    private Vector2 m_startPos;
    private Vector2 m_endPos;

    private float m_screenWidth;
    private float m_screenHeight;

    public enum SwipeDirections
    {
        UpDown,
        LeftRight,
        FourDirections,
        EightDirections,
        Percentage
    }

    public SwipeDirections PossibleDirections;
    [Range(0.0f, 100f)]
    [Header("Eight Directions")]

    [SerializeField] private float m_neededSimilarity = 80f; // THIS IS ONLY USED IF POSSIBLE DIRECTIONS IS EIGHT DIRECTIONS

    private Dictionary<string, float> m_percentages = new Dictionary<string, float>();

    void Start()
    {


        // get the screen width and height
        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;

        // set the minimum distance to swipe by percentage of the screen size
        if (m_screenWidth < m_screenHeight)
            m_minDistance = (m_screenWidth * m_minDistancePercent) / 100;
        else
            m_minDistance = (m_screenHeight * m_minDistancePercent) / 100;

        // set the directions and percentages for m_percentages
        m_percentages.Add("Up", 0);
        m_percentages.Add("Down", 0);
        m_percentages.Add("Left", 0);
        m_percentages.Add("Right", 0);


    }

    void Update()
    {
        SwipeUpdate();
    }

    void SwipeUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                m_startPos = touch.position;
                m_endSwipe = false;
            }
            if (Mathf.Abs(Vector2.Distance(m_startPos, touch.position)) >= m_minDistance && m_endSwipe == false)
            {
                m_endPos = touch.position;
                ResetPercentageDictionary();
                PercentageCalculator();
                StateManager();

                m_endSwipe = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                m_startPos = Vector2.zero;
                m_endPos = Vector2.zero;
            }
        }

    }

    // reset the percentages dictionary
    private void ResetPercentageDictionary()
    {
        m_percentages["Up"] = 0;
        m_percentages["Down"] = 0;
        m_percentages["Left"] = 0;
        m_percentages["Right"] = 0;
    }

    // calculate the percentage of the swipe in each direction (up, down, left, right)
    private void PercentageCalculator()
    {
        // calculate the percentage of the swipe in each direction

        // create a dictionary with the directions and their vectors, to later "compare them to the current direction"
        Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>();
        directions.Add("Up", new Vector2(0, 1));
        directions.Add("Right", new Vector2(1, 0));
        directions.Add("Down", new Vector2(0, -1));
        directions.Add("Left", new Vector2(-1, 0));


        Vector2 directionVector = m_endPos - m_startPos;
        directionVector = directionVector.normalized;

        //then we check if the direction is negative or positive

        if (directionVector.x < 0)
        {
            m_percentages["Left"] = Mathf.Abs(directionVector.x) * 100;
        }
        else
        {
            m_percentages["Right"] = Mathf.Abs(directionVector.x) * 100;
        }

        if (directionVector.y < 0)
        {
            m_percentages["Down"] = Mathf.Abs(directionVector.y) * 100;
        }
        else
        {
            m_percentages["Up"] = Mathf.Abs(directionVector.y) * 100;
        }

        // calculate the percentage of the swipe in each direction
        // first we get the two highest percentages value and string
    }

    // check the swipe directions and call the corresponding functions
    private void StateManager()
    {
        switch (PossibleDirections)
        {
            case SwipeDirections.UpDown:
                UpDownState();
                break;
            case SwipeDirections.LeftRight:
                LeftRightState();
                break;
            case SwipeDirections.FourDirections:
                FourDirectionsState();
                break;
            case SwipeDirections.EightDirections:
                EightDirectionsState();
                break;
            case SwipeDirections.Percentage:
                PercentageState();
                break;
        }
    }

    private void UpDownState()
    {
        if (m_percentages["Up"] > m_percentages["Down"])
        {
            OnSwipe?.Invoke("Up");
        }
        else
        {
            OnSwipe?.Invoke("Down");
        }
    }

    private void LeftRightState()
    {
        if (m_percentages["Left"] > m_percentages["Right"])
        {
            OnSwipe?.Invoke("Left");
        }
        else
        {
            OnSwipe?.Invoke("Right");
        }
    }

    private void FourDirectionsState()
    {
        string highestPercentageString = "Up";

        foreach (KeyValuePair<string, float> percentage in m_percentages)
        {
            if (percentage.Value > m_percentages[highestPercentageString])
            {
                highestPercentageString = percentage.Key;
            }
        }

        OnSwipe?.Invoke(highestPercentageString);
    }

    private void EightDirectionsState()
    {
        // first we get the two highest percentages value and string
        float firstPerceFloat = 0;
        float secondPerceFloat = 0;
        string firstPerceString = "Up";
        string secondPerceString = "Up";



        foreach (KeyValuePair<string, float> percentage in m_percentages)
        {
            if (percentage.Value > firstPerceFloat)
            {
                secondPerceFloat = firstPerceFloat;
                secondPerceString = firstPerceString;
                firstPerceFloat = percentage.Value;
                firstPerceString = percentage.Key;
            }
            else if (percentage.Value > secondPerceFloat)
            {
                secondPerceFloat = percentage.Value;
                secondPerceString = percentage.Key;
            }
        }

        // then we check if the difference between the two percentages is greater than 0.8
        if ((secondPerceFloat / firstPerceFloat) * 100 >= m_neededSimilarity)
        {
            string result;

            // makes sure the order is correct
            if (firstPerceString == "Up" || firstPerceString == "Down")
            {
                result = firstPerceString + secondPerceString;
            }
            else
            {
                result = secondPerceString + firstPerceString;
            }
            OnSwipe?.Invoke(result);
        }
        else
        {
            FourDirectionsState();
        }

        firstPerceFloat = 0;
        secondPerceFloat = 0;
        firstPerceString = "Up";
        secondPerceString = "Up";
    }

    private void PercentageState()
    {
        OnOnSwipePercentage?.Invoke(m_percentages);
    }
}

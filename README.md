# UnitySwipeManager
**Update: currently working on adjusting the percentages since they are slightly off and on a demo to show off the script**

Fed up with having to make a different swipe manager for your mobile Unity games, here you have a fast and effective one fits all solution!

A simple swipe manager for unity mobile, easy to use. Perfect for Game Jams or more lengthy projects.

Using the observer pattern to make sure the implementation is easy and easily scalable.

# Features 
- Implements different types of swipe directions to make sure that it covers all needs!
  Supports:
    - *Up Down :* Determines if the direction of the swipe is up or down
    - *Left Right :* Determines if the direction of the swipe is left or right
    - *Four Sides :* Determines if the direction of the swipe went in any of the last four directions.
    - *Eight Sides :*  Determines if the direction of the swipe went in any of the possible eight directions (Up, Down, Left, Right, UpRight, UpLeft ...)
    - *Percentages :* Sends you the percentage it went in each direction (up: 40%, right: 60%) if you need more precision

- Changing its behaviour easy by allowing you to change its various variables inside the Unity Editor. 

# How to use it

Firstly create an Empty GameObject, and apply this script to it. This is what the inspector will look like 

![image](https://user-images.githubusercontent.com/107070295/213000276-92b3e88b-fc8c-4c81-99a3-3064fa071b37.png)

You will see a few things:
- Min Distance Percent: this indicates how much percentage of the screen has to be covered for the swipe to activate (this percentage will apply to the smallest distance width or height)

- Possible Directions: When clicked it will display the possible "states" the swipe manager can be in, refer to Features to learn more about each "state".

![image](https://user-images.githubusercontent.com/107070295/213000875-0c7a0b36-2335-466c-ac01-583b0eb670c6.png)

- Needed Similarity: this only takes place in the eight directions state, and determines the sensibility in what is considered a diagonal swipe (Example: DownRight) or a straight swipe (Example: Down). Incrementing the needed similarity will make it harder to get diagonal swipes, therefore decreasing it will make diagonal swipes easier to obtain

Then in the script you want to access the swipe information you will have to "subscribe" to the event channel, this is done via:

```C#
    void OnEnable() {
        SwipeManager.OnSwipe += YourFunction;
    }

    void OnDisable() {
        SwipeManager.OnSwipe -= YourFunction;
    }
```

The function you insert will then be called whenever a swipe is detected. == !!! MAKE SURE YOUR FUNCTION HAS A STRING AS A PARAMETER!!! ==

If you need any help with the observer pattern you can watch this video I recommend (not made by me https://www.youtube.com/watch?v=8fcI8W9NBEo&ab_channel=Comp-3Interactive) or you can send me a mail (alexandroverapons@gmail.com) or message.

A string will be sent with the according directions ( the possible strings will change depending on the state), all the possible strings are = "Up", "UpRight", "Right", "DownRight", "Down", "DownLeft", "Left", "UpLeft".


## If using Percentage State

Instead of "subscribing" to OnSwipe, you have to "subscribe" to OnOnSwipePercentage. 

You will be returned a dictionary with each direction in a string and each percentage in a float (TKey string, TValue float). 


# How does it work 

When you touch the screen and slide your finger if you exceed m_minDistance then the swipe starts being calculated. First, the percentage is calculated, and then depending on the state different calculations will occur to then call the event and send the direction.

Here is an image that represents it.

![image](https://user-images.githubusercontent.com/107070295/213020609-8f0d92dd-0e5d-4db7-a619-64bfa4f52321.png)


# More Info 

If you need anything please feel free to mail me or message me.

I will be doing some changes soon to improve the script and if you have any suggestions I will implement them as well.



using UnityEngine;
using Rewired;
using UnityHelpers;

public class InputToSprayers : MonoBehaviour
{
    public int playerId = 0;
    private Player player;

    public Sprayer leftSprayer, rightSprayer;
    public float fastDecay = 0.95f;
    public float slowDecay = 0.99f;

    private float leftPower, rightPower;
    private bool prevLeft, prevRight;
    private float highestLeft, highestRight;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
    }
    void Update()
    {
        GetInput();

        leftSprayer.power = leftPower;
        rightSprayer.power = rightPower;
    }

    private void GetInput()
    {
        float leftValue = 1 - new Vector2(player.GetAxis("Horizontal1"), player.GetAxis("Vertical1")).magnitude;
        float rightValue = 1 - new Vector2(player.GetAxis("Horizontal2"), player.GetAxis("Vertical2")).magnitude;
        bool leftDown = player.GetButton("Horizontal1") || player.GetButton("Vertical1") || player.GetNegativeButton("Horizontal1") || player.GetNegativeButton("Vertical1");
        bool rightDown = player.GetButton("Horizontal2") || player.GetButton("Vertical2") || player.GetNegativeButton("Horizontal2") || player.GetNegativeButton("Vertical2");

        if (leftDown && !prevLeft)
        {
            leftPower = leftValue;
            highestLeft = leftPower;
        }
        else if (leftDown)
        {
            if (leftValue - highestLeft > float.Epsilon)
            {
                leftPower += (leftValue - highestLeft);
                highestLeft = leftValue;
            }

            leftPower *= slowDecay;
        }
        else
        {
            leftPower *= fastDecay;
            highestLeft = 0;
        }

        if (rightDown && !prevRight)
        {
            rightPower = rightValue;
            highestRight = rightPower;
        }
        else if (rightDown)
        {
            if (rightValue - highestRight > float.Epsilon)
            {
                rightPower += (rightValue - highestRight);
                highestRight = rightValue;
            }

            rightPower *= slowDecay;
        }
        else
        {
            rightPower *= fastDecay;
            highestRight = 0;
        }

        prevLeft = leftDown;
        prevRight = rightDown;
    }
}

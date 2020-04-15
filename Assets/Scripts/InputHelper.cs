using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper
{
    const string mHorizontalPlayer1 = "HorizontalPlayer1";
    const string mHorizontalPlayer2 = "HorizontalPlayer2";

    const string mFirePlayer1 = "FirePlayer1";
    const string mFirePlayer2 = "FirePlayer2";

    const string mJumpPlayer1 = "JumpPlayer1";
    const string mJumpPlayer2 = "JumpPlayer2";

    public static Dictionary<string,string> CalibrateInput(string playerTag)
    {
        Dictionary<string, string> inputDictionary = new Dictionary<string, string>();

        if (playerTag == "Robot")
        {
            inputDictionary["Horizontal"] = mHorizontalPlayer1;
            inputDictionary["Jump"] = mJumpPlayer1;
            inputDictionary["Fire"] = mFirePlayer1;
        }
        else if (playerTag == "Ninja")
        {
            inputDictionary["Horizontal"] = mHorizontalPlayer2;
            inputDictionary["Jump"] = mJumpPlayer2;
            inputDictionary["Fire"] = mFirePlayer2;
        }

        return inputDictionary;
    }
}

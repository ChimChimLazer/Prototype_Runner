using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores functions that will be used in multiple different scripts.
public static class GlobalFunctions
{
    // Converts time from a floating number of seconds to a formatted string that displays minutes, seconds and milliseconds.
    // This text will be fromatted as show in the line below (M = minutes, S = seconds, m = milliseconds
    // M:SS:mmm
    public static string convertTimeToText(float time)
    {
        int milliseconds = Mathf.RoundToInt((time % 1) * 1000);
        if (milliseconds > 999)
        {
            milliseconds = 0;
        }
        int seconds = Mathf.RoundToInt((time - milliseconds / 1000) % 60);
        if (seconds > 59)
        {
            seconds = 0;
        }
        int minutes = Mathf.RoundToInt((time - milliseconds / 1000 - seconds) / 60);

        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-pad-a-number-with-leading-zeros?redirectedfrom=MSDN
        return (minutes + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D3"));
    }
}

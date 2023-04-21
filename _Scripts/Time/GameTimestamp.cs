using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter,
    }
    public Season season;

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,

    }

    public int day;
    public int hour;
    public int minute;

    public GameTimestamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;

    }
    public GameTimestamp(GameTimestamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;

    }
    public void UpdateClock()
    {
        minute++;

        if (minute >= 60)
        {
            minute = 0;
            hour++;

        }
        if (hour >= 24)
        {
            hour = 0;
            day++;

        }

        if (day >= 30)
        {
            day = 1;
            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }

        }

    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToDays(year) + SeasonToDays(season) + day;

        int daysIndex = daysPassed % 7;

        return (DayOfTheWeek)daysIndex;
    }


    public static int HoursToMinutes(int hour)
    {
        return hour * 60;
    }

    public static int DaysToHours(int day)
    {
        return day * 24;
    }
    public static int SeasonToDays(Season season)
    {
        int seasonIndex = (int)season;

        return seasonIndex * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }

    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
     
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;

        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;

        int timeDiff = timestamp2Hours - timestamp1Hours;
     

        return Mathf.Abs(timeDiff);
         


    }
}

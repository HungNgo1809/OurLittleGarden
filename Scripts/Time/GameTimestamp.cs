using JetBrains.Annotations;
//using PacketDotNet.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public int year;
    public const int HoursPerDay = 24;
    public const int MinutesPerHour = 60;
    public const int DaysPerSeason = 30;
    public const int SeasonsPerYear = 4;

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

        return seasonIndex * 29; // sua lai 29 do khi chuyen season no nhay? them 1 ngay nen luc compare bi sai
    }

    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }

    public static int MinutesToYears(int totalMinutes)
    {
        return totalMinutes / (SeasonsPerYear * DaysPerSeason * HoursPerDay * MinutesPerHour);
    }

    public static Season MinutesToSeasons(int totalMinutes)
    {
        int seasonIndex = (totalMinutes / (DaysPerSeason * HoursPerDay * MinutesPerHour)) % SeasonsPerYear;

        switch (seasonIndex)
        {
            case 0:
                return Season.Spring;
            case 1:
                return Season.Summer;
            case 2:
                return Season.Fall;
            case 3:
                return Season.Winter;
            default:
                return Season.Spring; // Default to Spring if the index is out of range
        }
    }

    public static int MinutesToDays(int totalMinutes)
    {
        return (totalMinutes / (HoursPerDay * MinutesPerHour)) % DaysPerSeason;
    }

    public static int MinutesToHours(int totalMinutes)
    {
        return (totalMinutes / MinutesPerHour) % HoursPerDay;
    }

    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {

        int timestamp1Hours = HoursToMinutes(DaysToHours(YearsToDays(timestamp1.year))) + HoursToMinutes(DaysToHours(SeasonToDays(timestamp1.season))) + HoursToMinutes(DaysToHours(timestamp1.day)) + HoursToMinutes(timestamp1.hour) + timestamp1.minute;

        int timestamp2Hours = HoursToMinutes(DaysToHours(YearsToDays(timestamp2.year))) + HoursToMinutes(DaysToHours(SeasonToDays(timestamp2.season)))+ HoursToMinutes(DaysToHours(timestamp2.day)) + HoursToMinutes(timestamp2.hour) + timestamp2.minute;

        int timeDiff = timestamp2Hours - timestamp1Hours;


        return Mathf.Abs( timeDiff);
         


    }
    public static int CompareTimestampsWithoutAbs(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {

        int timestamp1Hours = HoursToMinutes(DaysToHours(YearsToDays(timestamp1.year))) + HoursToMinutes(DaysToHours(SeasonToDays(timestamp1.season))) + HoursToMinutes(DaysToHours(timestamp1.day)) + HoursToMinutes(timestamp1.hour) + timestamp1.minute;

        int timestamp2Hours = HoursToMinutes(DaysToHours(YearsToDays(timestamp2.year))) + HoursToMinutes(DaysToHours(SeasonToDays(timestamp2.season))) + HoursToMinutes(DaysToHours(timestamp2.day)) + HoursToMinutes(timestamp2.hour) + timestamp2.minute;

        int timeDiff = timestamp2Hours - timestamp1Hours;

     
        return timeDiff;



    }




    public static GameTimestamp CalculateApproximateTimestamp(GameTimestamp timestamp2, int timeDiff) // nguoc lai cua ban so sanh dungk nih @@
    {
        int TotalMinutes =  HoursToMinutes(DaysToHours(YearsToDays(timestamp2.year))) + HoursToMinutes(DaysToHours(SeasonToDays(timestamp2.season))) + HoursToMinutes(DaysToHours(timestamp2.day)) + HoursToMinutes(timestamp2.hour) + timestamp2.minute;

        int totalMinutes = TotalMinutes - timeDiff;

      
        int years = MinutesToYears(totalMinutes);
        totalMinutes -= HoursToMinutes(DaysToHours(YearsToDays(years)));
        Season seasons = MinutesToSeasons(totalMinutes);
    
        totalMinutes -=  HoursToMinutes(DaysToHours(SeasonToDays(seasons)));

        int days = MinutesToDays(totalMinutes);
        totalMinutes -= HoursToMinutes(DaysToHours(days));
        int hours = MinutesToHours(totalMinutes);
        totalMinutes -= HoursToMinutes(hours);
        int minutes = totalMinutes;

        return new GameTimestamp(years, seasons, days, hours, minutes);
    }
}

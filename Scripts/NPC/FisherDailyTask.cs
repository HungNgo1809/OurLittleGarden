using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class FisherDailyTask : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Task> taskList = new List<Task>();
    public List<TaskTime> taskTime = new List<TaskTime>();
    public Shoper NPCHouse;
    public List<string> taskNames = new List<string>();
    public List<string> taskNamesSpecialDay = new List<string>();
    public int SpecialDay;
    public Task currentTask;
    private FisherBehavior behavior;
    public bool isTasksCreated = false;
    private bool isTask0Doone = false;
    private bool isTask1Doone = false;
    private bool isTask2Doone = false;
    private bool isTask3Doone = false;
    private bool isTask4Doone = false;
    private bool isTask5Doone = false;
    void Start()
    {
        behavior = GetComponent<FisherBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        DailyRestTask();
    }

    public void DailyRestTask()
    {
        if (TimeManager.Instance.timestamp.hour == 0)
        {
            if (taskList.Count > 0)
            {
                taskList.Clear();
                isTasksCreated = false;

            }

        }
        if (TimeManager.Instance.timestamp.hour >= 7 && TimeManager.Instance.timestamp.minute == 30)
        {
            if (!isTasksCreated)
            {
      
                CreateANewDailyTask();
                isTasksCreated = true;

            }
        }

        if (taskList.Count > 0)
        {

            for (int i = 0; i < taskList.Count; i++)
            {

                if (TimeManager.Instance.timestamp.hour == taskList[i].time_.hour && TimeManager.Instance.timestamp.minute == taskList[i].time_.minute && !IsTaskDone(i))
                {

                    if (taskList[i].TaskGoal == "DoNothing")
                    {

                        behavior.SwitchStateAction(FisherBehavior.NPCAction.DoNothing);
                    }
                    else if (taskList[i].TaskGoal == "Fishing")
                    {

                        behavior.SwitchStateAction(FisherBehavior.NPCAction.Fishing);
                    }
                    else if (taskList[i].TaskGoal == "MeetSeller")
                    {

                        behavior.SwitchStateAction(FisherBehavior.NPCAction.MeetSeller);
                    }
                    currentTask = taskList[i];
                    SetTaskDone(i);
                }
            }

        }

    }


    private bool IsTaskDone(int index)
    {
        switch (index)
        {
            case 0:
                return isTask0Doone;
            case 1:
                return isTask1Doone;
            case 2:
                return isTask2Doone;
            case 3:
                return isTask3Doone;
            case 4:
                return isTask4Doone;
            case 5:
                return isTask5Doone;
            default:
                return false;
        }
    }

    private void SetTaskDone(int index)
    {
        switch (index)
        {
            case 0:
                isTask0Doone = true;
                break;
            case 1:
                isTask1Doone = true;
                break;
            case 2:
                isTask2Doone = true;
                break;
            case 3:
                isTask3Doone = true;
                break;
            case 4:
                isTask4Doone = true;
                break;
            case 5:
                isTask4Doone = true;
                break;
        }
    }




    public void CreateANewDailyTask()
    {
        int taskCount = 5; // Default task count

        // check task




        for (int i = 0; i <= taskCount; i++)
        {

            Task newTask = new Task("Task " + (i + 1), new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, 18, 18));
            taskList.Add(newTask);
            if (TimeManager.Instance.timestamp.day != SpecialDay)
            {
                SetTaskGoal(newTask);
            }
            else
            {
                SetTaskGoalSpecialDay(newTask);
                Debug.Log("aasasdsd");
            }


        }
        isTask0Doone = false;
        isTask1Doone = false;
        isTask2Doone = false;
        isTask3Doone = false;
        isTask4Doone = false;
        isTask5Doone = false;
    }
    private void SetTaskGoal(Task task) // set goal
    {
        // Debug.Log("SetTaskGoal");
        if (task.Name == "Task 1")
        {
            task.TaskGoal = taskNames[0];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[0].hour, taskTime[0].minute);
            //    Debug.Log("Task 1 " + taskTime[0].hour + " " + taskTime[0].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 2")
        {
            task.TaskGoal = taskNames[1];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[1].hour, taskTime[1].minute);
            // Debug.Log("Task 2 " + taskTime[1].hour + " " + taskTime[1].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 3")
        {
            task.TaskGoal = taskNames[2];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[2].hour, taskTime[2].minute);
            //     Debug.Log("Task 3 " + taskTime[2].hour + " " + taskTime[2].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 4")
        {
            task.TaskGoal = taskNames[3];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[3].hour, taskTime[3].minute);
            //  Debug.Log("Task 4 " + taskTime[3].hour + " " + taskTime[3].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 5")
        {
            task.TaskGoal = taskNames[4];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[4].hour, taskTime[4].minute);
            //   Debug.Log("Task 5 " + taskTime[4].hour + " " + taskTime[4].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 6")
        {
            task.TaskGoal = taskNames[5];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[5].hour, taskTime[5].minute);
            //   Debug.Log("Task 6 " + taskTime[5].hour + " " + taskTime[5].minute + " " + gameObject.name);
        }

    }
    private void SetTaskGoalSpecialDay(Task task) // set goal
    {
        // Debug.Log("SetTaskGoalSpecialDay");
    
        if (task.Name == "Task 1")
        {
            task.TaskGoal = taskNamesSpecialDay[0];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[0].hour, taskTime[0].minute);
            //Debug.Log("Task 1 " + taskTime[0].hour + " " + taskTime[0].minute + " " + gameObject.name); 
        }
        else if (task.Name == "Task 2")
        {
            task.TaskGoal = taskNamesSpecialDay[1];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[1].hour, taskTime[1].minute);
            // Debug.Log("Task 2 " + taskTime[1].hour + " " + taskTime[1].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 3")
        {
            task.TaskGoal = taskNamesSpecialDay[2];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[2].hour, taskTime[2].minute);
            //  Debug.Log("Task 3 " + taskTime[2].hour + " " + taskTime[2].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 4")
        {
            task.TaskGoal = taskNamesSpecialDay[3];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[3].hour, taskTime[3].minute);
            // Debug.Log("Task 4 " + taskTime[3].hour + " " + taskTime[3].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 5")
        {
            task.TaskGoal = taskNamesSpecialDay[4];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[4].hour, taskTime[4].minute);
            //  Debug.Log("Task 5 " + taskTime[4].hour + " " + taskTime[4].minute + " " + gameObject.name);

        }
        else if (task.Name == "Task 6")
        {
            task.TaskGoal = taskNamesSpecialDay[5];
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[5].hour, taskTime[5].minute);
            //  Debug.Log("Task 6 " + taskTime[5].hour + " " + taskTime[5].minute + " " + gameObject.name);

        }
    }

    public void DoCurrentTask()
    {
        if (currentTask != null)
        {
            if (currentTask.TaskGoal == "DoNothing")
            {

                behavior.SwitchStateAction(FisherBehavior.NPCAction.DoNothing);
            }
            else if (currentTask.TaskGoal == "Fishing")
            {

                behavior.SwitchStateAction(FisherBehavior.NPCAction.Fishing);
            }
            else if (currentTask.TaskGoal == "MeetSeller")
            {

                behavior.SwitchStateAction(FisherBehavior.NPCAction.MeetSeller);
            }
        }
    }


    [System.Serializable]
    public class Task
    {
        public string Name { get; set; }
        public GameTimestamp time_ { get; set; }
        public string TaskGoal { get; set; }
        public Task(string name, GameTimestamp timeDoTask)
        {
            Name = name;
            time_ = timeDoTask;

        }
    }
    [System.Serializable]
    public class TaskTime
    {
        public int hour;
        public int minute;

        public TaskTime(int hour, int minute)
        {
            this.hour = hour;
            this.minute = minute;
        }
    }
}

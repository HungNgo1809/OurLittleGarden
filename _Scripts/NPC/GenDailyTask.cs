using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static NPCBehavior;

public class GenDailyTask : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Task> taskList = new List<Task>();
    public List<TaskTime> taskTime = new List<TaskTime>();
    public Shoper NPCHouse;
    public List<TaskName> taskNames = new List<TaskName>();
    public List<TaskName> taskNamesSpecialDay = new List<TaskName>();
    public int SpecialDay;
    public Task currentTask;
    private GenNPCBehavior behavior;
    private bool isTasksCreated = false;
    private Task nextTask = null;

    void Start()
    {
        behavior = GetComponent<GenNPCBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        DailyRestTask();
    }

    public void DailyRestTask()
    {
        if (TimeManager.Instance.timestamp.hour >= 0 && TimeManager.Instance.timestamp.hour <= 5)
        {
            if (taskList.Count > 0)
            {
                taskList.Clear();
                isTasksCreated = false;
                currentTask = null;

               
            }

        }


        if (TimeManager.Instance.timestamp.hour >= 6)
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

                if (TimeManager.Instance.timestamp.hour == taskList[i].time_.hour && TimeManager.Instance.timestamp.minute == taskList[i].time_.minute && !IsTaskDone(i)) // co the them 1 Hanh dong khac cho ngay dac biet - chua nghi nen lam gi
                {

                    if (taskList[i].TaskGoal == "DoNothing")
                    {
                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
                    }
                    else if (taskList[i].TaskGoal == "MeetEmma")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetEmma);
                    }
                    else if (taskList[i].TaskGoal == "GoingSomeWhere")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.GoingSomeWhere);
                    }
                    else if (taskList[i].TaskGoal == "Dancing")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Dancing);
                    }
                    else if (taskList[i].TaskGoal == "Thinking")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Thinking);
                    }
                    else if (taskList[i].TaskGoal == "MeetSeller")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetSeller);
                    }
                    else if (taskList[i].TaskGoal == "MeetSophia")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetSophia);
                    }
                    else if (taskList[i].TaskGoal == "MeetEmma")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetEmma);
                    }
                    else if (taskList[i].TaskGoal == "GoingHome")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.GoingHome);
                    }
                    else if (taskList[i].TaskGoal == "Fishing")
                    {

                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Fishing);
                    }
                    else if (taskList[i].TaskGoal == "MeetNPC")
                    {
                 
                        behavior.NPCGonnaMeet = taskList[i].NPC;
                        behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetNPC);
                    }
                    currentTask = taskList[i];
                    SetTaskDone(i, true);
                }
            }

        }

    }


    private bool IsTaskDone(int index)
    {
        switch (index)
        {
            case 0:
                return taskList[0].IsDone;
            case 1:
                return taskList[1].IsDone;
            case 2:
                return taskList[2].IsDone;
            case 3:
                return taskList[3].IsDone;
            case 4:
                return taskList[4].IsDone;
            case 5:
                return taskList[5].IsDone;
            default:
                return false;
        }
    }
    private void SetTaskDone(int index, bool value)
    {
        if (index >= 0 && index < taskList.Count)
        {
            taskList[index].IsDone = value;
        }
    }



    public void CreateANewDailyTask()
    {
        int taskCount = 5; // Default task count

        // check task


        for (int i = 0; i <= taskCount; i++)
        {
            if (taskCount == 5)
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
                }
                SetTaskDone(i, false);
            }
            else
            {
                Task newTask = new Task("Task " + (i + 3), new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, 18, 18));
                taskList.Add(newTask);
                if (TimeManager.Instance.timestamp.day != SpecialDay)
                {
                    SetTaskGoal(newTask);
                }
                else
                {
                    SetTaskGoalSpecialDay(newTask);
                }
                SetTaskDone(i + 3, false);
            }
        }
    }
    private void SetTaskGoal(Task task) // set goal
    {
        // Debug.Log("SetTaskGoal");
        if (task.Name == "Task 1")
        {
            task.TaskGoal = taskNames[0].taskName;
         
            if (taskNames[0].taskName == "MeetNPC")
            {
                task.NPC = taskNames[0].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[0].hour, taskTime[0].minute);
            //    Debug.Log("Task 1 " + taskTime[0].hour + " " + taskTime[0].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 2")
        {
            task.TaskGoal = taskNames[1].taskName;
        
            if (taskNames[1].taskName == "MeetNPC")
            {
                task.NPC = taskNames[1].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[1].hour, taskTime[1].minute);
            // Debug.Log("Task 2 " + taskTime[1].hour + " " + taskTime[1].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 3")
        {
            task.TaskGoal = taskNames[2].taskName;
          
            if (taskNames[2].taskName == "MeetNPC")
            {
                task.NPC = taskNames[2].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[2].hour, taskTime[2].minute);
            //     Debug.Log("Task 3 " + taskTime[2].hour + " " + taskTime[2].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 4")
        {
            task.TaskGoal = taskNames[3].taskName;
        
            if (taskNames[3].taskName == "MeetNPC")
            {
                task.NPC = taskNames[3].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[3].hour, taskTime[3].minute);
            //  Debug.Log("Task 4 " + taskTime[3].hour + " " + taskTime[3].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 5")
        {
            task.TaskGoal = taskNames[4].taskName;
        
            if (taskNames[4].taskName == "MeetNPC")
            {
                task.NPC = taskNames[4].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[4].hour, taskTime[4].minute);
            //   Debug.Log("Task 5 " + taskTime[4].hour + " " + taskTime[4].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 6")
        {
            task.TaskGoal = taskNames[5].taskName;
            if (taskNames[5].taskName == "MeetNPC")
            {
                task.NPC = taskNames[5].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[5].hour, taskTime[5].minute);
            //   Debug.Log("Task 6 " + taskTime[5].hour + " " + taskTime[5].minute + " " + gameObject.name);
        }

    }
    private void SetTaskGoalSpecialDay(Task task) // set goal
    {
        // Debug.Log("SetTaskGoalSpecialDay");

        if (task.Name == "Task 1")
        {
            task.TaskGoal = taskNamesSpecialDay[0].taskName;
            
            if (taskNamesSpecialDay[0].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[0].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[0].hour, taskTime[0].minute);
            //Debug.Log("Task 1 " + taskTime[0].hour + " " + taskTime[0].minute + " " + gameObject.name); 
        }
        else if (task.Name == "Task 2")
        {
            task.TaskGoal = taskNamesSpecialDay[1].taskName;
            if (taskNamesSpecialDay[1].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[1].NPC;
                Debug.Log(task.NPC.name);
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[1].hour, taskTime[1].minute);
            // Debug.Log("Task 2 " + taskTime[1].hour + " " + taskTime[1].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 3")
        {
            task.TaskGoal = taskNamesSpecialDay[2].taskName;
            if (taskNamesSpecialDay[2].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[2].NPC;
                Debug.Log(task.NPC.name);
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[2].hour, taskTime[2].minute);
            //  Debug.Log("Task 3 " + taskTime[2].hour + " " + taskTime[2].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 4")
        {
            task.TaskGoal = taskNamesSpecialDay[3].taskName;
            if (taskNamesSpecialDay[3].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[3].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[3].hour, taskTime[3].minute);
            // Debug.Log("Task 4 " + taskTime[3].hour + " " + taskTime[3].minute + " " + gameObject.name);
        }
        else if (task.Name == "Task 5")
        {
            task.TaskGoal = taskNamesSpecialDay[4].taskName;
            if (taskNamesSpecialDay[4].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[4].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[4].hour, taskTime[4].minute);
            //  Debug.Log("Task 5 " + taskTime[4].hour + " " + taskTime[4].minute + " " + gameObject.name);

        }
        else if (task.Name == "Task 6")
        {
            task.TaskGoal = taskNamesSpecialDay[5].taskName;
            if (taskNamesSpecialDay[5].taskName == "MeetNPC")
            {
                task.NPC = taskNamesSpecialDay[5].NPC;
            }
            task.time_ = new GameTimestamp(TimeManager.Instance.timestamp.year, TimeManager.Instance.timestamp.season, TimeManager.Instance.timestamp.day, taskTime[5].hour, taskTime[5].minute);
            //  Debug.Log("Task 6 " + taskTime[5].hour + " " + taskTime[5].minute + " " + gameObject.name);

        }
    }

    public void GetNextTask(int numberOfTime)
    {

        if (taskList.Count > 0)
        {
            foreach (Task task in taskList)
            {


                if (!task.IsDone)
                {

                    int taskTime = task.time_.hour * 60 + task.time_.minute;
                    int currentTime = TimeManager.Instance.timestamp.hour * 60 + TimeManager.Instance.timestamp.minute;

                    if (Mathf.Abs(taskTime - currentTime) < numberOfTime)
                    {
                        currentTask = task;
                        task.IsDone = true;
                        return;
                    }

                }
            }

        }


    }

    public void DoCurrentTask()
    {
        if (currentTask != null)
        {

            if (currentTask.TaskGoal == "DoNothing")
            {
                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.DoNothing);
            }
            else if (currentTask.TaskGoal == "MeetEmma")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetEmma);
            }
            else if (currentTask.TaskGoal == "GoingSomeWhere")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.GoingSomeWhere);
            }
            else if (currentTask.TaskGoal == "Dancing")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Dancing);
            }
            else if (currentTask.TaskGoal == "Thinking")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Thinking);
            }
            else if (currentTask.TaskGoal == "MeetSeller")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetSeller);
            }
            else if (currentTask.TaskGoal == "MeetSophia")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetSophia);
            }
            else if (currentTask.TaskGoal == "Fishing")
            {

                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.Fishing);
            }
            else if (currentTask.TaskGoal == "MeetNPC")
            {
                behavior.NPCGonnaMeet = currentTask.NPC;
             
                behavior.SwitchStateAction(GenNPCBehavior.NPCAction.MeetNPC);
            }
        }
    }

    [System.Serializable]
    public class Task
    {
        public string Name { get; set; }
        public GameTimestamp time_ { get; set; }
        public string TaskGoal { get; set; }
        public bool IsDone { get; set; }  // New property to track completion status

        public GameObject NPC { get; set; }

        public Task(string name, GameTimestamp timeDoTask)
        {
            Name = name;
            time_ = timeDoTask;
            IsDone = false;  // Initialize as not done
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
    [System.Serializable]
    public class TaskName
    {
        public string taskName;
        public GameObject NPC;

    }

}

using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FocusriteCracklingFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Hide();


            bool SkipTask = false;
            bool RetryFix = true;

            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.ToLower().Contains("skiptask"))
                {
                    SkipTask = true;
                }
                if (arg.ToLower().Contains("dontretryfix"))
                {
                    RetryFix = false;
                }
            }

            if (!SkipTask)
            {
                SetUpTask();
            }

            FixAudio(RetryFix);

            Environment.Exit(0);
        }

        public static void SetUpTask()
        {
            try
            {
                TaskService tService = new TaskService();
                TaskDefinition tDefinition = tService.NewTask();

                string TaskName = "FocusriteCracklingFix";
                string TaskDescription = "Sets process priority to high and process affinity to a single core (audiodg.exe).";
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string exeLocation = Process.GetCurrentProcess().MainModule.FileName;

                tDefinition.Principal.Id = userName;
                tDefinition.Principal.DisplayName = userName;
                tDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
                tDefinition.Principal.RunLevel = TaskRunLevel.Highest;
                tDefinition.RegistrationInfo.Description = TaskDescription;

                tDefinition.Triggers.Add(new LogonTrigger());
                tDefinition.Actions.Add(new ExecAction(exeLocation));

                tDefinition.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                tDefinition.Settings.DisallowStartIfOnBatteries = false;
                tDefinition.Settings.StopIfGoingOnBatteries = false;
                tDefinition.Settings.StartWhenAvailable = true;
                tDefinition.Settings.RunOnlyIfNetworkAvailable = false;
                tDefinition.Settings.IdleSettings.StopOnIdleEnd = false;
                tDefinition.Settings.IdleSettings.RestartOnIdle = false;

                tDefinition.Settings.AllowDemandStart = true;
                tDefinition.Settings.Enabled = true;
                tDefinition.Settings.Hidden = false;
                tDefinition.Settings.RunOnlyIfIdle = false;
                tDefinition.Settings.WakeToRun = false;
                tDefinition.Settings.Priority = ProcessPriorityClass.High;


                tService.RootFolder.RegisterTaskDefinition(TaskName, tDefinition, TaskCreation.CreateOrUpdate, userName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to set up Task.\n\n" + e.ToString(), "FocusriteCracklingFix");
            }
        }

        public static void FixAudio(bool RunInLoopIfNoProcessesFound)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("audiodg");
                if (processes.Length == 0)
                {
                    System.Threading.Tasks.Task.Run(() => System.Threading.Tasks.Task.Delay(15 * 1000)).Wait();
                    if (RunInLoopIfNoProcessesFound)
                    {
                        FixAudio(RunInLoopIfNoProcessesFound);
                    }
                }
                foreach (Process p in processes)
                {
                    if (p.PriorityClass != ProcessPriorityClass.High)
                    {
                        p.PriorityClass = ProcessPriorityClass.High;
                    }

                    p.ProcessorAffinity = (IntPtr)4; // works lmao. Uses just the second (third) Core / Thread
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to change audiodg.exe.\n\n" + e.ToString(), "FocusriteCracklingFix");
            }
        }
    }
}

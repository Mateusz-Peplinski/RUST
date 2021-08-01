using RUST;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Windows.Forms;


/// <summary>
/// Author: Mateusz Peplinski
/// Date v .10 Published: 01 / 08 / 2021
/// License: MIT 
/// © 2021 Mateusz Peplinski
/// </summary>



namespace perfCounter
{
    class Program
    {


        static void Main(string[] args)
        {

            displayName();


            while (true)
            {
                menu();
            }
        }

        public static void displayName()
        {
            Console.WriteLine(" _____  _    _  _____ _______ ");
            Console.WriteLine("|  __ \\| |  | |/ ____|__   __|");
            Console.WriteLine("| |__) | |  | | (___    | |   ");
            Console.WriteLine("|  _  /| |  | |\\___ \\   | |   ");
            Console.WriteLine("| | \\ \\| |__| |____) |  | |   ");
            Console.WriteLine("|_|  \\_\\\\____/|_____/   |_|   ");
            Console.WriteLine("A general-purpose computer tool (RUST v1)");
            Console.WriteLine("By – Mateusz Peplinski");


        }
        
        //This function will disply the main menu and will be the entry point for many functions depending on the user input 
        public static void menu()
        {
            pageBreak();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(String.Format("{0,20}", "- Menu Options -\n"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(String.Format("[1] - {0,20}", "CPU Stats"));
            Console.WriteLine(String.Format("[2] - {0,20}", "RAM Stats"));
            Console.WriteLine(String.Format("[3] - {0,20}", "Process Options"));
            Console.WriteLine(String.Format("[4] - {0,20}", "Network Options"));
            Console.WriteLine(String.Format("[5] - {0,20}", "About"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(String.Format("[6] - {0,20}", "Quit"));
            Console.ForegroundColor = ConsoleColor.White;
            pageBreak();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.White;
            String userChoice = Console.ReadLine();
            int selectedOption_INT;

            if (int.TryParse(userChoice, out selectedOption_INT))
            {
                switch (selectedOption_INT)
                {
                    //[1] This case will deal with CPU Stats
                    case 1:

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(String.Format("{0,20}", "- CPU STATS -"));
                        Console.WriteLine(String.Format("{0,25}", "- Press S to show CPU stats-"));
                        Console.WriteLine(String.Format("{0,25}", "- Press B to go back -"));
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(String.Format("{0,20}", "Note: Sometimes may require a random key \nto be pressed to initiate Thread\n"));
                        Console.ForegroundColor = ConsoleColor.White;

                        //if false the while loop will break and thread will abort.
                        bool CPUflag = true;

                        //Create thread that show current CPU % 
                        //this is a thread so the waiting for B press does not break the cpu % bars from being displayed
                        Thread CPUMonitorThread = new Thread(CPUMonitor_Thread);

                        //THREAD START
                        CPUMonitorThread.Start();

                        while (CPUflag)
                        {
                            //If B is pressed the loop breaks and thread aborts
                            ConsoleKey userKey = Console.ReadKey(true).Key;
                            if (userKey == ConsoleKey.B)
                            {
                                CPUflag = false;
                            }
                            //If S is pressed the thread is aborted + loop breaks and CPU Infomation is showed
                            if (userKey == ConsoleKey.S)
                            {
                                CPUMonitorThread.Abort();
                                CPUflag = false;

                                //Call Function
                                showCPUStats();
                            }
                            //If the user presses anything else message is displayed 
                            else if (userKey != ConsoleKey.B)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Press B to go back !");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        //Thread Abort and case breaks
                        CPUMonitorThread.Abort();
                        break;
                    //[2] This case will show RAM Stats
                    case 2:

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(String.Format("{0,20}", "- RAM STATS -"));
                        Console.ForegroundColor = ConsoleColor.White;

                        //call function
                        showRAMStats();
                        break;

                    //[3] This case will deal with Processes
                    case 3:

                        //if false loop will break
                        bool procOptionsBool = true;
                        while (procOptionsBool)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            pageBreak();
                            Console.WriteLine(String.Format("{0,25}", "- Processes Options -\n"));
                            Console.WriteLine(String.Format("[1] - {0,20}", "Show Processes"));
                            Console.WriteLine(String.Format("[2] - {0,20}", "Dump A Process"));
                            Console.WriteLine(String.Format("[3] - {0,20}", "Kill A Process"));
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(String.Format("[4] - {0,20}", "Go back..."));
                            Console.ForegroundColor = ConsoleColor.Green;
                            pageBreak();
                            Console.ForegroundColor = ConsoleColor.White;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(">");
                            Console.ForegroundColor = ConsoleColor.White;
                            String ProcessesUserChoice = Console.ReadLine();

                            int ProcessesUserChoice_INT;
                            
                            //This makes sure that everything is an int and program does not crash 
                            if (int.TryParse(ProcessesUserChoice, out ProcessesUserChoice_INT))
                            {
                                switch (ProcessesUserChoice_INT)
                                {
                                    //[1] Shows Running Processes
                                    case 1:
                                        //calls function
                                        showProcesees();
                                        break;

                                    //[2] gives the user an option to dump a process from memory
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine("Note: Trying to dump SYSTEM level process\n(eg: system, registry, session manager)\nmay cause Access Denied Error");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        
                                        //calls function to get process ID
                                        int procID = getProcID();

                                        //if procID is 1 then case 2 exits
                                        if (procID == 1)
                                        {
                                            break;
                                        }
                                        
                                        //goes through all running processes...
                                        Process[] processlist = Process.GetProcesses();
                                        foreach (Process theprocess in processlist)
                                        {
                                            //if the process matches then it is diplayed
                                            if (procID == theprocess.Id)
                                            {
                                                Console.WriteLine(String.Format("Process {0} is: {1}", theprocess.Id, theprocess.ProcessName));
                                            }
                                            
                                        }

                                        //confirms this is the process the user would like to dump
                                        Console.WriteLine("Is this the Process you would like to dump? ");
                                        Console.WriteLine(String.Format("[1] - {0,20}", "YES"));
                                        Console.WriteLine(String.Format("[2] - {0,20}", "NO"));
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(">");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        String userProcConfirm = Console.ReadLine();
                                        int userProcConfirmINT;

                                        //This makes sure that everything is an int and program does not crash 
                                        if (int.TryParse(userProcConfirm, out userProcConfirmINT))
                                        {
                                            switch (userProcConfirmINT)
                                            {
                                                //[1] Deals with dumping the user selected process
                                                case 1:
                                                    //gets and displays process information needed for the dump file 
                                                    Process process = Process.GetProcessById(procID);
                                                    Console.WriteLine($"Dumping {process.ProcessName} process");
                                                    Console.WriteLine($"{process.ProcessName} process handler {process.Handle}");
                                                    Console.WriteLine($"{process.ProcessName} process ID {process.Id}");
                                                    //Calls function to Dump Process
                                                    createDump(process.Handle, (uint)process.Id, process.ProcessName);
                                                    break;
                                                //[2] Case 2 breaks
                                                case 2:
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Going Back...");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    break;
                                                //defult also causes a break... just in case 
                                                default:
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Going Back...");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    break;
                                            }
                                        
                                        }
                                        else
                                        {
                                         
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Going Back...");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            break;
                                        }

                                        break;
                                    //[3] Case 3 deals with killing a process
                                    case 3:

                                        //calls function to get ID
                                        int procIDforKill = getProcID();
                                       
                                        //if ID is 1 break out of case 3
                                        if (procIDforKill == 1)
                                        {
                                            break;
                                        }
                                        //else begin process killing
                                        else
                                        {
                                            //fetch process
                                            Process processToKill = Process.GetProcessById(procIDforKill);
                                            //call kill() method to kill the process
                                            processToKill.Kill();
                                            //Displays Success message
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("SUCCESS");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                        break;
                                    //[4] sets the loop flag to false breaking the loop 
                                    case 4:
                                        Console.WriteLine("Going Back...");
                                        procOptionsBool = false;
                                        break;

                                    default:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid Option !");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Enter A Valid Number !");
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                        }
                        break;
                    //[4] This case deals with Network Options
                    case 4:
                        //if false network loop will break 
                        bool netflag = true;
                        while (netflag)
                        {
                            //call function if returns false the loop wil break
                            netflag = networkOptions();
                        }

                        break;

                    //[5] Shows and about program message box 
                    case 5:
                        var aboutButton = MessageBox.Show("A general-purpose computer tool \nThe name \"RUST\" comes from the character\nRust Cohle from True Detective\n\n        Author: Mateusz Peplinski\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    //[6] For exiting the program 
                    case 6:

                        //play system sound
                        SystemSounds.Asterisk.Play();

                        var buttonBreakSwitch = MessageBox.Show("Are you sure you would like to quit?", "Close Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        //conditions based on message box buttons 
                        if (buttonBreakSwitch == DialogResult.No)
                        {
                            break;
                        }
                        else if (buttonBreakSwitch == DialogResult.Yes)
                        {
                            //EXIT PROGRAM
                            Environment.Exit(0);
                        }

                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Menu Option !");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter A Valid Number !");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }// End of MENU function


        //This function is responsible for gettigng a valid process ID (if return is 1 then the program will go back one step{see previous Function calls})
        public static int getProcID()
        {
            //if true loop breaks
            bool converted = false;

            //defult ID set to 0
            int userProcIDINT = 0;

            while (converted == false)
            {
                Console.WriteLine("Press [1] to go back");
                Console.WriteLine("Enter Process ID: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(">");
                Console.ForegroundColor = ConsoleColor.White;

                String userProcIDSTRING = Console.ReadLine();

                try
                {
                    //if the userProcID can be converted into and int then the loop breaks and the function return the ID 
                    int userProcID = Convert.ToInt32(userProcIDSTRING);

                    converted = true;
                    if (converted == true)
                    {
                        userProcIDINT = Convert.ToInt32(userProcIDSTRING);
                    }
                }
                //catches the error so the user must enter and int to not crash the program 
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid input");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
            return userProcIDINT;
        }
        //This is the thread that will display the CPU % and bars while it runs
        public static void CPUMonitor_Thread()
        {

            PerformanceCounter perfCpuCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

            //Counters to not display warning window more then 1 time 
            int warningMessageCount0 = 0;
            int warningMessageCount1 = 0;

            while (true)
            {
                //gets cpu load value
                int cpuLoad = (int)perfCpuCounter.NextValue();

                //displays value
                Console.WriteLine("CPU Load:    {0}%             ", cpuLoad);

                //Sets console colours for bars based on how high the CPU load is
                if (cpuLoad <= 30)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else if (cpuLoad > 30 && cpuLoad < 70)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                //call the function to draw the bar
                Console.WriteLine(drawCPUBar(cpuLoad));

                //sets colours back to defult
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("_______________________________");

                //warning for high CPU usage
                if (cpuLoad > 80)
                {
                    if (cpuLoad == 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("WARNING WARNING WARNING CPU at 100 precent");
                        Console.ForegroundColor = ConsoleColor.White;

                        //shows message box only once if load is 100%
                        if (warningMessageCount0 > 1)
                        {
                            SystemSounds.Hand.Play();
                            MessageBox.Show("High CPU LOAD", "WARNING");
                            warningMessageCount0++;
                        }

                    }
                    //shows message box only once if load is 80 > 100%
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("WARNING Your CPU is under HIGH load");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (warningMessageCount1 > 3)
                        {
                            SystemSounds.Hand.Play();
                            MessageBox.Show("High CPU LOAD", "WARNING");
                            warningMessageCount1++;
                        }
                    }

                }

                //get new value and draw new bar every 1 sec
                Thread.Sleep(1000);
            }//End of Loop
        }
        
        //This function draws the bar for cpuLoad
        public static string drawCPUBar(int cpuLoad)
        {
            //the higher the load the more "|" is printed making the bar bigger
            int barSize = cpuLoad / 5;
            String bar = "";
            if (barSize < 5)
            {
                bar += "|";
            }
            for (int i = 0; i < barSize; i++)
            {

                bar += "|";
            }

            return bar;

        }
        
        //This function displays running processes
        public static void showProcesees()
        {
            //goes through all processes in the list
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                //prints out the PID and process name 
                Console.WriteLine(String.Format("ID: {0,-10} {1}", theprocess.Id, theprocess.ProcessName));
            }
        }

        //This function prints the network menu options  
        public static void printNetworkOptions()
        {
            //call function
            pageBreak();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(String.Format("{0,25}", "- Network Options -\n"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(String.Format("[1] - {0,20}", "Ping a Target"));
            Console.WriteLine(String.Format("[2] - {0,20}", "Port Scanner"));
            Console.WriteLine(String.Format("[3] - {0,20}", "Subnet Scanner"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(String.Format("[4] - {0,20}", "Go Back... "));
            Console.ForegroundColor = ConsoleColor.White;
            //call function
            pageBreak();
        }

        //this function deals with the network options 
        public static bool networkOptions()
        {
            //if flag is false loop breaks
            bool netflag = true;
            while (netflag)
            {
                printNetworkOptions();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(">");
                Console.ForegroundColor = ConsoleColor.White;
                String userNetWorkOpt = Console.ReadLine();
                int userNetWorkOpt_INTVerification;

                //This makes sure that everything is an int and program does not crash 
                if (int.TryParse(userNetWorkOpt, out userNetWorkOpt_INTVerification))
                {
                    switch (userNetWorkOpt_INTVerification)
                    {
                        //[1] this case is for pinging an IP Addr
                        case 1:
                            Console.WriteLine("    - Ping -");
                            Console.WriteLine("Enter a Vaild IP Address to ping: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(">");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            //User enters an IP Addr. If not valid the program will simply not be able to ping (will return false)
                            String IPAddrPing = Console.ReadLine();

                            //Calls function that trys to ping the IP Addr and returns true(success) || false(failed)
                            bool pingStatus = tryPing(IPAddrPing);
                            Console.ForegroundColor = ConsoleColor.White;
                            
                            //If true show Success
                            if (pingStatus == true)
                            {
                                Console.Write("Ping Status: ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("SUCCESS");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            //else show ping failed
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("Ping Status: ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Failed to Ping");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            break;
                        //[2] deals with scanning ports on an target IP Addr
                        case 2:
                            Console.WriteLine("    - Port Scan -");
                            Console.WriteLine("Enter a Vaild IP Address to ping: ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            //As the scan will run as thread pressing B will cause a break without constantly breaking the loop
                            Console.WriteLine("Press B to go Stop Scanning");
                            Console.ForegroundColor = ConsoleColor.Green;
                            //User enters an IP Addr. if not valid the program will simply not be able to ping (will return false)
                            Console.Write(">");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            String IPAddrPort = Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            
                            //THREAD created with paramater 
                            Thread portScanner = new Thread(() => portScanner_Thread(IPAddrPort));
                            
                            //if false loop will break
                            bool portScanBool = true;

                            //START THREAD
                            portScanner.Start();
                            while (portScanBool)
                            {
                                //IF B is pressed the loop breaks and thread aborts
                                ConsoleKey userKey = Console.ReadKey(true).Key;
                                if (userKey == ConsoleKey.B)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Stoping Scanning...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    portScanBool = false;
                                }         
                                //if NOT B is pressed display message
                                else if (userKey != ConsoleKey.B)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Press B to go back !");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            //abort Thread
                            portScanner.Abort();
                            break;
                        case 3:
                            Console.WriteLine("    - Subnet Scan -");                           
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Note - Will only work with 255.255.255.0 Subnet Mask\nEg: 192.168.1");
                            Console.ForegroundColor = ConsoleColor.Red;
                            //As the scan will run as thread pressing B will cause a break without constantly breaking the loop
                            Console.WriteLine("Press B to go Stop Scanning");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Enter a Vaild IP Subnet Address: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            //User enters an IP Addr. if not valid the program will simply not be able to ping (will return false)
                            Console.Write(">");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            String IPSubNet = Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.White;

                            //THREAD created with paramater 
                            Thread subNetScanner = new Thread(() => subNetScanner_Thread(IPSubNet));

                            //if false loop will break
                            bool subNetScanBool = true;

                            //Start Thread
                            subNetScanner.Start();
                            while (subNetScanBool)
                            {
                                //IF B is pressed the loop breaks and thread aborts
                                ConsoleKey userKey = Console.ReadKey(true).Key;
                                if (userKey == ConsoleKey.B)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Stoping Scanning...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    subNetScanBool = false;
                                }
                                else if (userKey != ConsoleKey.B)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Press B to go back !");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            //Thread is Abored
                            subNetScanner.Abort();

                            break;
                        case 4:
                            //set flag to false exiting the main network loop
                            netflag = false;
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Menu Option !");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
            }
            //returns netFlag = true unless case 4 was selected
            return netflag;

        }

        // The thread for the subnet scan
        public static void subNetScanner_Thread(String IPSubNet)
        {
            //loop to increment from .2 --> .255 on the 4th octet of the IPAddr
            for (int octet4 = 2; octet4 < 255; octet4++)
            {
                String fullIPAddr = $"{IPSubNet}.{octet4}";
                
                //Call function
                bool subNetPingStatus = tryPing(fullIPAddr);

                Console.ForegroundColor = ConsoleColor.White;
                //If function call returns true display IP Addr and success
                if (subNetPingStatus == true)
                {
                    Console.Write("{0} - ", fullIPAddr);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCESS");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                //else if function call returns false display IP Addr and failed
                else
                {
                    Console.Write("{0} - ", fullIPAddr);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // The thread for the port scan
        public static void portScanner_Thread(String IPAddrPort)
        {
            //Loop for all Well-Knows Ports
            for (int port = 0; port < 1023; port++)
            {
                //call Function with IPADDR and port which is the index of the loop
                bool portStatus = scanPorts(IPAddrPort, port);

                //if function call returns true print that port and message "open"
                if (portStatus == true)
                {
                    Console.Write("Port {0}: ", port);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OPEN");
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
        }
        // This function is responsible for trying to see if a port is open
        public static bool scanPorts(String IPAddrDest, int port)
        {
            //returns false unless changed to true incase of open port
            bool portStat = false;
            try
            {
                //uses the tcpClient class
                using (var client = new TcpClient())
                {
                    //trys to connect if connection successful --> TRUE
                    var result = client.BeginConnect(IPAddrDest, port, null, null);
                    client.EndConnect(result);
                    portStat = true;
                }
            }
            //if unable to connect catch and keep flag as false
            catch
            {
                portStat = false;
            }
            return portStat;
        }

        // This function is responsible for trying to ping
        public static bool tryPing(String IPAddr)
        {
            //returns false unless set to true because of successful ping
            bool pingStatus = false;
            Ping pingObj = null;

            try
            {
                //try to ping with user IPAddr
                pingObj = new Ping();
                PingReply reply = pingObj.Send(IPAddr);
                //if successful set the ping status to true
                pingStatus = reply.Status == IPStatus.Success;
            }
            //if unsucessfull keep false
            catch (PingException)
            {
                pingStatus = false;
            }
            finally
            {
                if (pingObj != null)
                {
                    pingObj.Dispose();
                }
            }

            return pingStatus;
        }


        //Debug Help Library (dbghelp.dll)
        //https://docs.microsoft.com/en-us/windows/win32/api/minidumpapiset/nf-minidumpapiset-minidumpwritedump
        [DllImport("dbghelp.dll", SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);


        //This function is responsible for creating a dump file in the current working directory
        public static void createDump(IntPtr processhandle, uint processId, string processname)
        {
            try
            {
                bool status;
                //set file name and extension 
                string filename = processname + "_" + processId + ".dmp";

                //create file 
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                {
                    //call MiniDumpWriteDump from the .dll with correct parameters returns bool value 
                    status = MiniDumpWriteDump(processhandle, processId, fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
                //if status = TRUE print message
                if (status)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Dump Status: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCESS");
                    Console.ForegroundColor = ConsoleColor.White;
                    //Print where the file was saved
                    Console.WriteLine($"{processname} process dump saved at {Directory.GetCurrentDirectory()}\\{filename}");
                }
                //if status = False print message
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Dump Status: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR\nCannot Dump the process");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Marshal.GetExceptionCode());
                }
            }
            //if Error print message
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        //This function shows CPU stats
        public static void showCPUStats()
        {
            //Create object (I made these methods into a seprate class to make this file shorter and keep things cleaner)
            CPUStats CPUStatsObj = new CPUStats();

            //Call all Methods from that obj
            CPUStatsObj.CPUName();
            CPUStatsObj.CPUModel();
            CPUStatsObj.CPUStatus();
            CPUStatsObj.CPUArch();
            CPUStatsObj.CPUNumCores();
            CPUStatsObj.CPUNumLCores();
            CPUStatsObj.CPUVirtualizationStatus();
        }

        public static void showRAMStats()
        {
            Console.WriteLine("   - RAM Memory -");
            //Create object (I made these methods into a seprate class to make this file shorter and keep things cleaner)
            RAMStats RAMStatsObj = new RAMStats();

            //Call all Methods from that obj
            RAMStatsObj.RAMCapacity();
            Console.WriteLine("\n");
            RAMStatsObj.RAMArch();
            Console.WriteLine("\n");
            RAMStatsObj.RAMClockSpeed();
            Console.WriteLine("\n");
            RAMStatsObj.RAMModel();
            Console.WriteLine("\n");
            Console.WriteLine("   - Virtual Memory -\n");
            RAMStatsObj.virtualMemName();
            RAMStatsObj.virtualMemSize();
            RAMStatsObj.virtualMemEncryptionStat();

        }

        //Prints a dashed line 
        public static void pageBreak()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }


        //Values for Dumping processes from Memory
        //Values can be found at: https://docs.microsoft.com/en-us/windows/win32/api/minidumpapiset/nf-minidumpapiset-minidumpwritedump
        public static class MINIDUMP_TYPE
        {
            public const int MiniDumpNormal = 0x00000000;
            public const int MiniDumpWithDataSegs = 0x00000001;
            public const int MiniDumpWithFullMemory = 0x00000002;
            public const int MiniDumpWithHandleData = 0x00000004;
            public const int MiniDumpFilterMemory = 0x00000008;
            public const int MiniDumpScanMemory = 0x00000010;
            public const int MiniDumpWithUnloadedModules = 0x00000020;
            public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
            public const int MiniDumpFilterModulePaths = 0x00000080;
            public const int MiniDumpWithProcessThreadData = 0x00000100;
            public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
            public const int MiniDumpWithoutOptionalData = 0x00000400;
            public const int MiniDumpWithFullMemoryInfo = 0x00000800;
            public const int MiniDumpWithThreadInfo = 0x00001000;
            public const int MiniDumpWithCodeSegs = 0x00002000;
        }
    }
}

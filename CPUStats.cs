using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;

/// <summary>
/// Author: Mateusz Peplinski
/// Date v .10 Published: 01 / 08 / 2021
/// License: MIT 
/// © 2021 Mateusz Peplinski
/// </summary>

//This class is created with the help of WMI code Creator v1.0
// https://www.microsoft.com/en-us/download/details.aspx?id=8572
// All methods here use Windows Management Instrumentation (WMI)
// they are called from root//CIMV2 and call methods from the Win32 API



namespace RUST
{
    class CPUStats
    {
        public void CPUName()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Computer Name: {0}", queryObj["Name"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public void CPUModel()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("CPU: {0}", queryObj["Name"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUStatus()
        {
            //Status
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.Write("Status: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0}", queryObj["Status"]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUArch()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Console.WriteLine("Address Width: {0}", queryObj["AddressWidth"] + "bit");
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUNumCores()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Number Of CPU Cores: {0}", queryObj["NumberOfCores"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUNumLCores()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Console.WriteLine("Number Of Logical Processors: {0}", queryObj["NumberOfLogicalProcessors"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUVirtualizationStatus()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("VirtualizationFirmwareEnabled: {0}", queryObj["VirtualizationFirmwareEnabled"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
    }
}

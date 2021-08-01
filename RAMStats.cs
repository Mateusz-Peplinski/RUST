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
    class RAMStats
    {
        public void RAMCapacity()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    double dblMemory;
                    if (double.TryParse(Convert.ToString(queryObj["TotalPhysicalMemory"]), out dblMemory))
                    {
                        Console.WriteLine("TotalPhysicalMemory is: {0} MB", Convert.ToInt32(dblMemory / (1024 * 1024)));
                        Console.WriteLine("TotalPhysicalMemory is: {0} GB", Convert.ToInt32(dblMemory / (1024 * 1024 * 1024)));
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void RAMArch()
        {
            int RAMSlotCounter = 0;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Console.WriteLine("DIMM {0} - TotalWidth: {1}", RAMSlotCounter, queryObj["TotalWidth"] + "bit");
                    RAMSlotCounter++;
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void RAMClockSpeed()
        {
            int RAMSlotCounter = 0;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Console.WriteLine("DIMM:{0} - Clock Speed: {1}", RAMSlotCounter, queryObj["ConfiguredClockSpeed"] + "Mhz");
                    RAMSlotCounter++;
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void RAMModel()
        {
            int RAMSlotCounter = 0;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("DIMM {0} - Manufacturer: {1}", RAMSlotCounter, queryObj["Manufacturer"]);
                    RAMSlotCounter++;
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void virtualMemName()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PageFileSetting");

                Console.WriteLine("System Page File/s:");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    Console.WriteLine("Name: {0}", queryObj["Name"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void virtualMemEncryptionStat()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PageFile");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Encrypted: {0}", queryObj["Encrypted"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            
        }
        public void virtualMemSize()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PageFile");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    double dblVMemory;
                    if (double.TryParse(Convert.ToString(queryObj["FileSize"]), out dblVMemory))
                    {
                        Console.WriteLine("Page file Size is: {0} GB", Convert.ToInt32(dblVMemory / (1024 * 1024 * 1024)));
                    }
                }
                
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
    }
}

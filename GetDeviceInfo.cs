using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class GetDeviceInfo
    {
        //Device Information Variables
        private string mBB, proserial, hdserial, biosLabel, proInfoL, version, Manufacturer;
        private Byte[] OBKData = new Byte[30];
        private Byte[] CBKData = new Byte[30];
        private Byte[] real_ACT = new Byte[30];

        public GetDeviceInfo() 
        {
            mBB = getMotherBoardSerial();
            proserial = getProcessorID();
            hdserial = getHardDiskID();
        }

        //Get device information
        public string DeviceInfo()
        {
            Byte[] MBB_Buffer = Encoding.ASCII.GetBytes(mBB);
            Byte[] Proc_Buffer = Encoding.ASCII.GetBytes(proserial);
            Byte[] HDD_Buffer = Encoding.ASCII.GetBytes(hdserial);

            for (int i = 0; i < 10; i++)
                OBKData[i] = MBB_Buffer[i];

            for (int i = 10, j = 0; i < 20; i++, j++)
                OBKData[i] = Proc_Buffer[j];

            for (int i = 20, j = 0; i < 30; i++, j++)
                OBKData[i] = HDD_Buffer[j];

            for (int i = 0; i < 30; i++)
            {
                int temp1 = (int)OBKData[i] + 7;
                CBKData[i] = (Byte)temp1;
            }
            var str = System.Text.Encoding.Default.GetString(CBKData);
            return str;
        }

        private string getMotherBoardSerial()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();
            string motherBoard = "";
            foreach (ManagementObject mo in moc)
            {
                motherBoard = (string)mo["SerialNumber"];
            }
            return motherBoard;
        }

        private string getProcessorInfo()
        {
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    return ((string)processor_name.GetValue("ProcessorNameString"));   //Display processor ingo.
                }
                return ("");
            }
            return ("");
        }

        private string getManufacturer()
        {
            try
            {
                // create management class object
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                //collection to store all management objects
                ManagementObjectCollection moc = mc.GetInstances();
                if (moc.Count != 0)
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        // display general system information
                        return (mo["Manufacturer"].ToString());
                    }
                    return ("");
                }
                return ("");
            }
            catch (ManagementException t)
            {
                //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
                return (t.Message.ToString());
            }
        }

        //get machine version
        private string getVersion()
        {

            SelectQuery query = new SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            try
            {
                //execute the query
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    //print system info
                    process.Get();
                    return ((string)process["Model"]);
                }
                return ("");
            }

            catch (ManagementException t)
            {
                //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
                return (t.Message.ToString());
            }
        }

        //get bios info 
        private string getBios()
        {
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection collection = searcher1.Get();

            try
            {
                foreach (ManagementObject obj in collection)
                {
                    if (((string[])obj["BIOSVersion"]).Length > 1)
                        return (((string[])obj["BIOSVersion"])[0] + ((string[])obj["BIOSVersion"])[1]);
                    else
                        return (((string[])obj["BIOSVersion"])[0]);
                }
                return ("");
            }
            catch (ManagementException t)
            {
                //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
                return (t.Message.ToString());
            }
        }

        private string getProcessorID()
        {
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }
            return id;
        }

        private string getHardDiskID()
        {
            string hddID = null;
            ManagementClass mc = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject strt in moc)
            {
                hddID += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return hddID.Trim().ToString();
        }
    }
}

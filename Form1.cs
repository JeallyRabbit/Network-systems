using System.Net;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Text;
using System.Security.Principal;
//using System.Management;

namespace Network_Systems
{
    public partial class Form1 : Form
    {
        private void UpdateProgressBar(int progress)
        {
            if (progressBar1.InvokeRequired)
            {
                // Use Invoke to marshal the call to the UI thread
                progressBar1.Invoke(new System.Action<int>(UpdateProgressBar), progress);
            }
            else
            {
                // Update the progress bar value
                if (progressBar1.Value < progressBar1.Maximum)
                {
                    progressBar1.Value = progress;

                }
            }
        }

        private void UpdateLogTextBox(string logText)
        {
            if (textBoxLogs.InvokeRequired)
            {
                textBoxLogs.Invoke(new System.Action<string>(UpdateLogTextBox), logText);
            }
            else
            {
                textBoxLogs.AppendText(logText+"\n");
                textBoxLogs.ScrollToCaret();
            }
        }


        bool stop = false;
        public Form1()
        {
            /*
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool AllocConsole();
            */

            InitializeComponent();
            //AllocConsole();
        }
        class Pc
        {
            public string ip, systemName, procName, procGen, hostName, tpmVersion, SN;
            public bool tpmEnabled, tpmActivated;


            public Pc(string ip, string systemName, string procName,
                string procGen, string hostName, string tpmVersion, string SN, bool tpmEnabled, bool tpmActivated)
            {
                this.ip = ip;
                this.systemName = systemName;
                this.procName = procName;
                this.procGen = procGen;
                this.hostName = hostName;
                this.tpmVersion = tpmVersion;
                this.SN = SN;
                this.tpmEnabled = tpmEnabled;
                this.tpmActivated = tpmActivated;
            }
        }

        class myAddress
        {
            public string ip;
            public bool is_scanned;

            public myAddress(string ip, bool is_scanned = false)
            {
                this.ip = ip;
                this.is_scanned = is_scanned;
            }
        }


        private async void btn_start_Click(object sender, EventArgs e)
        {

            //need ip address, system name, processor name, processor generation (have to be 8th or greater)
            List<Pc> scanned = new List<Pc>();
            List<myAddress> addresses = new List<myAddress>();
            int i = Int32.Parse(textBoxStart1.Text); int j = Int32.Parse(textBoxStart2.Text);
            int max_i = Int32.Parse(textBoxEnd1.Text), max_j = Int32.Parse(textBoxEnd2.Text);
            int to_scan = 0;
            while (i <= max_i || j <= max_j)
            {
                if (i > max_i) { break; }
                if (i == max_i && j <= max_j)
                {
                    if (j > 0 && j < 255 && (i == 0 || i == 1 || i == 6 || i == 7 || i == 10 || i == 11 || i == 14 || i == 15 || i == 16))
                    {
                        // conditions for 'i' are for computers pc subnetworks
                        addresses.Add(new myAddress("192.168." + i.ToString() + "." + j.ToString(), false));
                        to_scan++;
                    }
                }
                else if (i < max_i)
                {
                    if (j > 0 && j < 255 && (i == 0 || i == 1 || i == 6 || i == 7 || i == 10 || i == 11 || i == 14 || i == 15 || i == 16))
                    {
                        // conditions for 'i' are for computers pc subnetworks
                        addresses.Add(new myAddress("192.168." + i.ToString() + "." + j.ToString(), false));
                        to_scan++;
                    }
                }
                //Console.WriteLine(i.ToString() + " " + j.ToString());

                j++;
                if (j >= 255) { j = 1; i++; }

            }
            int scanned_num = 0;
            //Console.WriteLine("Addresses to scan: " + to_scan.ToString());
            //string a = (addresses[0].ip);
            //Console.WriteLine(to_scan); Console.WriteLine(a);
            Task.Run(() => // task.run makes messageBox.Show run async
            {
                MessageBox.Show("Scanning...");
            });

            while (scanned_num < to_scan && stop == false)
            {
                await Task.Run(() =>
                Parallel.ForEach(addresses, (address, state) =>
                {
                    if (stop == true) { state.Break(); }
                    if (address.is_scanned == false && stop == false)
                    {
                        //Console.WriteLine("processing address: " + address.ip.ToString());
                        UpdateLogTextBox("processing address: " + address.ip.ToString());
                        IPHostEntry hostName;
                        try
                        {
                            hostName = Dns.GetHostEntry(address.ip.ToString());
                        }
                        catch (Exception e) { hostName = null; }
                        if (hostName != null)
                        {
                            //Console.WriteLine("Attempting: "+hostName.HostName.ToString());
                            // setting 
                            string wmiNamespace = @"\\{0}\root\cimv2";

                            ManagementScope scope = new ManagementScope(string.Format(wmiNamespace, hostName.HostName));



                            try
                            {
                                // Connect to the WMI scope
                                scope.Connect();
                                //Console.WriteLine("trying to connect");

                                // Define a query to retrieve information (example: Win32_OperatingSystem)
                                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");

                                // Create a management object searcher
                                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                                // Get the WMI object collection
                                ManagementObjectCollection queryCollection = searcher.Get();
                                string ip, foundSystemName = "null", foundProcName = "null", foundProcGen = "null", foundHostName;
                                ip = address.ip.ToString();
                                foundHostName = hostName.HostName;
                                // get info about system
                                foreach (ManagementObject m in queryCollection)
                                {
                                    foundSystemName = m["Caption"].ToString();

                                }


                                // get info about processor

                                query = new ObjectQuery("Select * FROM Win32_Processor");

                                searcher = new ManagementObjectSearcher(scope, query);
                                queryCollection = searcher.Get();
                                foreach (ManagementObject m in queryCollection)
                                {
                                    foundProcName = m["Name"].ToString();
                                    for (int i = 0; i < foundProcName.Length; i++)
                                    {
                                        if (foundProcName[i] == '-' && i < foundProcName.Length - 1)
                                        {
                                            foundProcGen = foundProcName[i + 1].ToString();
                                            if (foundProcName[i + 1] == '1' && i < foundProcName.Length - 2) { foundProcGen += foundProcName[i + 2].ToString(); }
                                            break;
                                        }
                                    }
                                }

                                // get info about tpm
                                string foundTpmVersion = "";

                                //wmiNamespace = @"\\{0}\root\cimv2\Security\MicrosoftTpm";
                                wmiNamespace = "\\\\" + hostName.HostName + "\\root\\cimv2\\Security\\MicrosoftTpm";

                                scope = new ManagementScope(string.Format(wmiNamespace));
                                scope.Options.Authentication = System.Management.AuthenticationLevel.PacketPrivacy;
                                //scope.Connect();
                                query = new ObjectQuery("Select * FROM Win32_TPM");
                                searcher = new ManagementObjectSearcher(scope, query);
                                //WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
                                //MessageBox.Show(WindowsIdentity.GetCurrent().ToString());

                                queryCollection = searcher.Get();
                                string version = "";
                                string isActivadedString, isEnabledString;
                                bool isActivaded = false, isEnabled = false;
                                foreach (ManagementObject m in queryCollection)
                                {
                                    version = m["SpecVersion"].ToString();
                                    isActivadedString = m["IsActivated_InitialValue"].ToString();
                                    if (isActivadedString == "True") { isActivaded = true; }
                                    isEnabledString = m["IsEnabled_InitialValue"].ToString();
                                    if (isEnabledString == "True") { isEnabled = true; }
                                    for (int i = 0; i < version.Length; i++)
                                    {
                                        if (version[i] != ',')
                                        {
                                            foundTpmVersion += version[i];
                                        }
                                        else { break; }

                                    }
                                }

                                //get SN
                                string foundSN = "";
                                wmiNamespace = "\\\\" + hostName.HostName + "\\root\\cimv2";
                                scope = new ManagementScope(string.Format(wmiNamespace));
                                query = new ObjectQuery("Select * FROM Win32_Bios");
                                searcher = new ManagementObjectSearcher(scope, query);
                                queryCollection = searcher.Get();

                                foreach (ManagementObject m in queryCollection)
                                {
                                    foundSN = m["SerialNumber"].ToString();

                                    //Console.WriteLine("Found SN: " + foundSN);
                                }


                                scanned.Add(new Pc(ip, foundSystemName, foundProcName, foundProcGen, foundHostName, foundTpmVersion, foundSN, isEnabled, isActivaded));
                                /*
                                Console.WriteLine(ip.ToString() + " | " + foundSystemName.ToString() + " | " + foundProcName.ToString() + " | " + foundProcGen.ToString() +
                                    " | " + foundHostName.ToString() + " | " + foundTpmVersion + " | " + foundSN + " | isActivated: " + isActivaded + " | isEnabled: " + isEnabled);
                                */
                                UpdateLogTextBox((ip.ToString() + " | " + foundSystemName.ToString() + " | " + foundProcName.ToString() + " | " + foundProcGen.ToString() +
                                    " | " + foundHostName.ToString() + " | " + foundTpmVersion + " | " + foundSN + " | isActivated: " + isActivaded + " | isEnabled: " + isEnabled) + "\n");
                                /*labelLastScanned.Text = ip.ToString() + " " + foundSystemName.ToString() + " " + foundProcName.ToString() +
                                " " + foundProcGen.ToString() + " " + foundHostName.ToString();
                                labelLastScanned.Visible = true;
                                */
                                address.is_scanned = true;
                                scanned_num++;
                                float progress = (float)scanned_num / (float)to_scan * (progressBar1.Maximum);

                                UpdateProgressBar((int)progress);
                                //progressBar1.Value = to_scan / scanned_num;

                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                // Console.WriteLine("Access denied: {0}", ex.Message);
                                UpdateLogTextBox("Access denied: {0}" + ex.Message.ToString());
                            }
                            catch (ManagementException ex)
                            {
                                //Console.WriteLine("WMI error: {0}", ex.Message);
                                UpdateLogTextBox("WMI error: {0}" + ex.Message.ToString());
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine("General error: {0}", ex.Message);
                                UpdateLogTextBox("General error: {0}" + ex.Message.ToString()); ;
                            }

                        }
                    }



                }));
            }

            //Console.WriteLine("Finished Scanning");
            UpdateLogTextBox("Finished Scanning");
            // Create a new Excel application instance
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create a new Workbook object
            Workbook workbook = excel.Workbooks.Add();

            // Get the first worksheet
            Worksheet worksheet = (Worksheet)workbook.Worksheets[1];


            int row = 2;
            worksheet.Cells[1, 1] = "Ip";
            worksheet.Cells[1, 2] = "hostName";
            worksheet.Cells[1, 3] = "systemName";
            worksheet.Cells[1, 4] = "procGen";
            worksheet.Cells[1, 5] = "procName";
            worksheet.Cells[1, 6] = "TpmVersion";
            worksheet.Cells[1, 7] = "tpmEnabled";
            worksheet.Cells[1, 8] = "tpmActivated";
            worksheet.Cells[1, 9] = "SN";
            int sufficient_procs = 0, insufficient_procs = 0;
            foreach (Pc unit in scanned)
            {
                worksheet.Cells[row, 1] = unit.ip;
                worksheet.Cells[row, 2] = unit.hostName;
                worksheet.Cells[row, 3] = unit.systemName;
                worksheet.Cells[row, 4] = unit.procGen;
                worksheet.Cells[row, 5] = unit.procName;
                byte[] asciiBytes = Encoding.ASCII.GetBytes(unit.procGen);
                string tmpTpmVersion = "";
                for (int k = 0; k < unit.tpmVersion.Length; k++)
                {
                    if (unit.tpmVersion[k] == '.') { tmpTpmVersion += ','; }
                    else if (unit.tpmVersion[k] != ',') { tmpTpmVersion += unit.tpmVersion[k]; }
                    //if (unit.tpmVersion[k] != ',') { tmpTpmVersion+= unit.tpmVersion[k]; }
                    else { break; }
                }
                if (tmpTpmVersion.Length == 0)
                {
                    tmpTpmVersion = "0,0";
                }
                float tpmVersion_float = float.Parse(tmpTpmVersion);
                worksheet.Cells[row, 6] = tmpTpmVersion;
                worksheet.Cells[row, 7] = unit.tpmEnabled;
                worksheet.Cells[row, 8] = unit.tpmActivated;
                worksheet.Cells[row, 9] = unit.SN;

                if ((asciiBytes[0] < 56 && asciiBytes.Length == 1) || tpmVersion_float < 2.0)
                {

                    for (int c = 1; c < 10; c++)
                    {
                        worksheet.Cells[row, c].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        if ((asciiBytes[0] < 56 && asciiBytes.Length == 1) && tpmVersion_float < 2.0)
                        {
                            worksheet.Cells[row, c].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            worksheet.Cells[row, c].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        }

                    }
                    insufficient_procs++;
                }
                else
                {
                    for (int c = 1; c < 10; c++)
                    {
                        worksheet.Cells[row, c].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    }
                    sufficient_procs++;
                }
                row++;
            }
            worksheet.Cells[1, 10] = "8th or greater processors and TPM 2.0 and above: " + sufficient_procs.ToString();
            worksheet.Cells[1, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            worksheet.Cells[2, 10] = "below 8th processors or tpm below 2.0: " + insufficient_procs.ToString();
            worksheet.Cells[2, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (addresses.Count > 0)
            {

                string filePath = Path.Combine(desktopPath, "Scanning " + addresses[0].ip.ToString() +
                " - " + addresses[addresses.Count - 1].ip.ToString() + ".xlsx");
                try
                {
                    workbook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
                                Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange,
                                XlSaveConflictResolution.xlUserResolution, true, Missing.Value,
                                Missing.Value, Missing.Value);
                    // Close the workbook and release the COM objects
                    workbook.Close();
                    excel.Quit();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Not saved to file");
                }

                try
                {
                    workbook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
                                Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange,
                                XlSaveConflictResolution.xlUserResolution, true, Missing.Value,
                                Missing.Value, Missing.Value);
                    // Close the workbook and release the COM objects
                    workbook.Close();
                    excel.Quit();
                }
                catch (Exception ex)
                {

                }

            }
            else { MessageBox.Show("Nothing Found"); }





            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            MessageBox.Show("Saved to file.");
            UpdateProgressBar(0);
            stop = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            MessageBox.Show("Stopping scanning.");
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nie goraczkuj sie...");
        }

        private void textBoxStart1_TextChanged(object sender, EventArgs e)
        {
            string input = textBoxStart1.Text;
            if (input.Length == 0) { return; }
            int MinValue = 0, MaxValue = 255;
            if (int.TryParse(input, out int value))
            {
                if (value < MinValue || value > MaxValue)
                {
                    MessageBox.Show($"Please enter a number between {MinValue} and {MaxValue}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxStart1.SelectAll();
                }
            }
            else if (!string.IsNullOrEmpty(this.Text)) // If it's not empty and not a number
            {
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxStart1.SelectAll();
            }
        }

        private void textBoxStart2_TextChanged(object sender, EventArgs e)
        {
            string input = textBoxStart2.Text;
            if (input.Length == 0) { return; }
            int MinValue = 0, MaxValue = 255;
            if (int.TryParse(input, out int value))
            {
                if (value < MinValue || value > MaxValue)
                {
                    MessageBox.Show($"Please enter a number between {MinValue} and {MaxValue}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxStart2.SelectAll();
                }
            }
            else if (!string.IsNullOrEmpty(this.Text)) // If it's not empty and not a number
            {
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxStart2.SelectAll();
            }
        }

        private void textBoxEnd1_TextChanged(object sender, EventArgs e)
        {
            string input = textBoxEnd1.Text;
            if (input.Length == 0) { return; }
            int MinValue = 0, MaxValue = 255;
            if (int.TryParse(input, out int value))
            {
                if (value < MinValue || value > MaxValue)
                {
                    MessageBox.Show($"Please enter a number between {MinValue} and {MaxValue}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxEnd1.SelectAll();
                }
            }
            else if (!string.IsNullOrEmpty(this.Text)) // If it's not empty and not a number
            {
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEnd1.SelectAll();
            }
        }

        private void textBoxEnd2_TextChanged(object sender, EventArgs e)
        {
            string input = textBoxEnd2.Text;
            if (input.Length == 0) { return; }
            int MinValue = 0, MaxValue = 255;
            if (int.TryParse(input, out int value))
            {
                if (value < MinValue || value > MaxValue)
                {
                    MessageBox.Show($"Please enter a number between {MinValue} and {MaxValue}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxEnd2.SelectAll();
                }
            }
            else if (!string.IsNullOrEmpty(this.Text)) // If it's not empty and not a number
            {
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEnd2.SelectAll();
            }
        }
    }
}
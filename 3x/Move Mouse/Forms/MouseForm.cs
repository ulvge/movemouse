using Ellanet.Classes;
using Ellanet.Events;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Ellanet.Forms
{
    public partial class MouseForm : Form
    {
        private const int TraceSeconds = 5;
        private const string MoveMouseXmlName = "Move Mouse.xml";
        //private const string HomeAddress = "http://movemouse.codeplex.com/";
        private const string ContactAddress = "mailto:contact@movemouse.co.uk?subject=Move%20Mouse%20Feedback";
        private const string HelpAddress = "http://www.movemouse.co.uk";
        private const string PayPalAddress = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QZTWHD9CRW5XN";
        private const string UpdateXmlUrl = "https://raw.githubusercontent.com/sw3103/movemouse/master/Update_3x.xml";
        private const string MiceResourceUrlPrefix = "https://raw.githubusercontent.com/sw3103/movemouse/master/Mice/";
        private const string MiceXmlName = "Mice.xml";

        private readonly TimeSpan _waitBetweenUpdateChecks = new TimeSpan(7, 0, 0, 0);
        private readonly TimeSpan _waitUntilAutoMoveDetect = new TimeSpan(0, 0, 2);
        private readonly System.Windows.Forms.Timer _mouseTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer _resumeTimer = new System.Windows.Forms.Timer();
        private readonly string _moveMouseTempDirectory = Environment.ExpandEnvironmentVariables(@"%Temp%\Ellanet\Move Mouse");
        private readonly bool _suppressAutoStart;
        private readonly string _homeAddress;
        private DateTime _mmStartTime;
        private Point _startingMousePoint;
        private DateTime _traceTimeComplete = DateTime.MinValue;
        private Thread _traceMouseThread;
        private BlackoutStatusChangedEventArgs.BlackoutStatus _blackoutStatus = BlackoutStatusChangedEventArgs.BlackoutStatus.Inactive;
        private DateTime _lastUpdateCheck = DateTime.MinValue;
        private string _scriptEditor = Path.Combine(Environment.ExpandEnvironmentVariables("%WINDIR%"), @"System32\notepad.exe");
        private int _mouseTimerTicks;
        private List<CelebrityMouse> _celebrityMice;
        private bool _easterEggActive;
        private PowerLineStatusChangedEventArgs.PowerLineStatus _powerLineStatus = PowerLineStatusChangedEventArgs.PowerLineStatus.Online;

        private delegate void UpdateCountdownProgressBarDelegate(ref ProgressBar pb, int delay, int elapsed);

        private delegate void ButtonPerformClickDelegate(ref Button b);

        private delegate object GetComboBoxSelectedItemDelegate(ref ComboBox cb);

        private delegate int GetComboBoxSelectedIndexDelegate(ref ComboBox cb);

        private delegate object GetComboBoxTagDelegate(ref ComboBox cb);

        private delegate void SetNumericUpDownValueDelegate(ref NumericUpDown nud, int value);

        private delegate void SetButtonTextDelegate(ref Button b, string text);

        private delegate void SetButtonTagDelegate(ref Button b, object o);

        private delegate object GetButtonTagDelegate(ref Button b);

        private delegate string GetButtonTextDelegate(ref Button b);

        private delegate bool GetCheckBoxCheckedDelegate(ref CheckBox cb);

        private delegate void AddComboBoxItemDelegate(ref ComboBox cb, string item, bool selected);

        private delegate void ClearComboBoxItemsDelegate(ref ComboBox cb);

        private delegate void ShowCelebrityMouseDelegate(CelebrityMouse cb);

        private delegate bool IsWindowMinimisedDelegate(IntPtr handle);

        private delegate void ZeroParameterDelegate();

        public delegate void BlackoutStatusChangedHandler(object sender, BlackoutStatusChangedEventArgs e);

        public delegate void NewVersionAvailableHandler(object sender, NewVersionAvailableEventArgs e);

        public delegate void ScheduleArrivedHandler(object sender, ScheduleArrivedEventArgs e);

        public delegate void PowerLineStatusChangedHandler(object sender, PowerLineStatusChangedEventArgs e);

        public delegate void PowerShellexecutionPolicyWarningHandler(object sender);

        public delegate void HookKeyStatusChangedHandler(object sender, HookKeyStatusChangedEventArgs e);

        public delegate void MoveMouseStartedHandler();

        public delegate void MoveMousePausedHandler();

        public delegate void MoveMouseStoppedHandler();

        public event BlackoutStatusChangedHandler BlackoutStatusChanged;
        public event NewVersionAvailableHandler NewVersionAvailable;
        public event ScheduleArrivedHandler ScheduleArrived;
        public event PowerLineStatusChangedHandler PowerLineStatusChanged;
        public event HookKeyStatusChangedHandler HookKeyStatusChanged;
        public event MoveMouseStartedHandler MoveMouseStarted;
        public event MoveMousePausedHandler MoveMousePaused;
        public event MoveMouseStoppedHandler MoveMouseStopped;

        public bool MinimiseToSystemTrayWarningShown { get; private set; }

        private enum Script
        {
            Start,
            Interval,
            Pause
        }

        private enum PowerShellExecutionPolicy
        {
            Restricted,
            AllSigned,
            RemoteSigned,
            Unrestricted
        }

        [Flags]
        private enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        [Flags]
        private enum Win32Consts
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        [Flags]
        private enum ShowWindowCommands
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNa = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            private static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
            [MarshalAs(UnmanagedType.U4)] public int cbSize;
            [MarshalAs(UnmanagedType.U4)] public int dwTime;
        }

        private struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public readonly int flags;
            public readonly ShowWindowCommands showCmd;
            public readonly Point ptMinPosition;
            public readonly Point ptMaxPosition;
            public readonly Rectangle rcNormalPosition;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(
            uint dwFlags,
            uint dx,
            uint dy,
            uint dwData,
            int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(
            ref LASTINPUTINFO plii);

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(
            int uAction,
            int uParam,
            ref int lpvParam,
            int flags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(
            uint nInputs,
            ref INPUT pInputs,
            int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(
            IntPtr hWnd,
            ShowWindowCommands nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(
            IntPtr hWnd,
            ref WINDOWPLACEMENT lpwndpl);

        public MouseForm(bool suppressAutoStart)
        {
            InitializeComponent();
            _suppressAutoStart = suppressAutoStart;
            int screenSaverTimeout = GetScreenSaverTimeout();

            if (screenSaverTimeout > 0)
            {
                if ((decimal)(screenSaverTimeout / 2.0) > resumeNumericUpDown.Maximum)
                {
                    resumeNumericUpDown.Value = resumeNumericUpDown.Maximum;
                }
                else
                {
                    resumeNumericUpDown.Value = (decimal)(screenSaverTimeout / 2.0);
                }
            }

            keystrokeCheckBox.CheckedChanged += keystrokeCheckBox_CheckedChanged;
            appActivateCheckBox.CheckedChanged += appActivateCheckBox_CheckedChanged;
            staticPositionCheckBox.CheckedChanged += startPositionCheckBox_CheckedChanged;
            resumeCheckBox.CheckedChanged += resumeCheckBox_CheckedChanged;
            launchAtLogonCheckBox.CheckedChanged += launchAtLogonCheckBox_CheckedChanged;
            hotkeyCheckBox.CheckedChanged += hotkeyCheckBox_CheckedChanged;
            hotkeyComboBox.SelectedIndexChanged += hotkeyComboBox_SelectedIndexChanged;
            ReadSettings();
            _homeAddress = "";
            Icon = Properties.Resources.Mouse_Icon;
            Text = String.Format("车辆保险 ({0}.{1}.{2})   {3}", Assembly.GetExecutingAssembly().GetName().Version.Major, Assembly.GetExecutingAssembly().GetName().Version.Minor, Assembly.GetExecutingAssembly().GetName().Version.Build, _homeAddress);
            FormClosing += MouseForm_FormClosing;
            Load += MouseForm_Load;
            Resize += MouseForm_Resize;
            actionButton.Click += actionButton_Click;
            moveMouseCheckBox.CheckedChanged += moveMouseCheckBox_CheckedChanged;
            clickMouseCheckBox.CheckedChanged += clickMouseCheckBox_CheckedChanged;
            autoPauseCheckBox.CheckedChanged += autoPauseCheckBox_CheckedChanged;
            _mouseTimer.Interval = 1000;
            _mouseTimer.Tick += _mouseTimer_Tick;
            _resumeTimer.Interval = 1000;
            _resumeTimer.Tick += _resumeTimer_Tick;
            traceButton.Click += traceButton_Click;
            refreshButton.Click += refreshButton_Click;
            SetButtonTag(ref traceButton, GetButtonText(ref traceButton));
        }
        private void hotkeyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHookKeyStatus();
        }

        private void hotkeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            hotkeyComboBox.Enabled = hotkeyCheckBox.Checked;
            UpdateHookKeyStatus();
        }

        private bool IsBlackoutActive(TimeSpan time)
        {
            return false;
        }

        private void GetNextBlackoutStatusChangeTime(out TimeSpan startTime, out TimeSpan endTime)
        {
            startTime = new TimeSpan();
            endTime = new TimeSpan();
        }


        private void refreshButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ListOpenWindows);
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void paypalPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(PayPalAddress);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void paypalPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Default;
            }
        }

        private void paypalPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void contactPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(ContactAddress);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void contactPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Default;
            }
        }

        private void contactPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void UpdateHookKeyStatus()
        {
            var key = Keys.None;
            bool enabled = GetCheckBoxChecked(ref hotkeyCheckBox);
            object hookKey = GetComboBoxSelectedItem(ref hotkeyComboBox);

            if (!String.IsNullOrEmpty(hookKey?.ToString()))
            {
                key = (Keys)Enum.Parse(typeof(Keys), hookKey.ToString(), true);
            }

            var eventArgs = new HookKeyStatusChangedEventArgs(enabled, key);
            OnHookKeyStatusChanged(this, eventArgs);
        }

        private void helpPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(HelpAddress);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void helpPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Default;
            }
        }

        private void helpPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (Cursor != Cursors.WaitCursor)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void appActivateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            processComboBox.Enabled = appActivateCheckBox.Checked;
            refreshButton.Enabled = appActivateCheckBox.Checked;
        }

        private void ListOpenWindows(object stateInfo)
        {
            try
            {
                ClearComboBoxItems(ref processComboBox);
                var tag = GetComboBoxTag(ref processComboBox);

                foreach (var p in Process.GetProcesses())
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(p.MainWindowTitle) && !processComboBox.Items.Contains(p.MainWindowTitle))
                        {
                            //Debug.WriteLine(p.MainWindowTitle);
                            AddComboBoxItem(ref processComboBox, p.MainWindowTitle, ((tag != null) && tag.ToString().Equals(p.MainWindowTitle)));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void MouseForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = !minimiseToSystemTrayCheckBox.Checked;
            }
            else
            {
                Refresh();
            }
        }

        private void launchAtLogonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (launchAtLogonCheckBox.Checked)
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key?.SetValue("车辆保险", Application.ExecutablePath);
            }
            else
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key?.DeleteValue("车辆保险");
            }
        }

        private void MouseForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(StaticCode.WorkingDirectory))
            {
                Directory.CreateDirectory(StaticCode.WorkingDirectory);
            }

            if (!Directory.Exists(_moveMouseTempDirectory))
            {
                Directory.CreateDirectory(_moveMouseTempDirectory);
            }

            ThreadPool.QueueUserWorkItem(ListOpenWindows);
            ThreadPool.QueueUserWorkItem(UpdateCelebrityMiceList);
            UpdateHookKeyStatus();

            if (startOnLaunchCheckBox.Checked && !_suppressAutoStart)
            {
                actionButton.PerformClick();
            }

            #region Loop for testing blackout schedules

            //for (int i = 0; i < Convert.ToInt32(new TimeSpan(24, 0, 0).TotalSeconds); i++)
            //{
            //    var ts = new TimeSpan(0, 0, i);
            //    Debug.WriteLine(String.Format("IsBlackoutActive({0} = {1}", ts, IsBlackoutActive(ts)));
            //}

            #endregion
        }

        public void StartStopToggle()
        {
            actionButton.PerformClick();
        }

        private void UpdateCelebrityMiceList(object stateInfo)
        {
            try
            {
                _celebrityMice = new List<CelebrityMouse>();
                var miceXmlDoc = new XmlDocument();
                miceXmlDoc.Load(String.Format("{0}{1}", MiceResourceUrlPrefix, MiceXmlName));
                var mouseNodes = miceXmlDoc.SelectNodes("mice/mouse");

                if ((mouseNodes != null) && (mouseNodes.Count > 0))
                {
                    foreach (XmlNode mouseNode in mouseNodes)
                    {
                        var nameNode = mouseNode.SelectSingleNode("name");
                        var imageNameNode = mouseNode.SelectSingleNode("image_name");
                        var toolTipNode = mouseNode.SelectSingleNode("tool_tip");

                        if ((nameNode != null) && (imageNameNode != null) && (toolTipNode != null))
                        {
                            _celebrityMice.Add(new CelebrityMouse { Name = nameNode.InnerText, ImageName = imageNameNode.InnerText, ToolTip = toolTipNode.InnerText });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void traceButton_Click(object sender, EventArgs e)
        {
            _traceTimeComplete = DateTime.Now.AddSeconds(TraceSeconds);

            if ((_traceMouseThread == null) || (_traceMouseThread.ThreadState != System.Threading.ThreadState.Running))
            {
                _traceMouseThread = new Thread(TraceMouse);
                _traceMouseThread.Start();
            }
        }

        private void TraceMouse()
        {
            do
            {
                SetNumericUpDownValue(ref xNumericUpDown, Cursor.Position.X);
                SetNumericUpDownValue(ref yNumericUpDown, Cursor.Position.Y);
                SetButtonText(ref traceButton, String.Format("{0}", _traceTimeComplete.Subtract(DateTime.Now).TotalSeconds.ToString("0.0")));
                Thread.Sleep(100);
            } while (_traceTimeComplete > DateTime.Now);

            SetButtonText(ref traceButton, Convert.ToString(GetButtonTag(ref traceButton)));
        }

        private void startPositionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            xNumericUpDown.Enabled = staticPositionCheckBox.Checked;
            yNumericUpDown.Enabled = staticPositionCheckBox.Checked;
            traceButton.Enabled = staticPositionCheckBox.Checked;
        }

        private void resumeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            resumeNumericUpDown.Enabled = resumeCheckBox.Checked;
        }

        private void _resumeTimer_Tick(object sender, EventArgs e)
        {
            //Debug.WriteLine("_resumeTimer_Tick");
            //Debug.WriteLine(String.Format("GetLastInputTime() = {0}", GetLastInputTime()));
            //todo Something is happening after 4 seconds

            if (GetCheckBoxChecked(ref resumeCheckBox) && (GetLastInputTime() >= resumeNumericUpDown.Value))
            {
                ButtonPerformClick(ref actionButton);
            }
        }

        private void autoPauseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _startingMousePoint = Cursor.Position;
        }

        private void clickMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (!AtLeastOneActionIsEnabled())
            //{
            //    clickMouseCheckBox.Checked = true;
            //}

            DetermineActionsTabControlState();
        }

        private void moveMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (!AtLeastOneActionIsEnabled())
            //{
            //    moveMouseCheckBox.Checked = true;
            //}

            DetermineActionsTabControlState();
        }

        private void keystrokeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (!AtLeastOneActionIsEnabled())
            //{
            //    keystrokeCheckBox.Checked = true;
            //}

            DetermineActionsTabControlState();
        }

        //private bool AtLeastOneActionIsEnabled()
        //{
        //    return (moveMouseCheckBox.Checked || clickMouseCheckBox.Checked || keystrokeCheckBox.Checked);
        //}

        private void DetermineActionsTabControlState()
        {
            stealthCheckBox.Enabled = moveMouseCheckBox.Checked;
            staticPositionCheckBox.Enabled = (clickMouseCheckBox.Checked | moveMouseCheckBox.Checked);
            xNumericUpDown.Enabled = (staticPositionCheckBox.Enabled & staticPositionCheckBox.Checked);
            yNumericUpDown.Enabled = (staticPositionCheckBox.Enabled & staticPositionCheckBox.Checked);
            traceButton.Enabled = (staticPositionCheckBox.Enabled & staticPositionCheckBox.Checked);
            keystrokeComboBox.Enabled = keystrokeCheckBox.Checked;

            if (keystrokeComboBox.SelectedIndex.Equals(-1))
            {
                keystrokeComboBox.SelectedIndex = 0;
            }
        }

        private void MouseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnMoveMouseStopped();
            _mouseTimer.Stop();
            _resumeTimer.Stop();
            SaveSettings();
        }

        private void UpdateCountdownProgressBar(ref ProgressBar pb, int delay, int elapsed)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateCountdownProgressBarDelegate(UpdateCountdownProgressBar), pb, delay, elapsed);
            }
            else
            {
                pb.Minimum = 0;
                pb.Maximum = delay;

                if (elapsed < delay)
                {
                    pb.Value = delay - elapsed;
                }
                else
                {
                    pb.Value = 0;
                }
            }
        }

        private void ButtonPerformClick(ref Button b)
        {
            if (InvokeRequired)
            {
                Invoke(new ButtonPerformClickDelegate(ButtonPerformClick), b);
            }
            else
            {
                b.PerformClick();
            }
        }

        private object GetComboBoxSelectedItem(ref ComboBox cb)
        {
            if (InvokeRequired)
            {
                return Invoke(new GetComboBoxSelectedItemDelegate(GetComboBoxSelectedItem), cb);
            }

            return cb.SelectedItem;
        }

        private int GetComboBoxSelectedIndex(ref ComboBox cb)
        {
            if (InvokeRequired)
            {
                return (int)Invoke(new GetComboBoxSelectedIndexDelegate(GetComboBoxSelectedIndex), cb);
            }

            return cb.SelectedIndex;
        }

        private void SetNumericUpDownValue(ref NumericUpDown nud, int value)
        {
            if (InvokeRequired)
            {
                Invoke(new SetNumericUpDownValueDelegate(SetNumericUpDownValue), nud, value);
            }
            else
            {
                nud.Value = value;
            }
        }

        private void SetButtonText(ref Button b, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new SetButtonTextDelegate(SetButtonText), b, text);
            }
            else
            {
                b.Text = text;
            }
        }

        private void SetButtonTag(ref Button b, object o)
        {
            if (InvokeRequired)
            {
                Invoke(new SetButtonTagDelegate(SetButtonTag), b, o);
            }
            else
            {
                b.Tag = o;
            }
        }

        private object GetButtonTag(ref Button b)
        {
            if (InvokeRequired)
            {
                return Invoke(new GetButtonTagDelegate(GetButtonTag), b);
            }

            return b.Tag;
        }

        private string GetButtonText(ref Button b)
        {
            if (InvokeRequired)
            {
                return Convert.ToString(Invoke(new GetButtonTextDelegate(GetButtonText), b));
            }

            return b.Text;
        }

        private bool GetCheckBoxChecked(ref CheckBox cb)
        {
            if (InvokeRequired)
            {
                return Convert.ToBoolean(Invoke(new GetCheckBoxCheckedDelegate(GetCheckBoxChecked), cb));
            }

            return cb.Checked;
        }

        private void AddComboBoxItem(ref ComboBox cb, string item, bool selected)
        {
            if (InvokeRequired)
            {
                Invoke(new AddComboBoxItemDelegate(AddComboBoxItem), cb, item, selected);
            }
            else
            {
                int index = cb.Items.Add(item);

                if (selected)
                {
                    cb.SelectedIndex = index;
                }
            }
        }

        private object GetComboBoxTag(ref ComboBox cb)
        {
            if (InvokeRequired)
            {
                return Invoke(new GetComboBoxTagDelegate(GetComboBoxTag), cb);
            }

            return cb.Tag;
        }

        private void ClearComboBoxItems(ref ComboBox cb)
        {
            if (InvokeRequired)
            {
                Invoke(new ClearComboBoxItemsDelegate(ClearComboBoxItems), cb);
            }
            else
            {
                cb.Items.Clear();
            }
        }

        private bool IsWindowMinimised(IntPtr handle)
        {
            if (InvokeRequired)
            {
                return Convert.ToBoolean(Invoke(new IsWindowMinimisedDelegate(IsWindowMinimised), handle));
            }

            var placement = GetPlacement(handle);
            return (placement.showCmd == ShowWindowCommands.ShowMinimized);
        }

        private void ShowCelebrityMouse(CelebrityMouse cb)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowCelebrityMouseDelegate(ShowCelebrityMouse), cb);
            }
            else
            {
                if (cb != null)
                {
                    string imageLocalPath = Path.Combine(_moveMouseTempDirectory, cb.ImageName);

                    if (File.Exists(imageLocalPath))
                    {
                    }
                }
            }
        }

        private WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            var placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        private int GetLastInputTime()
        {
            int idleTime = 0;
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;
            int envTicks = Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                int lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            return (idleTime > 0) ? (idleTime / 1000) : 0;
        }

        private int GetScreenSaverTimeout()
        {
            const int SPI_GETSCREENSAVERTIMEOUT = 14;
            int value = 0;
            SystemParametersInfo(SPI_GETSCREENSAVERTIMEOUT, 0, ref value, 0);
            return value;
        }

        private void MoveMousePointer(Point point)
        {
            var mi = new MOUSEINPUT
            {
                dx = point.X,
                dy = point.Y,
                mouseData = 0,
                time = 0,
                dwFlags = MouseEventFlags.MOVE,
                dwExtraInfo = UIntPtr.Zero
            };
            var input = new INPUT
            {
                mi = mi,
                type = Convert.ToInt32(Win32Consts.INPUT_MOUSE)
            };
            SendInput(1, ref input, Marshal.SizeOf(input));
        }

        private void WriteSingleNodeInnerText(ref XmlDocument xmlDoc, string nodePath, string text)
        {
            var node = xmlDoc?.SelectSingleNode(nodePath);

            if (node != null)
            {
                node.InnerText = text;
            }
        }

        private string ReadSingleNodeInnerTextAsString(ref XmlDocument xmlDoc, string nodePath)
        {
            var node = xmlDoc?.SelectSingleNode(nodePath);

            if (node != null)
            {
                return node.InnerText;
            }

            return String.Empty;
        }

        private bool ReadSingleNodeInnerTextAsBoolean(ref XmlDocument xmlDoc, string nodePath)
        {
            var node = xmlDoc?.SelectSingleNode(nodePath);

            if (node != null)
            {
                bool b;

                if (Boolean.TryParse(node.InnerText, out b))
                {
                    return b;
                }
            }

            return false;
        }

        private decimal ReadSingleNodeInnerTextAsDecimal(ref XmlDocument xmlDoc, string nodePath)
        {
            var node = xmlDoc?.SelectSingleNode(nodePath);

            if (node != null)
            {
                decimal d;

                if (Decimal.TryParse(node.InnerText, out d))
                {
                    return d;
                }
            }

            return 0;
        }

        private DateTime ReadSingleNodeInnerTextAsDateTime(ref XmlDocument xmlDoc, string nodePath)
        {
            var node = xmlDoc?.SelectSingleNode(nodePath);

            if (node != null)
            {
                DateTime dt;

                if (DateTime.TryParse(node.InnerText, out dt))
                {
                    return dt;
                }
            }

            return new DateTime();
        }

        //private Int32 ReadSingleNodeInnerTextAsInt32(ref XmlDocument xmlDoc, string nodePath)
        //{
        //    var node = xmlDoc?.SelectSingleNode(nodePath);

        //    if (node != null)
        //    {
        //        Int32 i;

        //        if (Int32.TryParse(node.InnerText, out i))
        //        {
        //            return i;
        //        }
        //    }

        //    return 0;
        //}

        private string ReadSingleNodeInnerTextAsString(XmlNode parentNode, string nodePath)
        {
            var node = parentNode?.SelectSingleNode(nodePath);

            if (node != null)
            {
                return node.InnerText;
            }

            return String.Empty;
        }

        //private bool ReadSingleNodeInnerTextAsBoolean(XmlNode parentNode, string nodePath)
        //{
        //    var node = parentNode?.SelectSingleNode(nodePath);

        //    if (node != null)
        //    {
        //        bool b;

        //        if (Boolean.TryParse(node.InnerText, out b))
        //        {
        //            return b;
        //        }
        //    }

        //    return false;
        //}

        //private decimal ReadSingleNodeInnerTextAsDecimal(XmlNode parentNode, string nodePath)
        //{
        //    var node = parentNode?.SelectSingleNode(nodePath);

        //    if (node != null)
        //    {
        //        decimal d;

        //        if (Decimal.TryParse(node.InnerText, out d))
        //        {
        //            return d;
        //        }
        //    }

        //    return 0;
        //}

        private DateTime ReadSingleNodeInnerTextAsDateTime(XmlNode parentNode, string nodePath)
        {
            var node = parentNode?.SelectSingleNode(nodePath);

            if (node != null)
            {
                DateTime dt;

                if (DateTime.TryParse(node.InnerText, out dt))
                {
                    return dt;
                }
            }

            return new DateTime();
        }

        private Int32 ReadSingleNodeInnerTextAsInt32(XmlNode parentNode, string nodePath)
        {
            var node = parentNode?.SelectSingleNode(nodePath);

            if (node != null)
            {
                Int32 i;

                if (Int32.TryParse(node.InnerText, out i))
                {
                    return i;
                }
            }

            return 0;
        }

        private void ReadSettings()
        {
            if (InvokeRequired)
            {
                Invoke(new ZeroParameterDelegate(ReadSettings));
            }
            else
            {
                try
                {
                    if (File.Exists(Path.Combine(StaticCode.WorkingDirectory, MoveMouseXmlName)))
                    {
                        var settingsXmlDoc = new XmlDocument();
                        settingsXmlDoc.Load(Path.Combine(StaticCode.WorkingDirectory, MoveMouseXmlName));
                        delayNumericUpDown.Value = ReadSingleNodeInnerTextAsDecimal(ref settingsXmlDoc, "settings/second_delay");
                        moveMouseCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/move_mouse_pointer");
                        stealthCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/stealth_mode");
                        staticPositionCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/enable_static_position");
                        xNumericUpDown.Value = ReadSingleNodeInnerTextAsDecimal(ref settingsXmlDoc, "settings/x_static_position");
                        yNumericUpDown.Value = ReadSingleNodeInnerTextAsDecimal(ref settingsXmlDoc, "settings/y_static_position");
                        clickMouseCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/click_left_mouse_button");
                        keystrokeCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/send_keystroke");
                        keystrokeComboBox.SelectedItem = ReadSingleNodeInnerTextAsString(ref settingsXmlDoc, "settings/keystroke");
                        autoPauseCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/pause_when_mouse_moved");
                        resumeCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/automatically_resume");
                        resumeNumericUpDown.Value = ReadSingleNodeInnerTextAsDecimal(ref settingsXmlDoc, "settings/resume_seconds");
                        disableOnBatteryCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/disable_on_battery");
                        hotkeyCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/enable_hotkey");
                        hotkeyComboBox.SelectedItem = ReadSingleNodeInnerTextAsString(ref settingsXmlDoc, "settings/hotkey");
                        startOnLaunchCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/automatically_start_on_launch");
                        launchAtLogonCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/automatically_launch_on_logon");
                        minimiseOnPauseCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/minimise_on_pause");
                        minimiseOnStartCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/minimise_on_start");
                        minimiseToSystemTrayCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/minimise_to_system_tray");
                        appActivateCheckBox.Checked = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/activate_application");

                        if (!String.IsNullOrEmpty(ReadSingleNodeInnerTextAsString(ref settingsXmlDoc, "settings/activate_application_title")))
                        {
                            processComboBox.Tag = ReadSingleNodeInnerTextAsString(ref settingsXmlDoc, "settings/activate_application_title");
                        }

                        _lastUpdateCheck = ReadSingleNodeInnerTextAsDateTime(ref settingsXmlDoc, "settings/last_update_check");
                        MinimiseToSystemTrayWarningShown = ReadSingleNodeInnerTextAsBoolean(ref settingsXmlDoc, "settings/system_tray_warning_shown");

                        ReadLegacySettings(ref settingsXmlDoc);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void ReadLegacySettings(ref XmlDocument xmlDoc)
        {
            try
            {
                if (ReadSingleNodeInnerTextAsBoolean(ref xmlDoc, "settings/blackout_schedule_enabled"))
                {
                    string start = null;
                    string end = null;
                    TimeSpan startTs;
                    TimeSpan endTs;

                    switch (ReadSingleNodeInnerTextAsString(ref xmlDoc, "settings/blackout_schedule_scope"))
                    {
                        case "outside":
                            start = ReadSingleNodeInnerTextAsString(ref xmlDoc, "settings/blackout_schedule_end");
                            end = ReadSingleNodeInnerTextAsString(ref xmlDoc, "settings/blackout_schedule_start");
                            break;
                        case "inside":
                            start = ReadSingleNodeInnerTextAsString(ref xmlDoc, "settings/blackout_schedule_start");
                            end = ReadSingleNodeInnerTextAsString(ref xmlDoc, "settings/blackout_schedule_end");
                            break;
                    }

                    if (!String.IsNullOrEmpty(start) && !String.IsNullOrEmpty(end) && TimeSpan.TryParse(start, out startTs) && TimeSpan.TryParse(end, out endTs))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SaveSettings()
        {
            try
            {
                var settingsXmlDoc = new XmlDocument();
                settingsXmlDoc.LoadXml("<settings><second_delay /><move_mouse_pointer /><stealth_mode /><enable_static_position /><x_static_position /><y_static_position /><click_left_mouse_button /><send_keystroke /><keystroke /><pause_when_mouse_moved /><automatically_resume /><resume_seconds /><disable_on_battery /><enable_hotkey /><hotkey /><automatically_start_on_launch /><automatically_launch_on_logon /><minimise_on_pause /><minimise_on_start /><minimise_to_system_tray /><activate_application /><activate_application_title /><last_update_check /><system_tray_warning_shown /><execute_start_script /><execute_interval_script /><execute_pause_script /><show_script_execution /><script_language /><script_editor /><schedules /><blackouts /></settings>");
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/second_delay", Convert.ToDecimal(delayNumericUpDown.Value).ToString(CultureInfo.InvariantCulture));
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/move_mouse_pointer", moveMouseCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/stealth_mode", stealthCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/enable_static_position", staticPositionCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/x_static_position", Convert.ToDecimal(xNumericUpDown.Value).ToString(CultureInfo.InvariantCulture));
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/y_static_position", Convert.ToDecimal(yNumericUpDown.Value).ToString(CultureInfo.InvariantCulture));
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/click_left_mouse_button", clickMouseCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/send_keystroke", keystrokeCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/keystroke", keystrokeComboBox.SelectedItem?.ToString() ?? String.Empty);
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/pause_when_mouse_moved", autoPauseCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/automatically_resume", resumeCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/resume_seconds", Convert.ToDecimal(resumeNumericUpDown.Value).ToString(CultureInfo.InvariantCulture));
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/disable_on_battery", disableOnBatteryCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/enable_hotkey", hotkeyCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/hotkey", hotkeyComboBox.SelectedItem?.ToString() ?? String.Empty);
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/automatically_start_on_launch", startOnLaunchCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/automatically_launch_on_logon", launchAtLogonCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/minimise_on_pause", minimiseOnPauseCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/minimise_on_start", minimiseOnStartCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/minimise_to_system_tray", minimiseToSystemTrayCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/activate_application", appActivateCheckBox.Checked.ToString());
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/activate_application_title", processComboBox.SelectedItem?.ToString() ?? String.Empty);
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/last_update_check", _lastUpdateCheck.ToString("yyyy-MMM-dd HH:mm:ss"));
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/system_tray_warning_shown", "True");
                WriteSingleNodeInnerText(ref settingsXmlDoc, "settings/script_editor", _scriptEditor);
                var schedulesNode = settingsXmlDoc.SelectSingleNode("settings/schedules");
                var blackoutsNode = settingsXmlDoc.SelectSingleNode("settings/blackouts");

                settingsXmlDoc.Save(Path.Combine(StaticCode.WorkingDirectory, MoveMouseXmlName));
                processComboBox.Tag = processComboBox.Text;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            switch (actionButton.Text)
            {
                case "pause":
                    OnMoveMouseStopped();
                    _mouseTimer.Stop();
                    _resumeTimer.Start();
                    _easterEggActive = false;
                    ResetMousePicture();
                    actionButton.Text = "launch";
                    countdownProgressBar.Value = 0;
                    optionsTabControl.Enabled = true;
                    Opacity = 1.0;

                    if (minimiseOnPauseCheckBox.Checked)
                    {
                        WindowState = FormWindowState.Minimized;
                    }

                    ReadSettings();


                    break;
                default:
                    _mouseTimerTicks = 0;
                    _mmStartTime = DateTime.Now;
                    _blackoutStatus = BlackoutStatusChangedEventArgs.BlackoutStatus.Inactive;
                    _powerLineStatus = PowerLineStatusChangedEventArgs.PowerLineStatus.Online;
                    _easterEggActive = new Random().Next(1, 100).Equals(31);
                    ResetMousePicture();

                    if (!IsBlackoutActive(DateTime.Now.TimeOfDay))
                    {
                        MoveMouseToStaticPosition();
                        ActivateApplication();
                        WindowState = minimiseOnStartCheckBox.Checked ? FormWindowState.Minimized : FormWindowState.Normal;
                    }

                    _resumeTimer.Stop();
                    OnMoveMouseStarted();
                    _mouseTimer.Start();
                    actionButton.Text = "pause";
                    optionsTabControl.Enabled = false;
                    Opacity = 0.95;
                    SaveSettings();
                    break;
            }
        }

        private void _mouseTimer_Tick(object sender, EventArgs e)
        {
            //Debug.WriteLine("_mouseTimer_Tick");

            if (!IsBlackoutActive(DateTime.Now.TimeOfDay) && (!GetCheckBoxChecked(ref disableOnBatteryCheckBox) || !IsRunningOnBattery()))
            {
                UpdateCountdownProgressBar(ref countdownProgressBar, Convert.ToInt32(delayNumericUpDown.Value), _mouseTimerTicks);
                _mouseTimerTicks++;
                AutoPauseIfMouseHasMoved();

                if (_blackoutStatus == BlackoutStatusChangedEventArgs.BlackoutStatus.Active)
                {
                    _blackoutStatus = BlackoutStatusChangedEventArgs.BlackoutStatus.Inactive;
                    TimeSpan startTime;
                    TimeSpan endTime;
                    GetNextBlackoutStatusChangeTime(out startTime, out endTime);
                    OnBlackoutStatusChanged(this, new BlackoutStatusChangedEventArgs(_blackoutStatus, startTime, endTime));
                    OnMoveMouseStarted();
                }

                if (_powerLineStatus == PowerLineStatusChangedEventArgs.PowerLineStatus.Offline)
                {
                    _powerLineStatus = PowerLineStatusChangedEventArgs.PowerLineStatus.Online;
                    OnPowerLineStatusChanged(this, new PowerLineStatusChangedEventArgs(_powerLineStatus));
                    OnMoveMouseStarted();
                }

                if (_mouseTimerTicks > Convert.ToInt32(delayNumericUpDown.Value))
                {
                    ReadSettings();
                    ShowCelebrityMouse(_easterEggActive);
                    SendKeystroke();
                    ClickMouse();
                    MoveMouse();
                    _mouseTimerTicks = 0;
                }
            }
            else
            {
                if (IsBlackoutActive(DateTime.Now.TimeOfDay) && (_blackoutStatus == BlackoutStatusChangedEventArgs.BlackoutStatus.Inactive))
                {
                    _blackoutStatus = BlackoutStatusChangedEventArgs.BlackoutStatus.Active;
                    TimeSpan startTime;
                    TimeSpan endTime;
                    GetNextBlackoutStatusChangeTime(out startTime, out endTime);
                    OnBlackoutStatusChanged(this, new BlackoutStatusChangedEventArgs(_blackoutStatus, startTime, endTime));
                    OnMoveMousePaused();
                }

                if (GetCheckBoxChecked(ref disableOnBatteryCheckBox) && IsRunningOnBattery() && (_powerLineStatus == PowerLineStatusChangedEventArgs.PowerLineStatus.Online))
                {
                    _powerLineStatus = PowerLineStatusChangedEventArgs.PowerLineStatus.Offline;
                    OnPowerLineStatusChanged(this, new PowerLineStatusChangedEventArgs(_powerLineStatus));
                    OnMoveMousePaused();
                }
            }
        }

        private void ClickMouse()
        {
            if (GetCheckBoxChecked(ref clickMouseCheckBox))
            {
                mouse_event((int)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
                mouse_event((int)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
            }
        }

        private void MoveMouse()
        {
            if (GetCheckBoxChecked(ref moveMouseCheckBox))
            {
                if (!GetCheckBoxChecked(ref stealthCheckBox))
                {
                    const int mouseMoveLoopSleep = 1;
                    const int mouseSpeed = 1;
                    const int moveSquareSize = 10;
                    var cursorStartPosition = Cursor.Position;

                    for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                    {
                        MoveMousePointer(new Point(1, 0));
                        Thread.Sleep(mouseMoveLoopSleep);
                    }

                    for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                    {
                        MoveMousePointer(new Point(0, 1));
                        Thread.Sleep(mouseMoveLoopSleep);
                    }

                    for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                    {
                        MoveMousePointer(new Point(-1, 0));
                        Thread.Sleep(mouseMoveLoopSleep);
                    }

                    for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                    {
                        MoveMousePointer(new Point(0, -1));
                        Thread.Sleep(mouseMoveLoopSleep);
                    }

                    Cursor.Position = cursorStartPosition;
                }
                else
                {
                    MoveMousePointer(new Point(0, 0));
                }
            }
        }

        private void MoveMouseToStaticPosition()
        {
            if (GetCheckBoxChecked(ref staticPositionCheckBox))
            {
                Cursor.Position = new Point(Convert.ToInt32(xNumericUpDown.Value), Convert.ToInt32(yNumericUpDown.Value));
            }
        }

        private void AutoPauseIfMouseHasMoved()
        {
            if (GetCheckBoxChecked(ref autoPauseCheckBox) && (_mmStartTime.Add(_waitUntilAutoMoveDetect) < DateTime.Now) && (_startingMousePoint != Cursor.Position))
            {
                ButtonPerformClick(ref actionButton);
            }
            else
            {
                _startingMousePoint = Cursor.Position;
            }
        }

        private void SendKeystroke()
        {
            if (GetCheckBoxChecked(ref keystrokeCheckBox) && (GetComboBoxSelectedIndex(ref keystrokeComboBox) > -1))
            {
                SendKeys.SendWait(GetComboBoxSelectedItem(ref keystrokeComboBox).ToString());
            }
        }

        private void ActivateApplication()
        {
            if (GetCheckBoxChecked(ref appActivateCheckBox) && (GetComboBoxSelectedIndex(ref processComboBox) > -1))
            {
                try
                {
                    IntPtr handle = FindWindow(null, GetComboBoxSelectedItem(ref processComboBox).ToString());

                    if (handle != IntPtr.Zero)
                    {
                        if (IsWindowMinimised(handle))
                        {
                            ShowWindow(handle, ShowWindowCommands.Restore);
                        }

                        Interaction.AppActivate(GetComboBoxSelectedItem(ref processComboBox).ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void ShowCelebrityMouse(bool ignoreEasterEggState)
        {
            try
            {
                if ((ignoreEasterEggState || _easterEggActive) && (_celebrityMice != null) && (_celebrityMice.Count > 0) && Directory.Exists(_moveMouseTempDirectory))
                {
                    ThreadPool.QueueUserWorkItem(ShowCelebrityMouseThread);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ShowCelebrityMouseThread(object stateInfo)
        {
            try
            {
                var mouse = _celebrityMice[new Random().Next(0, (_celebrityMice.Count - 1))];
                string imageLocalPath = Path.Combine(_moveMouseTempDirectory, mouse.ImageName);

                if (!File.Exists(imageLocalPath))
                {
                    string imageUrl = String.Format("{0}{1}", MiceResourceUrlPrefix, mouse.ImageName);
                    //Debug.WriteLine(imageUrl);
                    var wc = new WebClient();
                    wc.DownloadFile(imageUrl, imageLocalPath);
                }

                ShowCelebrityMouse(mouse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ResetMousePicture()
        {
            if (InvokeRequired)
            {
                Invoke(new ZeroParameterDelegate(ResetMousePicture), new object[] { });
            }
            else
            {
            }
        }

        private bool IsRunningOnBattery()
        {
            return SystemInformation.PowerStatus.PowerLineStatus.Equals(PowerLineStatus.Offline);
        }

        protected void OnBlackoutStatusChanged(object sender, BlackoutStatusChangedEventArgs e)
        {
            BlackoutStatusChanged?.Invoke(sender, e);
        }

        protected void OnNewVersionAvailable(object sender, NewVersionAvailableEventArgs e)
        {
            NewVersionAvailable?.Invoke(sender, e);
        }

        protected void OnScheduleArrived(object sender, ScheduleArrivedEventArgs e)
        {
            ScheduleArrived?.Invoke(sender, e);
        }

        protected void OnPowerLineStatusChanged(object sender, PowerLineStatusChangedEventArgs e)
        {
            PowerLineStatusChanged?.Invoke(sender, e);
        }


        protected void OnHookKeyStatusChanged(object sender, HookKeyStatusChangedEventArgs e)
        {
            HookKeyStatusChanged?.Invoke(sender, e);
        }

        protected void OnMoveMouseStarted()
        {
            MoveMouseStarted?.Invoke();
        }

        protected void OnMoveMousePaused()
        {
            MoveMousePaused?.Invoke();
        }

        protected void OnMoveMouseStopped()
        {
            MoveMouseStopped?.Invoke();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Automation.BDaq;
using xlTools;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace xlDriverDIO
{
    public class PCI1762 : xlDriverDioBase
    {
        private static Object syscReadObject = new Object();
        private static Object syscWriteObject = new Object();
        private InstantDoCtrl m_do = null;
        private InstantDiCtrl m_di = null;
        public PCI1762()
        {
            //xlIni.INIGetStringValue(    
        }

        public String DeviceDescription = "DemoDevice,BID#0";
        private Boolean isDiInitialized = false;
        private Boolean isDoInitialized = false;
        
        #region 操作函数
        public override void Init()
        {
            //初始化窗体
            FilePath = Assembly.GetExecutingAssembly().Location;
            String path = Path.GetDirectoryName(FilePath);

            DeviceDescription = xlIni.INIGetStringValue(FilePath + ".ini", "PCI1762", "DeviceDescription", null);
            if (DeviceDescription == null)
            {
                DeviceDescription = "DemoDevice,BID#0";
                xlIni.INIWriteValue(FilePath + ".ini", "PCI1762", "DeviceDescription", DeviceDescription);
            }
            isDiInitialized = true;
            isDoInitialized = true;
            m_do = new InstantDoCtrl();
            m_di = new InstantDiCtrl();
            try
            {
                m_do.SelectedDevice = new DeviceInformation(DeviceDescription);
                m_di.SelectedDevice = new DeviceInformation(DeviceDescription);

                m_do.Write(0, 0);
                m_do.Write(1, 0);
            }
            catch (Exception)
            {
                throw;
            }

            isDoInitialized = m_do.Initialized;
            isDiInitialized = m_di.Initialized;
        }

        public override void Release()
        {
            m_do.Dispose();
            m_di.Dispose();
        }
        public override void Write(Int32 PORTx, Int32 Pin, Boolean isHigh)
        {
            lock (syscWriteObject)
            {
                if (!isDoInitialized) return;
                m_do.WriteBit(PORTx, Pin, Convert.ToByte(isHigh));
                byte data = 0;
                m_do.Read(0, out data);
            }
        }
        public override void Write(Int32 PORTx, Int32 portValue)
        {
            lock (syscWriteObject)
            {
                if (!isDoInitialized) return;
                m_do.Write(PORTx, (Byte)portValue);
            }
        }
        public override Int32 Read(Int32 PORTx)
        {
            lock (syscReadObject)
            {
                if (!isDoInitialized) return 0;

                try
                {
                    Byte data = 0;
                    m_di.Read(PORTx, out data);
                    return (Int32)data;
                }
                catch
                {
                    //ex.ToString();
                    return 0;
                }
            }
        }
        public override Boolean Read(Int32 PORTx, Int32 Pin)
        {
            lock (syscReadObject)
            {
                if (!isDoInitialized) return false;
                try
                {
                    Byte data = 0;
                    m_di.ReadBit(PORTx, Pin, out data);
                    return (data == 1);
                }
                catch
                {
                    return false;
                }
            }
        }
        public override void SetDir(Int32 PORTxDir, Int32 Output)
        {
            throw new NotImplementedException();    
        }
        public override void Reset(ushort val)
        {
            throw new NotImplementedException(); 
        }

        #endregion
    }
}

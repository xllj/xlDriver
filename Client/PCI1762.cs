using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Collections;
//using UniDAQ_Ns;
using System.Windows.Forms;
using Automation.BDaq;

namespace DIO
{
    public class PCI1762
    {
        private InstantDoCtrl m_do = null;
        private InstantDiCtrl m_di = null;
        public PCI1762()
        {
            
        }

        public String DeviceDescription = "DemoDevice, BID#0";
        #region 操作函数
        public void Init()
        {
            m_do = new InstantDoCtrl();
            m_di = new InstantDiCtrl();

            m_do.SelectedDevice = new DeviceInformation(DeviceDescription);
            m_di.SelectedDevice = new DeviceInformation(DeviceDescription);    
        }


        #endregion
    }
}

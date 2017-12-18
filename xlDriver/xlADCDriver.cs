using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace xlTools
{
    using P1602_Ns;

    public class xlADCDriver : P1602
    {
        //uint wAddrBase;
        ushort wTotalBoard;
        ushort wErrCode;

        private bool isInit = false;
        private Object thisLock = new Object();

        ~xlADCDriver()
        {
            //DeviceRelease();
        }

        //public CalibrationFactor[] m_cf = new CalibrationFactor[32];        //测量通道倍数

        public Int32 DeviceInit(UInt16 wBoardNo)
        {
            try
            {
                wErrCode = DriverInit(out wTotalBoard);
                if (wErrCode != NoError)
                {
                    return wErrCode;
                }

                wErrCode = ActiveBoard(wBoardNo);
                if (wErrCode != NoError)
                {
                    return wErrCode;
                }

                //InitCalibrationParameter();
                isInit = true;
                return 0;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void DeviceRelease()
        {
            lock (thisLock)
            {
                try
                {
                    DriverClose();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    throw;
                }
                
            }
        }

        //float tempValue = 0;
        public void DeviceReadValue(UInt16 wAdChannel, out float AdVal)
        {
            try
            {
                if (!isInit)
                {
                    AdVal = (float)0.0;
                    return;
                }

                lock (thisLock)
                {
                    wErrCode = SetChannelConfig(wAdChannel, 0); //Set Channel Config Code
                    wErrCode = DelayUs(23);
                    wErrCode = AdPolling(out AdVal);
                    //tempValue = tempValue + (float)0.001;
                    //AdVal = tempValue;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        //滤波读取
        private const UInt16 ADC_SAMPLE_TIM = 1000;
        public void DeviceReadValueFilter(UInt16 wAdChannel, out float AdVal)
        {
            try
            {
                if (!isInit)
                {
                    AdVal = (float)0.0;
                    return;
                }

                float[] adVals = new float[ADC_SAMPLE_TIM];
                float sum = 0;
                lock (thisLock)
                {
                    if (wAdChannel > 31)
                    {
                        wAdChannel = 31;
                    }
                    wErrCode = SetChannelConfig(wAdChannel, 0); //Set Channel Config Code
                    wErrCode = DelayUs(23);
                    wErrCode = AdsPacer(out adVals[0], ADC_SAMPLE_TIM, 80);

                    #region 滤波处理
                    for (int i = 0; i < ADC_SAMPLE_TIM; i++)
                    {
                        sum += adVals[i];
                    }
                    AdVal = sum / ADC_SAMPLE_TIM;
                    #endregion


                    //AdVal = AdVal * m_cf[wAdChannel].k + m_cf[wAdChannel].b;
                    //if (AdVal < 0.0)
                    //{
                    //    AdVal = (float)0.0;
                    //}
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void DeviceReadValue(UInt16 wAdChannel, out float fAdVal, ushort wNum)
        {
            try
            {
                if (!isInit)
                {
                    fAdVal = (float)0.0;
                    return;
                }

                lock (thisLock)
                {
                    wErrCode = SetChannelConfig(wAdChannel, 0); //Set Channel Config Code
                    wErrCode = DelayUs(23);
                    wErrCode = AdsPolling(out fAdVal, wNum);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public void DeviceReadValue(UInt16 wAdChannel, out float fAdVal, ushort wNum, ushort wSample)
        {
            try
            {
             if (!isInit)
            {
                fAdVal = (float)0.0;
                return;
            }

            lock (thisLock)
            {
                wErrCode = SetChannelConfig(wAdChannel, 0); //Set Channel Config Code
                wErrCode = DelayUs(23);
                wErrCode = AdsPacer(out fAdVal, wNum, wSample);
            }
        
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

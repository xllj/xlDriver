using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PIODIO_Ns;

namespace xlTools
{
    public class xlDIODriver : PIODIO
    {
        private Object thisLock = new Object();

        private bool isInit = false;
        uint wAddrBase;
        ushort wIrqNo, wSubVendor, wSubDevice, wSubAux, wSlotBus, wSlotDevice, wInitialCode, wBoards;
        //uint InVal0, InVal1, InVal2, InCon, OutCon;

        public const ushort Pin_0 = 0x0001;
        public const ushort Pin_1 = 0x0002;
        public const ushort Pin_2 = 0x0004;
        public const ushort Pin_3 = 0x0008;
        public const ushort Pin_4 = 0x0010;
        public const ushort Pin_5 = 0x0020;
        public const ushort Pin_6 = 0x0040;
        public const ushort Pin_7 = 0x0080;
        public const ushort Pin_8 = 0x0100;
        public const ushort Pin_9 = 0x0200;
        public const ushort Pin_10 = 0x0400;
        public const ushort Pin_11 = 0x0800;
        public const ushort Pin_12 = 0x1000;
        public const ushort Pin_13 = 0x2000;
        public const ushort Pin_14 = 0x4000;
        public const ushort Pin_15 = 0x8000;

        public const ushort Port0 = 0x00c0;
        public const ushort Port1 = 0x00c4;
        public const ushort Port2 = 0x00c8;
        public const ushort PortDir0_2 = 0x00cc;

        public const ushort Port3 = 0x00d0;
        public const ushort Port4 = 0x00d4;
        public const ushort Port5 = 0x00d8;
        public const ushort PortDir3_5 = 0x00dc;

        public const ushort Port6 = 0x00e0;
        public const ushort Port7 = 0x00e4;
        public const ushort Port8 = 0x00e8;
        public const ushort PortDir6_8 = 0x00ec;

        public const ushort Port9 = 0x00f0;
        public const ushort Port10 = 0x00f4;
        public const ushort Port11 = 0x00f8;
        public const ushort PortDir9_11 = 0x00fc;


        public Int32 DeviceInit(ushort wBoardNo)
        {
            try
            {
                wInitialCode = DriverInit();

                if (wInitialCode != 0)
                {
                    return wInitialCode;
                }

                wInitialCode = SearchCard(out wBoards, PIOD_96);
                if (wInitialCode != 0)
                {
                    return wInitialCode;
                }

                wInitialCode = GetConfigAddressSpace(wBoardNo, out wAddrBase, out wIrqNo,
                out wSubVendor, out wSubDevice, out wSubAux, out wSlotBus, out wSlotDevice);
                if (wInitialCode != 0)
                {
                    return wInitialCode;
                }

                //ActiveBoard(wBoardNo);
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
            try
            {
                if (!isInit)
                {
                    return;
                }

                lock (thisLock)
                {
                    DriverClose();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void DeviceWrite(UInt16 PORTx, ushort Pin, bool state)
        {
            try
            {
                if (!isInit)
                {
                    return;
                }

                lock (thisLock)
                {
                    ushort val = InputByte(wAddrBase + PORTx);
                    if (state == true)
                    {
                        val = (ushort)(val | Pin);
                    }
                    else
                    {
                        val = (ushort)(val & ~Pin);
                    }

                    Debug.WriteLine(val.ToString());
                    OutputByte(wAddrBase + PORTx, val);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void DeviceWrite(UInt16 PORTx, UInt16 val)
        {
            try
            {
                if (!isInit)
                {
                    return;
                }

                lock (thisLock)
                {
                    Debug.WriteLine(val.ToString());
                    OutputByte(wAddrBase + PORTx, val);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public UInt16 DeviceRead(UInt16 PORTx)
        {
            try
            {
                ushort val = 0;
                if (!isInit)
                {
                    return val;
                }

                lock (thisLock)
                {
                    val = InputByte(wAddrBase + PORTx);

                    Debug.WriteLine(val.ToString());
                }
                return val;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

       
        public bool DeviceRead(UInt16 PORTx, ushort Pin)
        {
            try
            {
                if (!isInit)
                {
                    return false;
                }

                lock (thisLock)
                {
                    ushort val = InputByte(wAddrBase + PORTx);

                    Debug.WriteLine(val.ToString());

                    if ((val & Pin) == Pin)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //public UInt32 DeviceReadGroup0()
        //{
        //    UInt32 val = 0;
        //    return val;
        //}

        public void DeviceDir(UInt16 PORTxDir, ushort dir)
        {
            try
            {
                if (!isInit)
                {
                    return;
                }
                lock (thisLock)
                {
                    OutputByte(wAddrBase + PORTxDir, dir);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        /****************************************************
         * 
         * val : 0 设备复位
         *       1 设备正常
         */
        public void DeviceReset(ushort val)
        {
            try
            {
                if (!isInit)
                {
                    return;
                }
                lock (thisLock)
                {
                    OutputByte(wAddrBase, val);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        ~xlDIODriver()
        {
            lock (thisLock)
            {
                //DeviceRelease();
            }
        }
    }
}

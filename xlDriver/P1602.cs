using System;
using System.Collections.Generic;
using System.Text;

namespace P1602_Ns
{
    using System.Runtime.InteropServices ;
    public class P1602
    {
        public const uint NoError = 0;
        public const uint DriverHandleError = 1;
        public const uint DriverCallError = 2;
        public const uint AdControllerError = 3;
        public const uint M_FunExecError = 4;
        public const uint ConfigCodeError = 5;
        public const uint FrequencyComputeError = 6;
        public const uint HighAlarm = 7;
        public const uint LowAlarm = 8;
        public const uint AdPollingTimeOut = 9;
        public const uint AlarmTypeError = 10;
        public const uint FindBoardError = 11;
        public const uint AdChannelError = 12;
        public const uint DaChannelError = 13;
        public const uint InvalidateDelay = 14;
        public const uint DelayTimeOut = 15;
        public const uint InvalidateData = 16;
        public const uint FifoOverflow = 17;
        public const uint TimeOut = 18;
        public const uint ExceedBoardNumber = 19;
        public const uint NotFoundBoard = 20;
        public const uint OpenError = 21;
        public const uint FindTwoBoardError = 22;
        public const uint ThreadCreateError = 23;
        public const uint StopError = 24;
        public const uint AllocateMemoryError = 25;

        [DllImport("P1602.dll", EntryPoint = "P1602_ShortSub")]
        public static extern ushort ShortSub(ushort fA, ushort fB);


        [DllImport("P1602.dll", EntryPoint = "P1602_FloatSub")]
        public static extern float FloatSub(float nA, float nB);


        [DllImport("P1602.dll", EntryPoint = "P1602_GetDllVersion")]
        public static extern ushort GetDllVersion();


        [DllImport("P1602.dll", EntryPoint = "P1602_DriverInit")]
        public static extern ushort DriverInit(out ushort wTotalBoards);


        [DllImport("P1602.dll", EntryPoint = "P1602_DriverClose")]
        public static extern void DriverClose();


        [DllImport("P1602.dll", EntryPoint = "P1602_GetDriverVersion")]
        public static extern ushort GetDriverVersion(out ushort wVxDVersion);


        [DllImport("P1602.dll", EntryPoint = "P1602_GetConfigAddressSpace")]
        public static extern ushort GetConfigAddressSpace(ushort wBoardNo, out ushort wAddrTimer, out ushort wAddCtrl, out ushort wAddrDio, out ushort wAddrAd);


        [DllImport("P1602.dll", EntryPoint = "P1602_ActiveBoard")]
        public static extern ushort ActiveBoard(ushort wBoardNo);


        [DllImport("P1602.dll", EntryPoint = "P1602_WhichBoardActive")]
        public static extern ushort WhichBoardActive();


        [DllImport("P1602.dll", EntryPoint = "P1602_M_FUN_1")]
        public static extern ushort M_FUN_1(ushort wDaFrequency, ushort wDaWave, float fDaAmplitude, ushort wAdClock, ushort wAdNumber, ushort wAdConfig, out float fAdBuf, float fLowAlarm, float fHighAlarm);


        [DllImport("P1602.dll", EntryPoint = "P1602_M_FUN_2")]
        public static extern ushort M_FUN_2(ushort wDaNumber, ushort wDaWave, out ushort wDaBuf, ushort wAdClock, ushort wAdNumber, ushort wAdConfig, out ushort fAdBuf);


        [DllImport("P1602.dll", EntryPoint = "P1602_M_FUN_3")]
        public static extern ushort M_FUN_3(ushort wDaFrequency, ushort wDaWave, float fDaAmplitude, ushort wAdClock, ushort wAdNumber, out ushort wChannelStatus, out ushort wAdConfig, out float fAdBuf, float fLowAlarm, float fHighAlarm);


        [DllImport("P1602.dll", EntryPoint = "P1602_M_FUN_4")]
        public static extern ushort M_FUN_4(ushort wType, ushort wDaWave, float fDaAmplitude, ushort wAdClock, ushort wAdNumber, out ushort wChannelStatus, out ushort wAdConfig, out float fAdBuf, float fLowAlarm, float fHighAlarm);


        [DllImport("P1602.dll", EntryPoint = "P1602_Do")]
        public static extern ushort Do(ushort wDO);


        [DllImport("P1602.dll", EntryPoint = "P1602_Di")]
        public static extern ushort Di(out ushort wDI);


        [DllImport("P1602.dll", EntryPoint = "P1602_Da")]
        public static extern ushort Da(ushort wDaChannel, ushort wDaVal);


        [DllImport("P1602.dll", EntryPoint = "P1602_SetChannelConfig")]
        public static extern ushort SetChannelConfig(ushort wAdChannel, ushort wConfig);


        [DllImport("P1602.dll", EntryPoint = "P1602_AdPolling")]
        public static extern ushort AdPolling(out float fAdVal);


        [DllImport("P1602.dll", EntryPoint = "P1602_AdsPolling")]
        public static extern ushort AdsPolling(out float fAdVal, ushort wNum);


        [DllImport("P1602.dll", EntryPoint = "P1602_AdsPacer")]
        public static extern ushort AdsPacer(out float fAdVal, ushort dwNum, ushort wSample);


        [DllImport("P1602.dll", EntryPoint = "P1602_ClearScan")]
        public static extern ushort ClearScan();


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScan")]
        public static extern ushort StartScan(ushort wSampleRateDiv, uint dwNum, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_ReadScanStatus")]
        public static extern void ReadScanStatus(out ushort wStatus, out uint dwLowAlarm, out uint dwHighAlarm);


        [DllImport("P1602.dll", EntryPoint = "P1602_AddToScan")]
        public static extern ushort AddToScan(ushort wAdChannel, ushort wConfig, ushort wAverage, ushort wLowAlarm, ushort wHighAlarm, ushort wAlarmType);


        [DllImport("P1602.dll", EntryPoint = "P1602_SaveScan")]
        public static extern ushort SaveScan(ushort wAdChannel, out ushort wBuf);


        [DllImport("P1602.dll", EntryPoint = "P1602_WaitMagicScanFinish")]
        public static extern void WaitMagicScanFinish(out ushort wStatus, out ushort dwLowAlarm, out ushort dwHighAlarm);


        [DllImport("P1602.dll", EntryPoint = "P1602_StopMagicScan")]
        public static extern ushort StopMagicScan();


        [DllImport("P1602.dll", EntryPoint = "P1602_DelayUs")]
        public static extern ushort DelayUs(ushort wDelayUs);


        [DllImport("P1602.dll", EntryPoint = "P1602_Card0_StartScan")]
        public static extern ushort Card0_StartScan(ushort wSampleRate, out ushort wChannelStatus, out ushort wChannelConfig, ushort wCount);


        [DllImport("P1602.dll", EntryPoint = "P1602_Card0_ReadStatus")]
        public static extern ushort Card0_ReadStatus(out ushort wBuf, out ushort wBuf2, out uint dwP1, out uint dwP2, out ushort wStatus);


        [DllImport("P1602.dll", EntryPoint = "P1602_Card0_Stop")]
        public static extern void Card0_Stop();


        [DllImport("P1602.dll", EntryPoint = "P1602_Card1_StartScan")]
        public static extern ushort Card1_StartScan(ushort wSampleRate, out ushort wChannelStatus, out ushort wChannelConfig, ushort wCount);


        [DllImport("P1602.dll", EntryPoint = "P1602_Card1_ReadStatus")]
        public static extern ushort Card1_ReadStatus(out ushort wBuf, out ushort wBuf2, out uint dwP1, out uint dwP2, out ushort wStatus);


        [DllImport("P1602.dll", EntryPoint = "P1602_Card1_Stop")]
        public static extern void Card1_Stop();


        [DllImport("P1602.dll", EntryPoint = "P1602_FunA_Start")]
        public static extern ushort FunA_Start(ushort wClock0Div, out ushort wChannel0, out ushort wConfig0, out ushort Buffer0, uint dwMaxCount0, ushort wClock1Div, out ushort wChannel1, out ushort wConfig1, out ushort Buffer1, uint dwMaxCount1, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_FunA_ReadStatus")]
        public static extern ushort FunA_ReadStatus();


        [DllImport("P1602.dll", EntryPoint = "P1602_FunA_Stop")]
        public static extern ushort FunA_Stop();


        [DllImport("P1602.dll", EntryPoint = "P1602_FunA_Get")]
        public static extern ushort FunA_Get(out uint P0, out uint P1);


        [DllImport("P1602.dll", EntryPoint = "P1602_FunB_Start")]
        public static extern ushort FunB_Start(ushort wClock0Div, out ushort wChannel0, out ushort wConfig0, out ushort Buffer0, uint dwMaxCount0, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_FunB_ReadStatus")]
        public static extern ushort FunB_ReadStatus();


        [DllImport("P1602.dll", EntryPoint = "P1602_FunB_Stop")]
        public static extern ushort FunB_Stop();


        [DllImport("P1602.dll", EntryPoint = "P1602_FunB_Get")]
        public static extern ushort FunB_Get(out uint P0);


        [DllImport("P1602.dll", EntryPoint = "P1602_MemoryStatus")]
        public static extern ushort MemoryStatus(out uint dwTotalPhys, out uint dwAvailPhys, out uint dwTotalPageFile, out uint dwAvailPageFile, out uint dwTotalVirtual, out uint dwAvailVirtual);


        [DllImport("P1602.dll", EntryPoint = "P1602_AllocateMemory")]
        public static extern ushort AllocateMemory(out uint hMem, out ushort Buffer, uint dwSize);


        [DllImport("P1602.dll", EntryPoint = "P1602_FreeMemory")]
        public static extern ushort FreeMemory(uint hMem);


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScanPostTrg")]
        public static extern ushort StartScanPostTrg(ushort wSampleRateDiv, uint dwNum, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScanPreTrg")]
        public static extern ushort StartScanPreTrg(ushort wSampleRateDiv, uint dwNum, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScanMiddleTrg")]
        public static extern ushort StartScanMiddleTrg(ushort wSampleRateDiv, uint dwNum1, uint dwNum2, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScanPreTrgVerC")]
        public static extern ushort StartScanPreTrgVerC(ushort wSampleRateDiv, uint dwNum, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_StartScanMiddleTrgVerC")]
        public static extern ushort StartScanMiddleTrgVerC(ushort wSampleRateDiv, uint dwNum1, uint dwNum2, ushort nPriority);


        [DllImport("P1602.dll", EntryPoint = "P1602_OutputWord")]
        public static extern void OutputWord(uint wPortAddr, uint wOutVal);


        [DllImport("P1602.dll", EntryPoint = "P1602_OutputByte")]
        public static extern void OutputByte(uint wPortAddr, ushort bOutVal);


        [DllImport("P1602.dll", EntryPoint = "P1602_intputWord")]
        public static extern uint IntputWord(uint wPortAddr);


        [DllImport("P1602.dll", EntryPoint = "P1602_IntputByte")]
        public static extern ushort IntputByte(uint wPortAddr);


        [DllImport("P1602.dll", EntryPoint = "P1602_SetCounter")]
        public static extern void SetCounter(uint dwBase, ushort wCounterNo, ushort bCounterMode, uint wCounterValue);


        [DllImport("P1602.dll", EntryPoint = "P1602_ReadCounter")]
        public static extern uint ReadCounter(uint dwBase, ushort wCounterNo, ushort bCounterMode);


        [DllImport("P1602.dll", EntryPoint = "P1602_SetCounterA")]
        public static extern void SetCounterA(ushort wCounterNo, ushort bCounterMode, uint wCounterCalue);


        [DllImport("P1602.dll", EntryPoint = "P1602_ReadCounterA")]
        public static extern uint ReadCounterA(ushort wCounterNo, ushort bCounterMode);

        private int DriverOpened = 0;
        public P1602()//constroctor
        {
            DriverOpened = 0;
        }
        ~P1602()
        {
            if (DriverOpened != 0)
            {
                DriverOpened = 0;
                DriverClose();
            }
        }
       
    }

}

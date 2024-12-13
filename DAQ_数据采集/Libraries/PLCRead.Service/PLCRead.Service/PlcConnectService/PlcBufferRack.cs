using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.PlcConnectService
{
    public class PlcBufferRack
    {
        //定义静态变量保存实例
        public static volatile Dictionary<string, SiemensS7Net> CathodePlc = new Dictionary<string, SiemensS7Net>();
        //定义一个标识确保线程同步
        private static object lockHelper = new object();

        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        //定义私有变量，使外部不能创建该类实例
        private PlcBufferRack()
        {

        }


        public static SiemensS7Net Instance(string plcIp)
        {
            var plcInfo = ReadPlc(plcIp);
            if (plcInfo != null)
            {
                return plcInfo;
            }
            else
            {
                var newConnetc = new SiemensS7Net(SiemensPLCS.S1500, plcIp);
                newConnetc.ConnectTimeOut = 2000;
                newConnetc.SetPersistentConnection();
                if (newConnetc.ConnectServer().IsSuccess)
                {
                    AddPlc(plcIp, newConnetc);
                    return newConnetc;
                }
                else
                {
                    return null;
                }
            }

        }


        public static void RemovePlcIp(string plcIp)
        {
            var plcData = ReadPlc(plcIp);
            if (plcData != null)
            {
                plcData.ConnectClose();
                plcData.Dispose();
                DeletePlc(plcIp);
            }
        }



        public static SiemensS7Net ReadPlc(string key)
        {
            cacheLock.EnterReadLock();
            try
            {
                if (CathodePlc.ContainsKey(key))
                {
                    return CathodePlc[key];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public static void AddPlc(string key, SiemensS7Net data)
        {
            cacheLock.EnterWriteLock();
            try
            {
                CathodePlc.Add(key, data);
            }
            catch
            {
                return;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public static void DeletePlc(string key)
        {
            cacheLock.EnterWriteLock();
            try
            {
                CathodePlc.Remove(key);
            }
            catch
            {
                return;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }
}

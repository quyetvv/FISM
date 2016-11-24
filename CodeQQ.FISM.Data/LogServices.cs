using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeQQ.FISM.Data
{
    public class LogFactory
    {
        private static ILogServices _Logger = null;

        public static ILogServices GetLogger()
        {
            if (_Logger == null)
            {
                _Logger = new LogServices();
            }
            return _Logger;
        }
    }

    public interface ILogServices
    {
        void SetCurrentKMin(int value);
        void ActivaShowLog(bool value);
        void ShowLog(string s);
        void ShowLog(string start, int[] values);
        bool IsShowLog();
        int GetKMinMaxPrint();
    }    

    public class LogServices: ILogServices
    {
        private int _KMIN_MAX_PRINT = 20;

        private int _MAX_TIME_DELAY = 100;

        private int _KMIN_CURRENT = 20;

        // This use to create delay in show message log to screen.
        private bool IsDelayShowLog = true;
        private bool IsActivateShowLog = false;


        public int GetKMinMaxPrint()
        {
            return _KMIN_MAX_PRINT;
        }

        public void SetCurrentKMin(int value)
        {
            _KMIN_CURRENT = value;
        }

        public void ActivaShowLog(bool value)
        {
            IsActivateShowLog = value;
        }

        public void ShowLog(string s)
        {
            if (IsShowLog())
            {
                Console.WriteLine(s);
                DeplayShowLog();
            }
        }

        public void ShowLog(string start, int[] values)
        {
            if (IsShowLog())
            {
                Console.WriteLine(start + string.Join(",", values));
                DeplayShowLog();
            }
        }

        public bool IsShowLog()
        {
            return IsActivateShowLog && _KMIN_CURRENT <= _KMIN_MAX_PRINT;
        }

        private void DeplayShowLog()
        {
            if (IsDelayShowLog)
            {
                Thread.Sleep(_MAX_TIME_DELAY);
            }
        }
    }
}

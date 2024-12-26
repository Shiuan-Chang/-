using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace 記帳系統
{
    public static class DebounceExtension
    {
        // 擴充方法：使用this在control控鍵上，擴充新的方法。前提是該方須要為靜態

        //Action:委派的一種，允許無參數傳遞(委派允許把方法當參數傳遞)
        public static void Debounce(this Control control, Action action, int delay)
        {
            System.Threading.Timer timer = control.Tag as System.Threading.Timer;

            if (timer != null)
            {
                timer.Change(delay, Timeout.Infinite);
            }
            else
            {
                TimerCallback callback = state =>
                {
                    if (control.InvokeRequired)
                    {
                        control.Invoke(new Action(action));
                    }
                    else
                    {
                        action();
                    }

                    DisposeTimer(control);
                };

                timer = new System.Threading.Timer(callback, null, delay, Timeout.Infinite);
                control.Tag = timer;
            }
        }

        private static void ActionAndDisposeTimer(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action.Invoke()));
            }
            else
            {
                action.Invoke();
            }
            DisposeTimer(control);
        }

        private static void DisposeTimer(Control control)
        {
            if (control.Tag is System.Threading.Timer timer)
            {
                timer.Dispose();
                control.Tag = null;
            }
        }
    }
}

//if (debounceTimers.TryGetValue(control, out var existingTimer)) //找到當前計時器，重置其時間
//{
//    existingTimer.Change(delay, Timeout.Infinite);//重置已存在的計時器時間////delay1000毫秒後執行一次然後停止
//}
//else
//{
//    System.Threading.Timer timer = null;  
//    TimerCallback callback = state => 
//    {
//        control.Invoke(new Action(() => 
//        {
//            action();
//        }));
//        timer?.Dispose();
//        debounceTimers.Remove(control);
//    };

//    timer = new System.Threading.Timer(callback, null, delay, Timeout.Infinite);
//    debounceTimers[control] = timer;// timer作為control的vlaue值
//}

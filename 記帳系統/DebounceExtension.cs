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
        private static Dictionary<Control, System.Threading.Timer> debounceTimers = new Dictionary<Control, System.Threading.Timer>();

        // 擴充方法：使用this在control控鍵上，擴充新的方法。前提是該方須要為靜態。
        public static void Debounce(this Control control, Action action, int delay = 1000) //Action:委派的一種，允許無參數傳遞(委派允許把方法當參數傳遞)
        {
            if (debounceTimers.TryGetValue(control, out var existingTimer)) //找到當前計時器，重置其時間
            {
                existingTimer.Change(delay, Timeout.Infinite);//重置已存在的計時器時間////delay1000毫秒後執行一次然後停止
            }
            else
            {
                System.Threading.Timer timer = null;  
                TimerCallback callback = state => //callback 指定計時器的觸發方法，lambda表達
                {
                    control.Invoke(new Action(() => // 由後端轉成前端時，確保UI執行緒上執行
                    {
                        action();
                    }));
                    timer?.Dispose();
                    debounceTimers.Remove(control);
                };

                timer = new System.Threading.Timer(callback, null, delay, Timeout.Infinite);
                debounceTimers[control] = timer;// timer作為control的vlaue值
            }
        }
    }
}

// 1001
//
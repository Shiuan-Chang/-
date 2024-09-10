using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Models;

namespace 記帳程式.Forms
{
    internal class SingletonForm
    {
        private static Form currentForm = null;//獨體模式1：static 可以控制共用同一個instace，而不是一直new一個instance出來

        //獨體模式2:透過建立class，控管使用時只有一個instance

        // 取代switch，採用路徑對應方式，根據窗體名稱動態創建窗體的實例
        public static Form CreateForm(FormType formName)
        {
            if (currentForm != null)
            {
                currentForm.Hide();
            }
            string type = "記帳系統.Forms." + formName.ToString();
            Type t = Type.GetType(type); // 反射用法,透過該方式回找需要的form
            currentForm = (Form)Activator.CreateInstance(t); // 根據t創建一個實例
            return currentForm;
        }
    }
}

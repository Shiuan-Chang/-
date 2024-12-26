using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Components;
using 記帳系統.Models;

namespace 記帳系統.Forms
{
    internal class SingletonForm
    {
        private static Form currentForm = null; // 獨體模式1：static 可以控制共用同一個instace，而不是一直new一個instance出來
        private static Dictionary<FormType, Form> tempForm = new Dictionary<FormType, Form>();

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

            //把先前狀態存在dictionary中的狀態裡，保留之前的狀態→讓之前文字框得東西保存下來
            if (tempForm.ContainsKey(formName))
            {
                currentForm = tempForm[formName];
            } else
            {
                currentForm = (Form)Activator.CreateInstance(t); // 根據t創建一個實例
                tempForm[formName] = currentForm;
            }

            //現存頁面無法再度點取
            FieldInfo navBarField = t.GetFields(BindingFlags.Instance)
                                   .FirstOrDefault(x => x.FieldType == typeof(NavBar));

            //FieldInfo的主要用途和功能
           //獲取字段信息：可以使用FieldInfo來查詢字段的名稱、是否為靜態的、它的數據類型等。
           //讀取和設置字段值：FieldInfo允許你對指定對象的字段賦值或取值。這可以對私有或保護的字段進行操作，這在正常的類型安全訪問中是不被允許的。
           //應用於動態類型和運行時編程：在動態生成的類型或者在需要根據用戶輸入或其他運行時數據來讀取或修改對象狀態的情況下，FieldInfo非常有用。

            if (navBarField != null)
            {
                NavBar navbar = (NavBar)navBarField.GetValue(currentForm);
                navbar.DisableButton(formName); // 调用NavBar的DisableButton方法

            }

            return currentForm;
        }
        
        // 抓取一開始預設畫面，並到navbar設定禁止按鈕click
        public static string CurrentFormType()
        {
            if (currentForm != null)
            {
                return currentForm.GetType().Name;
            }
            return null; 
        }


    }

}

// 單體模式
// 標籤
// 狀態保存<dictonary>→讓之前頁面的狀態保留下來，像輸入過文字的視窗，保留輸入的文字
// 反射→可以用來輔助做到動態生成

//HW1: 修掉一開始無法disable
//Hw2: 不要寫死navBar1 去找 t的Type=NavBar
//NavBar navbar = (NavBar)t.GetField("navBar1").GetValue(currentForm);
//navbar.DisableButton(formName);




//t.GetField("navBar1").GetMethod("DisableButton").Invoke(navbar,new object[]{formName})

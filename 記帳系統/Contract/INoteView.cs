using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳系統.Models;

namespace 記帳系統.Contract
{
    public interface INoteView
    {     
            void DataGenerator(List<AccountingModel> lists);
    }
}

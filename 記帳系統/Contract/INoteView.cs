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
            void ClearDataGridView();
            void SetDataSource(List<AccountingModel> lists);
            void HideColumns(string[] columnNames);
            void SetColumnHeaderText(int columnIndex, string headerText);
            void SetColumnCellTemplate(int columnIndex, DataGridViewCell cellTemplate);
            void AddImageColumn(string headerText, DataGridViewImageCellLayout layout);
            void SetRowImageValues(int rowIndex, Bitmap[] images);
    }
}

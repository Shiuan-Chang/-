using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class UpdateAnalysisModel
    {
        public int ColumnIndex { get; set; }
        public string NoteDate { get; set; }
        public string NoteHour { get; set; }
        public string UpdateData { get; set; }

        public UpdateAnalysisModel(int columnIndex, string noteDate, string noteHour, string updateData)
        {
            ColumnIndex = columnIndex;
            NoteDate = noteDate;
            NoteHour = noteHour;
            UpdateData = updateData;
        }
    }
}

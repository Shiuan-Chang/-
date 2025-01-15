using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{

    public class UpdateNoteModel
    {
        public int ColumnIndex { get; set; }    
        public string NoteDate { get; set; }    
        public string NoteHour { get;set; }
        public string UpdateData { get;set; }

        public UpdateNoteModel(int columnIndex, string noteDate, string noteHour, string updateData)
        {
            ColumnIndex = columnIndex;
            NoteDate = noteDate;
            NoteHour = noteHour;
            UpdateData = updateData;
        }
    }
}

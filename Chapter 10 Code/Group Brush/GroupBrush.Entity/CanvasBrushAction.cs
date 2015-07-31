using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.Entity
{
    public class CanvasBrushAction
    {
        public int Sequence { get; set; }
        public Int64 ClientSequenceId { get; set; }
        public int Type { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public List<Position> BrushPositions { get; set; }
    }
}

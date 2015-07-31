using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Canvases
{
    public interface ILookUpCanvasData
    {
        Guid? LookUpCanvas(string canvasName);
    }
}

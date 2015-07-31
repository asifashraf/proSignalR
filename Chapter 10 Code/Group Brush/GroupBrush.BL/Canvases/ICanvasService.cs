using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Canvases
{
    public interface ICanvasService
    {
        Guid? CreateCanvas(CanvasDescription canvasDescription);
        Guid? LookUpCanvas(string canvasName);
    }
}

using GroupBrush.DL.Canvases;
using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Canvases
{
    public class CanvasService : ICanvasService
    {
        ICreateCanvasData _createCanvasData;
        ILookUpCanvasData _lookUpCanvasData;
        public CanvasService(ICreateCanvasData createCanvasData, ILookUpCanvasData lookUpCanvasData)
        {
            _createCanvasData = createCanvasData;
            _lookUpCanvasData = lookUpCanvasData;
        }
        public Guid? CreateCanvas(CanvasDescription canvasDescription)
        {
            Guid? canvasId = null;
            if (canvasDescription != null && !string.IsNullOrWhiteSpace(canvasDescription.Name) &&
            !string.IsNullOrWhiteSpace(canvasDescription.Description))
            {
                canvasId = _createCanvasData.CreateCanvas(canvasDescription.Name,
                canvasDescription.Description);
            }
            return canvasId;
        }
        public Guid? LookUpCanvas(string canvasName)
        {
            Guid? canvasId = null;
            if (!string.IsNullOrWhiteSpace(canvasName))
            {
                canvasId = _lookUpCanvasData.LookUpCanvas(canvasName);
            }
            return canvasId;
        }
    }
}

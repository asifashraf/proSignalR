using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Canvases
{
    public interface ICanvasRoomService
    {
        CanvasBrushAction AddBrushAction(string canvasId, CanvasBrushAction brushData);
        CanvasSnapshot SyncToRoom(string canvasId, int currentPosition);
        void AddUserToCanvas(string canvasId, string id);
        void RemoveUserFromCanvas(string canvasId, string id);
    }
}

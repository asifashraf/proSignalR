using GroupBrush.BL.Storage;
using GroupBrush.DL.Canvases;
using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Canvases
{
    public class CanvasRoomService : ICanvasRoomService
    {
        IMemStorage _memStorage;
        IGetCanvasDescriptionData _getCanvasDescriptionData;
        public CanvasRoomService(IMemStorage memStorage, IGetCanvasDescriptionData getCanvasDescriptionData)
        {
            _memStorage = memStorage;
            _getCanvasDescriptionData = getCanvasDescriptionData;
        }
        public CanvasBrushAction AddBrushAction(string canvasId, CanvasBrushAction brushData)
        {
            return _memStorage.AddBrushAction(canvasId, brushData);
        }
        public CanvasSnapshot SyncToRoom(string canvasId, int currentPosition)
        {
            CanvasDescription canvasDescription =
            _getCanvasDescriptionData.GetCanvasDescription(Guid.Parse(canvasId));
            List<CanvasBrushAction> actions = new List<CanvasBrushAction>();
            actions = _memStorage.GetBrushActions(canvasId, currentPosition);
            List<string> users = new List<string>();
            users = _memStorage.GetCanvasUsers(canvasId);
            return new CanvasSnapshot()
            {
                Users = users,
                Actions = actions,
                CanvasName =
                    canvasDescription.Name,
                CanvasDescription = canvasDescription.Description
            };
        }
        public void AddUserToCanvas(string canvasId, string id)
        {
            _memStorage.AddUserToCanvas(canvasId, id);
        }
        public void RemoveUserFromCanvas(string canvasId, string id)
        {
            _memStorage.RemoveUserFromCanvas(canvasId, id);
        }
    }
}

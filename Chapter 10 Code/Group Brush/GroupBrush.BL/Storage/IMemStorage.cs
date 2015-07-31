using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Storage
{
    public interface IMemStorage
    {
        CanvasBrushAction AddBrushAction(string canvasId, CanvasBrushAction brushData);
        List<CanvasBrushAction> GetBrushActions(string canvasId, int currentPosition);
        List<string> GetCanvasUsers(string canvasId);
        void AddUserToCanvas(string canvasId, string id);
        void RemoveUserFromCanvas(string canvasId, string id);
        string GetUserName(int id);
        void StoreUserName(int id, string userName);
    }
}

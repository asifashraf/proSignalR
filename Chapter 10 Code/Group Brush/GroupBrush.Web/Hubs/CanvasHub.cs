using GroupBrush.BL.Canvases;
using GroupBrush.BL.Users;
using GroupBrush.Entity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.Web.Hubs
{
    public class CanvasHub : Hub
    {
        private IUserService _userService;
        private ICanvasRoomService _canvasRoomService;
        public CanvasHub(IUserService userService, ICanvasRoomService canvasRoomService)
        {
            _userService = userService;
            _canvasRoomService = canvasRoomService;
        }
        private string GetCanvasIdFromQueryString()
        {
            Guid validationGuid = Guid.Empty;
            string groupId = Context.QueryString["canvasid"];
            if (!string.IsNullOrWhiteSpace(groupId) && Guid.TryParse(groupId, out validationGuid))
            {
                return groupId;
            }
            throw new ArgumentException("Invalid Canvas Id");
        }
        private string GetUserNameFromContext()
        {
            string strUserId = Context.Request.User.Identity.Name;
            int userId = 0;
            if (int.TryParse(strUserId, out userId))
            {
                return _userService.GetUserNameFromId(userId);
            }
            return string.Empty;
        }
        public override Task OnConnected()
        {
            _canvasRoomService.AddUserToCanvas(GetCanvasIdFromQueryString(), GetUserNameFromContext());
            Groups.Add(Context.ConnectionId.ToString(), GetCanvasIdFromQueryString());
            Clients.Group(GetCanvasIdFromQueryString()).UserConnected(GetUserNameFromContext());
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            _canvasRoomService.RemoveUserFromCanvas(GetCanvasIdFromQueryString(),
            GetUserNameFromContext());
            Groups.Remove(Context.ConnectionId.ToString(), GetCanvasIdFromQueryString());
            Clients.Group(GetCanvasIdFromQueryString()).UserDisconnected(GetUserNameFromContext());
            return base.OnDisconnected(stopCalled);
        }
        public void MoveCursor(double x, double y)
        {
            Clients.Group(GetCanvasIdFromQueryString()).MoveOtherCursor(GetUserNameFromContext(), x, y);
        }
        public void SendChatMessage(string message)
        {
            Clients.Group(GetCanvasIdFromQueryString()).UserChatMessage(GetUserNameFromContext()
            + ": " + message);
        }
        public void SendDrawCommand(CanvasBrushAction brushData)
        {
            CanvasBrushAction canvasBrushAction =
            _canvasRoomService.AddBrushAction(GetCanvasIdFromQueryString(), brushData);
            Clients.Group(GetCanvasIdFromQueryString()).DrawCanvasBrushAction(canvasBrushAction);
        }
        public CanvasSnapshot SyncToRoom(int currentPosition)
        {
            return _canvasRoomService.SyncToRoom(GetCanvasIdFromQueryString(), currentPosition);
        }
    }
}
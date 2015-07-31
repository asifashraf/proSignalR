using GroupBrush.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Storage
{
    public class MemoryStorage : IMemStorage
    {
        private ConcurrentDictionary<string, int> canvasTransactions = new ConcurrentDictionary<string, int>();
        private ConcurrentDictionary<string, ConcurrentBag<CanvasBrushAction>> canvasActions = new
        ConcurrentDictionary<string, ConcurrentBag<CanvasBrushAction>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> canvasUsers = new
        ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        private ConcurrentDictionary<int, string> userNames = new ConcurrentDictionary<int, string>();
        public CanvasBrushAction AddBrushAction(string canvasId, CanvasBrushAction brushData)
        {
            if (!canvasTransactions.ContainsKey(canvasId))
            {
                canvasTransactions[canvasId] = 0;
            }
            int transactionNumber = canvasTransactions[canvasId] = canvasTransactions[canvasId]++;
            if (!canvasActions.ContainsKey(canvasId))
            {
                canvasActions[canvasId] = new ConcurrentBag<CanvasBrushAction>();
            }
            brushData.Sequence = transactionNumber;
            canvasActions[canvasId].Add(brushData);
            return brushData;
        }
        public List<CanvasBrushAction> GetBrushActions(string canvasId, int currentPosition)
        {
            List<CanvasBrushAction> actions = new List<CanvasBrushAction>();
            if (canvasActions.ContainsKey(canvasId))
            {
                ConcurrentBag<CanvasBrushAction> storedActions = canvasActions[canvasId];
                actions.AddRange(storedActions.Where(x => x.Sequence >= currentPosition));
            }
            actions.Sort(new Comparison<CanvasBrushAction>((a, b) =>
            {
                return
                    a.Sequence.CompareTo(b.Sequence);
            }));
            return actions;
        }
        public List<string> GetCanvasUsers(string canvasId)
        {
            List<string> returnValue = new List<string>();
            if (canvasUsers.ContainsKey(canvasId))
            {
                HashSet<string> uniqueList = new HashSet<string>();
                ConcurrentDictionary<string, string> users = canvasUsers[canvasId];
                foreach (KeyValuePair<string, string> user in users)
                {
                    uniqueList.Add(user.Key);
                }
                returnValue = uniqueList.ToList<string>();
            }
            return returnValue;
        }
        public void AddUserToCanvas(string canvasId, string id)
        {
            if (!canvasUsers.ContainsKey(canvasId))
            {
                canvasUsers[canvasId] = new ConcurrentDictionary<string, string>();
            }
            ConcurrentDictionary<string, string> users = canvasUsers[canvasId];
            if (!users.ContainsKey(id))
            {
                users[id] = id;
            }
        }
        public void RemoveUserFromCanvas(string canvasId, string id)
        {
            if (canvasUsers.ContainsKey(canvasId))
            {
                ConcurrentDictionary<string, string> users = canvasUsers[canvasId];
                if (users.ContainsKey(id))
                {
                    string tempValue = null;
                    users.TryRemove(id, out tempValue);
                }
            }
        }
        public string GetUserName(int id)
        {
            if (userNames.ContainsKey(id))
            {
                return userNames[id];
            }
            return null;
        }
        public void StoreUserName(int id, string userName)
        {
            userNames[id] = userName;
        }
    }
}

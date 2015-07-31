using BookSleeve;
using GroupBrush.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Storage
{
    public class RedisStorage : IMemStorage
    {
        private const string TRANSACTION_PREFIX = "CanvasTransaction:";
        private const string ACTION_PREFIX = "CanvasBrushAction:";
        private const string USERS_PREFIX = "CanvasUsers:";
        private const string USERNAMES_PREFIX = "CanvasUsernames:";
        private readonly RedisConfiguration _redisConfiguration;
        Dictionary<int, string> _userNames;
        public RedisStorage(RedisConfiguration redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
            _userNames = new Dictionary<int, string>();
        }
        public CanvasBrushAction AddBrushAction(string canvasId, CanvasBrushAction brushData)
        {
            int transactionNumber = 0;
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                var incrTask = conn.Hashes.Increment(0, TRANSACTION_PREFIX + canvasId,
                "transaction");
                transactionNumber = (int)incrTask.Result;
            }
            brushData.Sequence = transactionNumber;
            string serializedData = JsonConvert.SerializeObject(brushData);
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                conn.Lists.AddLast(0, ACTION_PREFIX + canvasId, serializedData);
            }
            return brushData;
        }
        public List<CanvasBrushAction> GetBrushActions(string canvasId, int currentPosition)
        {
            List<CanvasBrushAction> actions = new List<CanvasBrushAction>();
            string[] storedActions = null;
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                var rangeTask = conn.Lists.RangeString(0, ACTION_PREFIX + canvasId, currentPosition,
                Int32.MaxValue);
                storedActions = rangeTask.Result;
            }
            if (storedActions != null)
            {
                foreach (string storedAction in storedActions)
                {
                    actions.Add(JsonConvert.DeserializeObject<CanvasBrushAction>(storedAction));
                }
                actions.Sort(new Comparison<CanvasBrushAction>((a, b) =>
                {
                    return a.Sequence.
                        CompareTo(b.Sequence);
                }));
            }
            return actions;
        }
        public List<string> GetCanvasUsers(string canvasId)
        {
            List<string> returnValue = new List<string>();
            HashSet<string> uniqueList = new HashSet<string>();
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                var getAllTask = conn.Sets.GetAllString(0, USERS_PREFIX + canvasId);
                uniqueList = new HashSet<string>(getAllTask.Result.ToList());
            }
            returnValue = uniqueList.ToList<string>();
            return returnValue;
        }
        public void AddUserToCanvas(string canvasId, string id)
        {
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                conn.Sets.Add(0, USERS_PREFIX + canvasId, id);
            }
        }
        public void RemoveUserFromCanvas(string canvasId, string id)
        {
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                conn.Sets.Remove(0, USERS_PREFIX + canvasId, id);
            }
        }
        public string GetUserName(int id)
        {
            string userName = null;
            if (_userNames.ContainsKey(id))
            {
                userName = _userNames[id];
            }
            else
            {
                using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.Port, password: _redisConfiguration.Password))
                {
                    conn.Open();
                    var getTask = conn.Strings.GetString(0, USERNAMES_PREFIX + id.ToString());
                    userName = getTask.Result;
                    _userNames[id] = userName;
                }
            }
            return userName;
        }
        public void StoreUserName(int id, string userName)
        {
            using (var conn = new RedisConnection(_redisConfiguration.HostName, _redisConfiguration.
            Port, password: _redisConfiguration.Password))
            {
                conn.Open();
                conn.Strings.Set(0, USERNAMES_PREFIX + id.ToString(), userName).Wait();
            }
            _userNames[id] = userName;
        }
    }
}

using KittyEngine.Core.Common;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KittyEngine.Core.State
{
    public class StateSynchronizer<TSynchronized>
    {
        private JObject _initial;
        private TSynchronized _synchronized;

        public StateSynchronizer(TSynchronized initial)
        {
            _initial = JObject.FromObject(initial);
            _synchronized = initial;
        }

        public JsonPatchDocument GetJsonPatch()
        {
            var updated = JObject.FromObject(_synchronized);
            var patch = JsonDiffPatch.Diff(_initial, updated);
            return patch;
        }

        public string GetJsonSerializedState()
        {
            var json = JsonConvert.SerializeObject(_synchronized);
            return json;
        }
    }
}

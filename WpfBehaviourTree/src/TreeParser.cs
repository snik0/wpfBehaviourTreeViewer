using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfBehaviourTree.src;

namespace WpfBehaviourTree
{
    // class provides basic json parsing and generates TreeNodes from them
    static class TreeParser
    {
        public static TreeNode Parse(string in_json)
        {
            JObject template = JObject.Parse(in_json);
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                var node = (TreeNode)serializer.Deserialize(new JTokenReader(template), typeof(TreeNode));

                return node;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

            return null;
        }
    }
}

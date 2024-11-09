using AE.Core;
using AE.Core.Log;
using AE.Dal;
using AE.Node.Interface;
using AE.Node.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AE.Node
{
    public static class WorkflowManager
    {
        private static WorkflowSettings Settings;

        private static readonly List<string> DataTypes = new List<string>
        {
            "*",
            "THREAD",
            "BOOLEAN",
            "BYTE",
            "INT", 
            "FLOAT", 
            "DOUBLE", 
            "STRING", 
            "POINT", 
            "RANGE", 
            "COLOR", 
            "KEY", 
            "KEY_MODIFIER",
            "MOUSE_BUTTON",
            "MOUSE_EVENT",
        };

        private static readonly Dictionary<string, INodeType> NodeTypes = new Dictionary<string, INodeType>();

        private static readonly Dictionary<string, object> UserInputTypes = new Dictionary<string, object> {
            { "BOOLEAN", false },
            { "BYTE", (byte)0 },
            { "INT", 0 },
            { "FLOAT", 0f },
            { "DOUBLE", 0d },
            { "STRING", "" },
            { "POINT", new double[2] {0, 0} },
            { "RANGE", new double[4] {0, 0, 0, 0} },
            { "COLOR", new byte[3] { 0, 0, 0 } },
            { "KEY", Keys.A },
            { "KEY_MODIFIER", KeyModifiers.Control },
            { "MOUSE_BUTTON", MouseButtonType.Left },
            { "MOUSE_EVENT", MouseEventType.MouseDown },  
        };

        public static void Init(WorkflowSettings settings)
        {
            Settings = settings;

            foreach (var type in settings.TypeColors.Keys)
            {
                if (!DataTypes.Contains(type))
                    DataTypes.Add(type);
            }
        }

        public static bool AddDataType(string dataType, string color)
        {
            if (DataTypes.Contains(dataType))
            {
                AELogger.DefaultLogger?.Log($"Duplicate {dataType} data type!", LogLevel.Error);
                return false;
            }

            AELogger.DefaultLogger?.Log($"Add {dataType} data type.", LogLevel.Message);
            DataTypes.Add(dataType);

            if (!Settings.TypeColors.ContainsKey(dataType))
                Settings.TypeColors.Add(dataType, color);

            return true;
        }

        public static bool AddInputEnumType(string type, string color, Enum value)
        {
            var result = AddDataType(type, color);

            if (result)
                UserInputTypes.Add(type, value);

            return result;
        }

        public static bool AddNodeType(INodeType nodeType)
        {
            if (NodeTypes.ContainsKey(nodeType.Name))
            {
                AELogger.DefaultLogger?.Log($"Duplicate {nodeType.Name} node type!", LogLevel.Error);
                return false;
            }

            if (nodeType.In.Concat(nodeType.Out).Concat(nodeType.Values.Cast<PinItem>()).Any(p => !DataTypes.Contains(p.Type)))
            {
                AELogger.DefaultLogger?.Log($"Node type {nodeType.Name} contains not registered type!", LogLevel.Error);
                return false;
            }

            if (nodeType.Values.Any(v => UserInputTypes.ContainsKey(v.Type) == false))
            {
                AELogger.DefaultLogger?.Log($"Node type {nodeType.Name} contains values with incorrect type!", LogLevel.Error);
                return false;
            }

            AELogger.DefaultLogger?.Log($"Add {nodeType.Name} node type.", LogLevel.Message);
            NodeTypes.Add(nodeType.Name, nodeType);

            return true;
        }

        public static Color GetTypeColor(string type, FactorType factorType = FactorType.Color, int factor = 0)
        {
            return ColorPath.GetColor(Settings.TypeColors[type], factorType, factor);
        }

        public static object GetTypeValue(string type)
        {
            if (UserInputTypes.ContainsKey(type))
                return UserInputTypes[type];

            return null;
        }

        public static Dictionary<string, IEnumerable<string>> GetNodeTypes()
        {
            return NodeTypes.Values
                .GroupBy(v => v.Group)
                .ToDictionary(
                    v => v.Key, 
                    v => v.Select(i => i.Name)
                );
        }

        public static NodeItem CreateNode(int id, string type)
        {
            var data = NodeTypes[type];

            return new NodeItem
            {
                Id = id,
                Type = type,
                Title = data.DefaultTitle.Empty($"Node {type}"),
                Shape = Settings.DefaultShape,
                Color = data.DefaultColor.Empty(Settings.DefaultColor),
                Enabled = true,
                In = data.In.Select(p => new PinItem { Title = p.Title, Type = p.Type }).ToArray(),
                Out = data.Out.Select(p => new PinItem { Title = p.Title, Type = p.Type }).ToArray(),
                Values = data.Values.Select(p => new PinValue { Title = p.Title, Type = p.Type, Value = p.Value }).ToArray(),
            };
        }
    }
}

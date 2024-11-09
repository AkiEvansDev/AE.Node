using AE.Dal;
using System.Collections.Generic;

namespace AE.Node
{
    public class WorkflowSettings
    {
        public NodeShapeType DefaultShape { get; set; } = NodeShapeType.Round;
        public string DefaultColor { get; set; } = "#212121";
        public Dictionary<string, string> TypeColors { get; set; } = new Dictionary<string, string>();

        public WorkflowSettings()
        {
            TypeColors.Add("*", $"#{new ColorPath(ColorKey.SilverNight, FactorType.Shade, 2).ToColor().Name}");
            TypeColors.Add("THREAD", $"#{new ColorPath(ColorKey.SilverNight, FactorType.Tint, 4).ToColor().Name}");

            TypeColors.Add("BOOLEAN", $"#{new ColorPath(ColorKey.CrimsonRed).ToColor().Name}");
            TypeColors.Add("BYTE", $"#{new ColorPath(ColorKey.Celadon, FactorType.Shade, 1).ToColor().Name}");
            TypeColors.Add("INT", $"#{new ColorPath(ColorKey.Eucalyptus, FactorType.Shade, 1).ToColor().Name}");
            TypeColors.Add("FLOAT", $"#{new ColorPath(ColorKey.TropicalRainForest, FactorType.Shade, 1).ToColor().Name}");
            TypeColors.Add("DOUBLE", $"#{new ColorPath(ColorKey.TropicalRainForest, FactorType.Shade, 1).ToColor().Name}");
            TypeColors.Add("STRING", $"#{new ColorPath(ColorKey.ChinesePink).ToColor().Name}");
            TypeColors.Add("POINT", $"#{new ColorPath(ColorKey.BlueEyes, FactorType.Shade, 2).ToColor().Name}");
            TypeColors.Add("RANGE", $"#{new ColorPath(ColorKey.PaleViolet, FactorType.Shade, 2).ToColor().Name}");
            TypeColors.Add("COLOR", $"#{new ColorPath(ColorKey.HarlequinGreen, FactorType.Shade, 2).ToColor().Name}");
            TypeColors.Add("KEY", $"#{new ColorPath(ColorKey.Calamansi).ToColor().Name}");
            TypeColors.Add("KEY_MODIFIER", $"#{new ColorPath(ColorKey.Calamansi).ToColor().Name}");
            TypeColors.Add("MOUSE_BUTTON", $"#{new ColorPath(ColorKey.Calamansi).ToColor().Name}");
            TypeColors.Add("MOUSE_EVENT", $"#{new ColorPath(ColorKey.Calamansi).ToColor().Name}");
        }
    }
}

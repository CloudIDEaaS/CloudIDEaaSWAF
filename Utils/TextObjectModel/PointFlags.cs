using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.TextObjectModel
{
    public enum PointType
    {
        AllowOffClient = 512,
        ClientCoord = 256,
        ObjectArg = 2048,
        Transform = 1024,
        NoUpdateCurrentPosition = 0,
        UpdateCurrentPosition = 1,
        Left = 0,
        Right = 2,
        Center = 6,
        Top = 0,
        Bottom = 8,
        Baseline = 24,
        RTLReading = 256,
        Mask = (Baseline + Center + UpdateCurrentPosition + RTLReading),
        VBaseline = Baseline,
        VLeft = Bottom,
        VRight = Top,
        VCenter = Center,
        VBottom = Right,
        VTop = Left
    }
}

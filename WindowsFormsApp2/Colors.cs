using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WindowsFormsApp2
{
    static class Colors
    {
        private const int alpha = 150;
        private static readonly Color[] positiveColors = { Color.FromArgb(alpha, 254, 224, 144), Color.FromArgb(alpha, 253, 174, 97), Color.FromArgb(alpha, 244, 109, 67), Color.FromArgb(alpha, 215, 48, 39), Color.FromArgb(alpha, 165, 0, 38) };
        private static readonly Color[] negativeColors = { Color.FromArgb(alpha, 224, 243, 248), Color.FromArgb(alpha, 171, 217, 233), Color.FromArgb(alpha, 116, 173, 209), Color.FromArgb(alpha, 69, 117, 180), Color.FromArgb(alpha, 49, 54, 149) };

        private static int Convert(decimal value, decimal From1, decimal From2, decimal To1, decimal To2) => (int)((value - From1) / (From2 - From1) * (To2 - To1) + To1);
        
        private static Color[] GetArrayOfColors(Func<decimal, bool> condition) => condition.EqualsMethods(x => x > 0) ? positiveColors : negativeColors;
        
        private static List<List<GMapPolygon>> GetColors(Func<decimal, bool> condition, Dictionary<List<GMapPolygon>, decimal> information) 
        {
            var data = information.Values.Where(condition);

            if (data.Count() == 0)
            {
                return new List<List<GMapPolygon>>();
            }

            var min = data.Min();
            var max = data.Max();
            var colors = GetArrayOfColors(condition);

            foreach (var state in information)
            {
                if (condition(state.Value))
                {
                    foreach (var polygon in state.Key)
                    {
                        if (state.Value == decimal.Zero)
                        {
                            polygon.Fill = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255));
                            continue;
                        }

                        if (state.Value == decimal.MinValue)
                        {
                            polygon.Fill = new SolidBrush(Color.FromArgb(alpha, 170, 170, 170));
                            continue;
                        }

                        int index = Convert(state.Value, min, max, decimal.Zero, positiveColors.Length - 1);
                        polygon.Fill = new SolidBrush(colors[index]);
                    }
                }
            }

            return information.Keys.ToList();
        }

        public static List<List<GMapPolygon>> GetLists(Dictionary<List<GMapPolygon>, decimal> information)
        {
            var positivePolygons = GetColors(x => x > decimal.Zero, information);
            var negativePolygons = GetColors(x => x < decimal.Zero, information);
            return positivePolygons.Concat(negativePolygons).ToList();
        }
    }
}

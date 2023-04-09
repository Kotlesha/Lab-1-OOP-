using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp2
{
    class Tweets
    {
        public static Dictionary<GMarkerGoogle, decimal> TweetsInfromation { get; } = new Dictionary<GMarkerGoogle, decimal>();

        public Tweets(string path)
        {
            StreamReader reader = new StreamReader(path);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] data = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (data.Length != 4)
                {
                    continue;
                }

                string[] location = data[0].Split(new char[] { '[', ']', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                PointLatLng point = new PointLatLng(double.Parse(location[0].Replace('.', ',')), double.Parse(location[1].Replace('.', ',')));
                GMarkerGoogle marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_pushpin);
                decimal totalValue = Words.GetValue(data[3]);
                TweetsInfromation.Add(marker, totalValue);
            }

            reader.Close();
        }
    }
}

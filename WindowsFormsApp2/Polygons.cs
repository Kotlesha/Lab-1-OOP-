using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsFormsApp2
{
    class Polygons
    {
        public static Dictionary<string, List<GMapPolygon>> StatesInformation { get; } = new Dictionary<string, List<GMapPolygon>>();

        public Polygons(string nameOfFile)
        {
            StreamReader reader = new StreamReader(nameOfFile);
            string states = reader.ReadToEnd();
            reader.Close();

            JObject polygonsInformation = JObject.Parse(states);
            foreach (var polygonInfromation in polygonsInformation)
            { 
                string nameOfState = polygonInfromation.Key;
                List<GMapPolygon> polygons = new List<GMapPolygon>();

                foreach (var array in polygonInfromation.Value)
                {
                    List<PointLatLng> points = new List<PointLatLng>();

                    foreach (var item in array)
                    {
                        if (item[0] is JValue)
                        {
                            points.Add(new PointLatLng((double)item[1], (double)item[0]));
                        }

                        if (item[0] is JArray)
                        {
                            foreach (var elements in item)
                            {
                                points.Add(new PointLatLng((double)elements[1], (double)elements[0]));
                            }
                        }
                    }

                    GMapPolygon polygon = new GMapPolygon(points, nameOfState);
                    polygons.Add(polygon);
                }

                StatesInformation.Add(nameOfState, polygons);
            }
        }

        public static Dictionary<List<GMapPolygon>, List<KeyValuePair<GMarkerGoogle, decimal>>> GroupingTweetsByStates()
        {
            var tweets = Tweets.TweetsInfromation;
            var groupTweetByStates = new Dictionary<List<GMapPolygon>, List<KeyValuePair<GMarkerGoogle, decimal>>>();
            foreach (var tweet in tweets)
            {
                foreach (var state in StatesInformation)
                {
                    foreach (var polygon in state.Value)
                    {
                        if (polygon.IsInside(tweet.Key.Position))
                        {
                            if (groupTweetByStates.ContainsKey(state.Value))
                            {
                                groupTweetByStates[state.Value].Add(tweet);
                            }
                            else
                            {
                                groupTweetByStates.Add(state.Value, new List<KeyValuePair<GMarkerGoogle, decimal>>() { tweet });
                            }
                        }
                    }
                }
            }

            return groupTweetByStates;
        }

        public static Dictionary<List<GMapPolygon>, decimal> GetStatesWithValues()
        {
            var groupTweetByStates = GroupingTweetsByStates();
            var result = StatesInformation.Select(element => element.Value)
                .ToDictionary(item => item, item => decimal.MinValue);

            foreach (var tweet in groupTweetByStates)
            {
                decimal totalValue = 0.0m;

                foreach (var pair in tweet.Value)
                {
                    totalValue += pair.Value;
                }
                    
                if (result[tweet.Key] == decimal.MinValue)
                {
                    result[tweet.Key] = decimal.Zero;
                }

                result[tweet.Key] = totalValue / tweet.Key.Count;
            }

            return result;
        }
    }
}

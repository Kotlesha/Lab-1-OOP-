using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Words words = new Words("sentiments.csv");
            Tweets tweets = new Tweets("cali_tweets2014.txt");
            Polygons gMapPolygons = new Polygons("states.json");
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.SetPositionByKeywords("Usa");
            gMapControl1.MapProvider = GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 3; 
            gMapControl1.MaxZoom = 16; 
            gMapControl1.Zoom = 3; 
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; 
            gMapControl1.CanDragMap = true; 
            gMapControl1.DragButton = MouseButtons.Left; 
            gMapControl1.ShowCenter = false; 
            gMapControl1.ShowTileGridLines = false; 
            gMapControl1.SetPositionByKeywords("USA");

            GMapOverlay polygons = new GMapOverlay("polygons");
            var statesValues = Polygons.GetStatesWithValues();
            var statesWithColors = Colors.GetLists(statesValues);

            foreach (var states in statesWithColors)
            {
                foreach (var state in states)
                {
                    polygons.Polygons.Add(state);
                }
            }

            gMapControl1.Overlays.Add(polygons);

            GMapOverlay overlay = new GMapOverlay("markers");
            foreach (var element in Tweets.TweetsInfromation)
            {
                overlay.Markers.Add(element.Key);
            }

            gMapControl1.Overlays.Add(overlay);

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSIsoft.PIDevClub.PIWebApiClient;
using OSIsoft.PIDevClub.PIWebApiClient.Client;
using OSIsoft.PIDevClub.PIWebApiClient.Model;


namespace PI_Csharp
{
    public partial class Form1 : Form
    {
        public static PIWebApiClient client;
        public static PIDataServer ds;
        public static PIAssetServer afs;
        public static List<PIPoint> piPoints;
        public static Dictionary<string,string> pointDescriptors;
        public Form1()
        {
            InitializeComponent();

            //Create an instance of the PI Web API top level object.
            client = new PIWebApiClient("https://sgc-pi.engr.tamu.edu:450/piwebapi", true);


            //Get the PI Data Archive object
            ds = client.DataServer.GetByPath("\\\\SGC-PI");

            // Extract all the points in the dataserver 
            var points = client.DataServer.GetPoints(ds.WebId);

            var items = points.Items;

            piPoints = new List<PIPoint>();
            piPoints.AddRange(items);

            pointDescriptors = new Dictionary<string,string>();
            foreach (var piPoint in piPoints)
            {
                pointDescriptors.Add(piPoint.Name, piPoint.Descriptor);
            }

            //populate the tree-view
            foreach (var pd in pointDescriptors)
            {
                TreeNode nd = new TreeNode() { Text = pd.Value, Name = pd.Key };
                treeView1.Nodes["root"].Nodes.Add(nd);
            }

            //Get the asset framework server
            afs = client.AssetServer.GetByPath("\\\\SGC-PI");

            var dbs = client.AssetServer.GetDatabases(afs.WebId);
            var db = client.AssetData.GetByPath(dbs.Items[1].Path);
            var eles = client.AssetData.GetElements(db.WebId);
            var k = client.Element.GetByPath(eles.Items[0].Path);
            var locs = client.Element.GetElements(k.WebId);

            foreach (var loc in locs.Items)
            {
                TreeNode n = new TreeNode() { Name = loc.Name, Text = loc.Name };
                plantTreeView.Nodes[0].Nodes.Add(n);
                n.Tag = loc;
                var processes = client.Element.GetElements(loc.WebId);
                foreach (var process in processes.Items)
                {
                    TreeNode p = new TreeNode() { Name = process.Name, Text = process.Name };
                    n.Nodes.Add(p);
                    p.Tag = process;
                    var equipmentcat = client.Element.GetElements(process.WebId);
                    if (equipmentcat.Items.Count() < 1) continue;
                    var equipments = client.Element.GetElements(equipmentcat.Items[0].WebId);
                    foreach (var equipment in equipments.Items)
                    {
                        TreeNode e = new TreeNode() { Name = equipment.Name, Text = equipment.Name };
                        e.Tag = equipment;
                        p.Nodes.Add(e);
                        var components = client.StreamSet.GetRecorded(equipment.WebId);
                        foreach (var component in components.Items)
                        {
                            TreeNode c = new TreeNode() { Name = component.Name, Text = component.Name };
                            c.Tag = component;
                            e.Nodes.Add(c);
                        }
                    }
                }
            }

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            var name = e.Node.Name;
            string pitag = "\\\\SGC-PI\\" + name;

            PIPoint cp = client.Point.GetByPath(pitag, null);

            //var interpolatedData = createdPoint.
            List<string> webIds = new List<string>() { cp.WebId };
            PIItemsStreamValues piItemsStreamValues = client.StreamSet.GetRecordedAdHoc(webIds, startTime: "*-3y", endTime: "*");
            var values = piItemsStreamValues.Items[0].Items;
            PlotData(values);

        }

        private void PlotData(List<PITimedValue> values)
        {
            formsPlot1.Reset();
            if (values.Count == 0) return;
            List<double> vals = new List<double>();
            List<DateTime> dts = new List<DateTime>();
            foreach (var value in values)
            {
                if (value.Value == null) continue;

                if (value.Value.ToString().Contains("{"))
                {
                    // handle it separately
                    var jo = JObject.Parse(value.Value.ToString());
                    foreach (var element in jo)
                    {
                        if (element.Key == "Value")
                            vals.Add(element.Value.ToObject<double>());

                    }
                    dts.Add(Convert.ToDateTime(value.Timestamp));
                }
                else
                {
                    try
                    {
                        vals.Add(Convert.ToDouble(value.Value));
                        dts.Add(Convert.ToDateTime(value.Timestamp));
                    }
                    catch
                    {
                        continue;
                    }
                    
                }
            }
            double[] xs = dts.ToArray().Select(x => x.ToOADate()).ToArray();
            //this.formsPlot1.Plot.PlotScatter(xs,vals.ToArray());

            formsPlot1.Plot.XAxis.DateTimeFormat(true);
            //formsPlot1.Plot.YAxis.SetSizeLimit(max: 100, min: -100);
            //formsPlot1.Plot.PlotScatter(xs, vals.ToArray());
            var sig = formsPlot1.Plot.AddScatter(xs, vals.ToArray());
            //var sig = formsPlot1.Plot.AddSignal(vals.ToArray());
            //sig.Render();
            formsPlot1.Render();
        }

        private void plantTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is PIElement)
            {
                var nTag = (PIElement)e.Node.Tag;
                if (nTag.Path.Contains("Equipment"))
                {
                    var values = new List<PITimedValue>();
                    var items = client.StreamSet.GetValues(nTag.WebId).Items;
                    foreach (var item in items)
                    {
                        values.Add(item.Value);
                    }
                    PlotData(values);

                }
            }
            else if (e.Node.Tag is PIStreamValues)
            {
                var nTag = (PIStreamValues)e.Node.Tag;
                if (nTag.Path.Contains("Equipment"))
                {
                    var values = new List<PITimedValue>();
                    var items = nTag.Items;
                    foreach (var item in items)
                    {
                        values.Add(new PITimedValue() { Timestamp= item.Timestamp,Value=item.Value});
                    }
                    PlotData(values);

                }
            }
            
        }
    }
}

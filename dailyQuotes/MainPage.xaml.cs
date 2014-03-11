using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using Microsoft.Phone.Shell;

namespace dailyQuotes
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private static Version TargetedVersion = new Version(7, 10, 8858);
        public static bool isTargetedVersion { get { return Environment.OSVersion.Version >= TargetedVersion; } }
        public MainPage()
        {
            InitializeComponent();

            
            //Connect using HTTP to the webpage to get XML file.
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += HttpCompleted;
            wc.DownloadStringAsync(new Uri("http://api.theysaidso.com/qod.xml?category=inspire"));
        }
        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

        private void HttpCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //If connection is successful:
            if (e.Error == null)
            {
                //Working with the XML file using XDocument.
                XDocument loadedData = XDocument.Parse(e.Result, LoadOptions.None);

                IEnumerable<XElement> xmlThing = loadedData.Descendants("response").Descendants("contents");

                foreach (string s in xmlThing.Descendants("quote"))
                {
                    //show the qoute in a message box.
                    MessageBox.Show(s);
                    string data = "Hello";
                    set(data);
                    
                }
            }
            //If file is not downloaded means error i.e. no internet connection etc.
            else
            {
                //the respective error is shown in a message box.
                MessageBox.Show(e.Error.ToString());
                
            }
        }

        //set the tile data by this function.
        private void set(string data)
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            if (appTile != null)
            {
                    // Get the new FlipTileData type.
                    Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                    // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                    Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                    // Loop through any existing Tiles that are pinned to Start.
                    var tileToUpdate = ShellTile.ActiveTiles.First();


                    // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                    var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                    // Set the properties. 
                    SetProperty(UpdateTileData, "Title", "Daily Quotes");
                    //SetProperty(UpdateTileData, "Count", 12);
                    SetProperty(UpdateTileData, "BackTitle", "Daily Quotes");
                    SetProperty(UpdateTileData, "BackContent", "Daily Quotes are shown Here.");
                    SetProperty(UpdateTileData, "WideBackContent", data);
                    // Invoke the new version of ShellTile.Update.
                    shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });


               }


        }
    }
    
    
    //The class for the Quote data which is used in XDocument
    public class Quote
    {
        string quote;
        public string thing
        {
            get { return quote; }
            set { quote = value; }
        }
    }


}
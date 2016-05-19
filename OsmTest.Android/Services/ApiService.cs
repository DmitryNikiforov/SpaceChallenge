using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
using OsmSharp.Collections.Tags;
using OsmSharp.IO.Xml;
using OsmSharp.IO.Xml.Sources;
using OsmSharp.Osm;
using OsmSharp.Osm.Xml;
using OsmSharp.Osm.Xml.v0_6;
using OsmTest.Core.Models;
using Environment = Android.OS.Environment;
using Point = OsmTest.Core.Models.Point;

namespace OsmTest.Android.Services
{
   class ApiService
   {
      private async Task<string> GetJsonAsync(string url, int timeout = 15000, CancellationTokenSource cancelToken = null)
      {
         try
         {
            string json;
            AzureService wc = new AzureService();
            json = await wc.DownloadStringAsync(url, timeout, cancelToken);
            json = json.Replace("Date(-", "Date(");

            return json;
         }
         catch (Exception exception)
         {
            return "Error";
         }
      }

      public async Task<string> GetTestData()
      {
         return await GetJsonAsync("http://spaceherdsmans.azurewebsites.net/api/values");
      }

      public async Task<Bitmap> DownloadTile(int zoomLevel, double x, double y)
      {
         AzureService service = new AzureService();
         Stream stream = service.CreateBitmap($"http://b.tile.openstreetmap.org/{zoomLevel}/{x}/{y}.png");
         byte[] imageBytes = new byte[stream.Length];
         stream.Read(imageBytes, 0, (int)stream.Length);
         BitmapFactory.Options options = new BitmapFactory.Options();
         options.InPurgeable = true;
         options.InInputShareable = true;
         options.InPreferredConfig = Bitmap.Config.Argb8888;
         return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length, options);
      }

      public async Task<List<OsmGeo>> DownloadArea(double leftLong, double bottomLat, double rightLong, double topLat)
      {
         AzureService wc = new AzureService();

         string res =
               await wc.DownloadStringAsync(
                     $"http://overpass.osm.rambler.ru/cgi/xapi_meta?*[bbox={leftLong}, {bottomLat}, {rightLong}, {topLat}]", 120000);
         if (res == null)
         {
            throw new Exception("Reading unsuccessful");
         }
         string path = Environment.ExternalStorageDirectory + Java.IO.File.Separator + "temporary_file.txt";
         Java.IO.File file = new Java.IO.File(path);
         if (file.Exists())
         {
            file.Delete();
         }
         using (StreamWriter bytes = new StreamWriter(path, true))
         {
            bytes.WriteLine(res);
         }
         return Deserialize(res);
      }

      private List<OsmGeo> Deserialize(string deserialized)
      {
         if (deserialized == null || deserialized.Trim().Length <= 0)
            return (List<OsmGeo>)null;
         OsmDocument osmDocument = new OsmDocument((IXmlSource)new XmlReaderSource(XmlReader.Create((TextReader)new StringReader(deserialized), new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse })));
         List<OsmGeo> list = new List<OsmGeo>();
         osm osm = osmDocument.Osm as osm;
         if (osm.node != null)
         {
            foreach (node xml_node in osm.node)
               list.Add((OsmGeo)this.Convertv6XmlNode(xml_node));
         }
         if (osm.way != null)
         {
            foreach (way xml_way in osm.way)
               list.Add((OsmGeo)this.Convertv6XmlWay(xml_way));
         }
         if (osm.relation != null)
         {
            foreach (relation xml_relation in osm.relation)
               list.Add((OsmGeo)this.Convertv6XmlRelation(xml_relation));
         }
         return list;
      }

      private OsmSharp.Osm.Node Convertv6XmlNode(node xml_node)
      {
         OsmSharp.Osm.Node node = new OsmSharp.Osm.Node();
         node.Id = new long?(xml_node.id);
         node.Latitude = new double?(xml_node.lat);
         node.Longitude = new double?(xml_node.lon);
         if (xml_node.tag != null)
         {
            node.Tags = (TagsCollectionBase)new TagsCollection();
            foreach (tag tag in xml_node.tag)
               node.Tags.Add(tag.k, tag.v);
         }
         node.ChangeSetId = new long?(xml_node.changeset);
         node.TimeStamp = new DateTime?(xml_node.timestamp);
         node.UserName = xml_node.user;
         node.UserId = new long?(xml_node.uid);
         node.Version = new ulong?(xml_node.version);
         node.Visible = new bool?(xml_node.visible);
         return node;
      }

      private Way Convertv6XmlWay(way xml_way)
      {
         Way way = new Way();
         way.Id = new long?(xml_way.id);
         if (xml_way.tag != null)
         {
            way.Tags = (TagsCollectionBase)new TagsCollection();
            foreach (tag tag in xml_way.tag)
               way.Tags.Add(tag.k, tag.v);
         }
         if (xml_way.nd != null)
         {
            way.Nodes = new List<long>();
            foreach (nd nd in xml_way.nd)
               way.Nodes.Add(nd.@ref);
         }
         way.ChangeSetId = new long?(xml_way.changeset);
         way.TimeStamp = new DateTime?(xml_way.timestamp);
         way.UserName = xml_way.user;
         way.UserId = new long?(xml_way.uid);
         way.Version = new ulong?(xml_way.version);
         way.Visible = new bool?(xml_way.visible);
         return way;
      }

      private Relation Convertv6XmlRelation(relation xml_relation)
      {
         Relation relation = new Relation();
         relation.Id = new long?(xml_relation.id);
         if (xml_relation.tag != null)
         {
            relation.Tags = (TagsCollectionBase)new TagsCollection();
            foreach (tag tag in xml_relation.tag)
               relation.Tags.Add(tag.k, tag.v);
         }
         if (xml_relation.member != null)
         {
            relation.Members = new List<RelationMember>();
            foreach (member member in xml_relation.member)
            {
               OsmGeoType? nullable = new OsmGeoType?();
               switch (member.type)
               {
                  case memberType.node:
                     nullable = new OsmGeoType?(OsmGeoType.Node);
                     break;
                  case memberType.way:
                     nullable = new OsmGeoType?(OsmGeoType.Way);
                     break;
                  case memberType.relation:
                     nullable = new OsmGeoType?(OsmGeoType.Relation);
                     break;
               }
               relation.Members.Add(new RelationMember()
               {
                  MemberId = new long?(member.@ref),
                  MemberRole = member.role,
                  MemberType = nullable
               });
            }
         }
         relation.ChangeSetId = new long?(xml_relation.changeset);
         relation.TimeStamp = new DateTime?(xml_relation.timestamp);
         relation.UserName = xml_relation.user;
         relation.UserId = new long?(xml_relation.uid);
         relation.Version = new ulong?(xml_relation.version);
         relation.Visible = new bool?(xml_relation.visible);
         return relation;
      }

      public async Task UploadPoint(double lat, double longitude)
      {
         Coordinates coordinates = new Coordinates()
         {
            CreatorId = Guid.Parse("3133C939-CA10-4F6F-9FEE-BB647AF50BF7"),
            CrowdsourcedPlaceType = 1,
            Point = new Point()
            {
               coordinates = new List<double> {lat, longitude},
            }
         };
         AzureService service = new AzureService();
         await service.PostDataAsync<Coordinates>("http://spaceherders.northeurope.cloudapp.azure.com/api/CrowdsourcedPlace", coordinates);
      }
   }
}
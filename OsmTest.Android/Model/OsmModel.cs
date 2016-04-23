using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OsmTest.Android.Model
{
   [XmlRoot(ElementName = "meta")]
   public class Meta
   {
      [XmlAttribute(AttributeName = "osm_base")]
      public string Osm_base { get; set; }
   }

   [XmlRoot(ElementName = "node")]
   public class Node
   {
      [XmlAttribute(AttributeName = "id")]
      public string Id { get; set; }
      [XmlAttribute(AttributeName = "lat")]
      public string Lat { get; set; }
      [XmlAttribute(AttributeName = "lon")]
      public string Lon { get; set; }
      [XmlAttribute(AttributeName = "version")]
      public string Version { get; set; }
      [XmlAttribute(AttributeName = "timestamp")]
      public string Timestamp { get; set; }
      [XmlAttribute(AttributeName = "changeset")]
      public string Changeset { get; set; }
      [XmlAttribute(AttributeName = "uid")]
      public string Uid { get; set; }
      [XmlAttribute(AttributeName = "user")]
      public string User { get; set; }
      [XmlElement(ElementName = "tag")]
      public List<Tag> Tag { get; set; }
   }

   [XmlRoot(ElementName = "tag")]
   public class Tag
   {
      [XmlAttribute(AttributeName = "k")]
      public string K { get; set; }
      [XmlAttribute(AttributeName = "v")]
      public string V { get; set; }
   }

   [XmlRoot(ElementName = "nd")]
   public class Nd
   {
      [XmlAttribute(AttributeName = "ref")]
      public string Ref { get; set; }
   }

   [XmlRoot(ElementName = "way")]
   public class Way
   {
      [XmlElement(ElementName = "nd")]
      public List<Nd> Nd { get; set; }
      [XmlElement(ElementName = "tag")]
      public List<Tag> Tag { get; set; }
      [XmlAttribute(AttributeName = "id")]
      public string Id { get; set; }
      [XmlAttribute(AttributeName = "version")]
      public string Version { get; set; }
      [XmlAttribute(AttributeName = "timestamp")]
      public string Timestamp { get; set; }
      [XmlAttribute(AttributeName = "changeset")]
      public string Changeset { get; set; }
      [XmlAttribute(AttributeName = "uid")]
      public string Uid { get; set; }
      [XmlAttribute(AttributeName = "user")]
      public string User { get; set; }
   }

   [XmlRoot(ElementName = "member")]
   public class Member
   {
      [XmlAttribute(AttributeName = "type")]
      public string Type { get; set; }
      [XmlAttribute(AttributeName = "ref")]
      public string Ref { get; set; }
      [XmlAttribute(AttributeName = "role")]
      public string Role { get; set; }
   }

   [XmlRoot(ElementName = "relation")]
   public class Relation
   {
      [XmlElement(ElementName = "member")]
      public List<Member> Member { get; set; }
      [XmlElement(ElementName = "tag")]
      public List<Tag> Tag { get; set; }
      [XmlAttribute(AttributeName = "id")]
      public string Id { get; set; }
      [XmlAttribute(AttributeName = "version")]
      public string Version { get; set; }
      [XmlAttribute(AttributeName = "timestamp")]
      public string Timestamp { get; set; }
      [XmlAttribute(AttributeName = "changeset")]
      public string Changeset { get; set; }
      [XmlAttribute(AttributeName = "uid")]
      public string Uid { get; set; }
      [XmlAttribute(AttributeName = "user")]
      public string User { get; set; }
   }

   [XmlRoot(ElementName = "osm")]
   public class Osm
   {
      [XmlElement(ElementName = "note")]
      public string Note { get; set; }
      [XmlElement(ElementName = "meta")]
      public Meta Meta { get; set; }
      [XmlElement(ElementName = "node")]
      public List<Node> Node { get; set; }
      [XmlElement(ElementName = "way")]
      public List<Way> Way { get; set; }
      [XmlElement(ElementName = "relation")]
      public List<Relation> Relation { get; set; }
      [XmlAttribute(AttributeName = "version")]
      public string Version { get; set; }
      [XmlAttribute(AttributeName = "generator")]
      public string Generator { get; set; }
   }
}
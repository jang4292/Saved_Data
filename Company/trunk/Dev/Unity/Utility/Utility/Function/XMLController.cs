using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Utility.Function
{
    public abstract class XMLController : MonoBehaviour
    {
        //public delegate void Finished();

            public interface IFinishedReadXML
        {
            void FinishedReadXML();
        }

        private IFinishedReadXML iFinishedReadXML;
        public void setIFinishedReadXML(IFinishedReadXML i)
        {
            iFinishedReadXML = i;
        }
        public abstract string getXMLFilePath {
            get;
        }
        public abstract string getSelectNodesPath
        {
            get;
        }

        public delegate void RequestRead(XmlNodeList list);

        public abstract void FinishedReadXML(XmlNodeList list);
        public void xmlReader(/*RequestRead requestRead*/)
        {
            XmlDocument xmldoc = new XmlDocument();

            //string path = SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + StringData.XML_FILE_NAME;
            try
            {
                //if (File.Exists(path))
                //{
                //   xmldoc.Load(path);
                //}
                if (File.Exists(getXMLFilePath))
                {
                    xmldoc.Load(getXMLFilePath);
                }
                else
                {
                    Log.e("Configuration File is not opend");
                };
            }
            catch (Exception e)
            {
                Log.e("[Exception] " + e.StackTrace);
            }

            XmlNodeList xnList = xmldoc.SelectNodes(getSelectNodesPath);
            FinishedReadXML(xnList);
            if(iFinishedReadXML!= null)
            {
                iFinishedReadXML.FinishedReadXML();
            }
            //requestRead(xnList);
            //XmlNodeList xnList = xmldoc.SelectNodes(StringData.ACCESS_NODE_PATH);

            /*
            VideoXMLDataList.lists.Clear();
            foreach (XmlNode xn in xnList)
            {
                VideoXMLDataList.lists.Add(new VideoXMLDataList.XMLData(getTitle(xn[StringData.ConfigurationString.PICTURE].InnerText),
                    xn[StringData.ConfigurationString.PICTURE].InnerText,
                    xn[StringData.ConfigurationString.MOVIE].InnerText,
                    xn[StringData.ConfigurationString.SIMPATH].InnerText,
                    xn[StringData.ConfigurationString.MOVIE_TIME].InnerText));
            }
            */
            //finished();
        }
        /*
        public static void XMLwrite(List<VideoDataList.VideoData> lists)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml("<configuration></configuration>");


            XmlElement newElem = doc.CreateElement("product");
            XmlAttribute newAttr = doc.CreateAttribute("id");
            newAttr.Value = "jamong";
            newElem.Attributes.Append(newAttr);


            XmlElement score = doc.CreateElement("type");
            score.InnerXml = "SCREEN";
            newElem.AppendChild(score);


            AddInnerText(newElem, "encoder1", "999");
            AddInnerText(newElem, "encoder2", "999");
            AddInnerText(newElem, "xscale", "70");
            AddInnerText(newElem, "yscale", "70");

            doc.DocumentElement.AppendChild(newElem);


            foreach (var item in lists)
            {
                newElem = doc.CreateElement("video");
                newAttr = doc.CreateAttribute("id");
                newAttr.Value = "svd001";
                newElem.Attributes.Append(newAttr);


                AddInnerText(newElem, StringData.ConfigurationString.PICTURE, @".\img\" + item.video_name + "-hard.bmp");
                AddInnerText(newElem, StringData.ConfigurationString.MOVIE, @".\mov\" + item.video_name + ".wmv");
                AddInnerText(newElem, StringData.ConfigurationString.SIMPATH, @".\sim\" + item.video_name + ".sim");
                AddInnerText(newElem, StringData.ConfigurationString.MOVIE_TIME, item.video_playtime);
                doc.DocumentElement.AppendChild(newElem);

                newElem = doc.CreateElement("video");
                newAttr = doc.CreateAttribute("id");
                newAttr.Value = "svd002";
                newElem.Attributes.Append(newAttr);

                AddInnerText(newElem, StringData.ConfigurationString.PICTURE, @".\img\" + item.video_name + "-normal.bmp");
                AddInnerText(newElem, StringData.ConfigurationString.MOVIE, @".\mov\" + item.video_name + ".wmv");
                AddInnerText(newElem, StringData.ConfigurationString.SIMPATH, @".\sim\" + item.video_name + ".sim");
                AddInnerText(newElem, StringData.ConfigurationString.MOVIE_TIME, item.video_playtime);
                doc.DocumentElement.AppendChild(newElem);
            }
            //테스트하는동안주석처리
            saveXML(doc, SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"configuration.xml");
            //xmlReader(() => { });
        }
        */

        protected string getTitle(string value)
        {
            string t = value.Substring(value.LastIndexOf('\\') + 1);
            return t.Substring(0, t.LastIndexOf('.'));
        }

        protected void AddInnerText(XmlElement parent, string Element, string value)
        {
            var element = parent.OwnerDocument.CreateElement(Element);
            element.InnerText = value;
            parent.AppendChild(element);
        }

        protected void saveXML(XmlDocument doc, string path)
        {
            using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
            }
        }

    }
}
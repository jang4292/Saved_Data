using CommonData.Data;
using System.Collections.Generic;
using System.Xml;
using Utility.Function;
using UnityEngine;

public class CustomXmlController : XMLController
{

    private const string ClassName = "CustomXmlController";

    private static CustomXmlController current = null; // 이 클래스의 싱글톤 객체 
    private static GameObject container = null; // 객체를 생성하기 위한 GameObject



    public static CustomXmlController Instance// 싱글톤 객체 만들기
    {
        get
        {
            if (current == null)
            {
                container = new GameObject();
                container.name = ClassName;
                current = container.AddComponent(typeof(CustomXmlController)) as CustomXmlController;
            }
            return current as CustomXmlController;
        }
    }


    public override string getXMLFilePath
    {
        get
        {
            return SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + StringData.XML_FILE_NAME;
        }
    }

    public override string getSelectNodesPath
    {
        get
        {
            return StringData.ACCESS_NODE_PATH;
        }
    }

    public override void FinishedReadXML(XmlNodeList list)
    {
        VideoXMLDataList.lists.Clear();
        foreach (XmlNode xn in list)
        {
            VideoXMLDataList.lists.Add(new VideoXMLDataList.XMLData(getTitle(xn[StringData.ConfigurationString.PICTURE].InnerText),
                xn[StringData.ConfigurationString.PICTURE].InnerText,
                xn[StringData.ConfigurationString.MOVIE].InnerText,
                xn[StringData.ConfigurationString.SIMPATH].InnerText,
                xn[StringData.ConfigurationString.MOVIE_TIME].InnerText));
        }
    }

    public void XMLwrite(List<VideoDataList.VideoData> lists)
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
        xmlReader();
    }
}

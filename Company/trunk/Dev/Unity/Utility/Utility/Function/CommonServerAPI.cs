using UnityEngine;
using System.Collections.Generic;
using CommonData;
using System;

namespace Utility.Function
{
    public class CommonServerAPI : MonoBehaviour
    {
        public void Login(IDictionary<string, string> data,CustomWWW.RequestHandler requestHandler, string loginUrl)
        {
            StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON,
                                              new PostData(loginUrl, data), 
                                              (string str) => {
                                              }, 
                                              (WWW www) => { requestHandler(www);
                                              })
                                        );
        }

        public void LoadImage(string url, CustomWWW.RequestHandler requestHandler)
        {
            StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.IMAGE, new PostData(url)
                , (string str) => { },
                (WWW www) =>
                {
                    requestHandler(www);
                }));
        }
    }
}
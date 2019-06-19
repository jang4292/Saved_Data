using UnityEngine;
using System.Collections.Generic;

namespace CommonData
{
    public class PostData
    {
        public string path;
        public WWWForm form;

        public PostData(string path, IDictionary<string, string> data)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> post_arg in data)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }

            this.path = path;
            this.form = form;
        }
        public PostData(string path, WWWForm form)
        {
            this.path = path;
            this.form = form;
        }
        public PostData(string path)
        {
            this.path = path;
            this.form = null;
        }
    }
}
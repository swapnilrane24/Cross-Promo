using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devshifu.CrossPromo
{
    [System.Serializable]
    public class GameItem
    {
        public string AppTitle;
        public string IconUrl;
        public string PageUrl;
        public string Id;

        public void Print()
        {
            Debug.Log("Title: " + AppTitle + ", icon url: " + IconUrl + ", page url: " + PageUrl + ", id: " + Id);
        }
    }
}
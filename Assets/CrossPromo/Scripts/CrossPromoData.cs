using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devshifu.CrossPromo
{
    [System.Serializable]
    public class CrossPromoData
    {
        public bool IsEnabled = true;
        public List<GameItem> Games;

        public CrossPromoData()
        {
            Games = new List<GameItem>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIY.UI
{
    public class UIManager : BaseManager<UIManager>
    {
        public Canvas mainCanvas;
        public Dictionary<string, UIPanel> dic_panels;






        private void Awake()
        {
            InitInstance();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

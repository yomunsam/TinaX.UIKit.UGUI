using TinaX.UIKit.Page.View;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.View
{
#nullable enable

    public class UGUIPageView : PageView
    {
        protected readonly GameObject m_uGuiPrefab;
        protected readonly UGUIPage m_uGuiPage;

        public UGUIPageView(string viewUri, GameObject uguiPrefab, UGUIPage page) : base(viewUri, page)
        {
            this.m_uGuiPrefab = uguiPrefab;
            m_uGuiPage = page;
        }

        protected GameObject? m_uGuiGameObject;



        public override void Display()
        {
            if(m_uGuiGameObject == null)
            {
                m_uGuiGameObject = GameObject.Instantiate(m_uGuiPrefab, m_uGuiPage.Transform);
                if (m_uGuiGameObject.name.Length > 7)
                    m_uGuiGameObject.name = m_uGuiGameObject.name.Substring(0, m_uGuiGameObject.name.Length - 7);
            }
        }

    }
#nullable restore
}

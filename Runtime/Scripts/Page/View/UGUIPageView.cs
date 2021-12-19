using TinaX.UIKit.Page.View;
using TinaX.XComponent.Warpper;
using UnityEngine;
using TinaX.XComponent;

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
        protected UnityEngine.Canvas? m_UnityCanvas;


        public override void Display()
        {
            if(m_uGuiGameObject == null)
            {
                m_uGuiGameObject = GameObject.Instantiate(m_uGuiPrefab, m_uGuiPage.Transform);
                if (m_uGuiGameObject.name.Length > 7)
                    m_uGuiGameObject.name = m_uGuiGameObject.name.Substring(0, m_uGuiGameObject.name.Length - 7);

                //Sort Order
                m_UnityCanvas = m_uGuiGameObject.GetComponentOrAdd<UnityEngine.Canvas>();
                m_UnityCanvas.overrideSorting = true;
            }

            

            if (m_uGuiPage.Controller != null)
            {

                //导航器

                //挂载XComponent
                var xbehaviour = new XBehaviourWarpper(m_uGuiPage.Controller, m_uGuiPage.XBehaviourWrapperReflectionProvider);
                var xcomponent = m_uGuiGameObject.GetComponentOrAdd<TinaX.XComponent.XComponent>();
                xcomponent.AddBehaviour(xbehaviour);
            }
        }

        /// <summary>
        /// 设置序号
        /// </summary>
        /// <param name="order"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SetOrder(int order)
        {
            if (m_UnityCanvas != null)
                m_UnityCanvas.sortingOrder = order;
        }
    }
#nullable restore
}

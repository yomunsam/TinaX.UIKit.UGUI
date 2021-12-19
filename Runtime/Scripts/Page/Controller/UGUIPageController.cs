using TinaX.UIKit.Page.Controller;
using TinaX.UIKit.UGUI.Page;
using UnityEngine;

namespace TinaX.UIKit.UGUI
{
#nullable enable

    public abstract class UGUIPageController : PageControllerBase
    {
        protected UGUIPage? m_uGuiPage { get; set; }

        public Transform? Transform
        {
            get
            {
                if (this.Page == null)
                    return null;
                if(this.m_uGuiPage == null)
                {
                    this.m_uGuiPage = this.Page as UGUIPage;
                }
                return m_uGuiPage?.Transform;
            }
        }
    }
#nullable restore
}

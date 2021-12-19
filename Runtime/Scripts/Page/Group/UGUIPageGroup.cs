using System;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.Group
{
    public class UGUIPageGroup : UIPageGroup
    {
        protected Transform m_RootTransform;

        public UGUIPageGroup(Transform rootTransform) : base()
        {
            m_RootTransform = rootTransform;
        }

        public UGUIPageGroup(Transform rootTransform, string name) : base(name)
        {
            m_RootTransform = rootTransform;
        }

        public Transform RootTransform => m_RootTransform;

        public override void Push(UIPageBase page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            m_Children.Add(page);
            if(page is UGUIPage)
            {
                var uPage = page as UGUIPage;
                uPage.OnJoinUGUIGroup(this);
            }
            else
            {
                page.OnJoinGroup(this);
            }

            //重新排序
            ResetOrder();
        }
    }
}

using System;
using System.Linq;
using TinaX.UIKit.Canvas;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.UGUI.Canvas;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.Group
{
#nullable enable
    public class UGUIPageGroup : UIPageGroup, IUGUIGroup
    {
        //------------构造函数们-----------------------------------------------------------------------------------------
        public UGUIPageGroup() : base()
        {
        }

        public UGUIPageGroup(string name) : base(name)
        {
        }

        //------------内部字段--------------------------------------------------------------------------------------------

        protected UIKitUGUICanvas? m_UGUICanvas;

        protected UGUIPageGroup? m_ParentUGUI;

        //------------公开属性--------------------------------------------------------------------------------------------

        public virtual UIKitUGUICanvas? UGUICanvas
        {
            get
            {
                if(m_UGUICanvas == null)
                    m_UGUICanvas = FindUIKitUGUICanvas();
                return m_UGUICanvas;
            }
            set
            {
                this.m_UGUICanvas = value;
                this.m_Canvas = value;
            }
        }

        public override UIKitCanvas? Canvas 
        { 
            get => base.Canvas; 
            set
            {
                base.Canvas = value;
                if(value != null && value is UIKitUGUICanvas uGUICanvas)
                    m_UGUICanvas = uGUICanvas;
            }
        }

        /// <summary>
        /// 该UIGroup中是否有某个元素使用遮罩
        /// 这个属性注意仅由Group管理
        /// </summary>
        public bool UseMask { get; set; }

        //------------公开方法们---------------------------------------------------------------------------------------

        public override void Push(UIPageBase page, object?[]? displayMessageArgs = null)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if(page is UGUIPage uPage)
            {
                Push(uPage, pushArgs: null);
                return;
            }


            m_Children.Add(page);
            page.OnJoinGroup(this, displayMessageArgs);
            //重新排序
            ResetOrder(); //Todo: 可优化，只需要排序受影响的部分
        }

        public virtual void Push(UGUIPage page, PushUGUIPageArgs? pushArgs = null)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            m_Children.Add(page);
            page.OnJoinUGUIGroup(this, pushArgs?.DisplayMessageArgs);

            //重新排序
            ResetOrder(); //Todo: 可优化，只需要排序受影响的部分

            //显示遮罩？
            if (pushArgs?.UseBackgroundMask ?? false)
            {
                this.UseBackgroundMask(ref page, pushArgs?.CloseByMask ?? false, pushArgs?.BackgroundMaskColor);
            }
        }

        

        public virtual UGUIPageGroup GetLastChildUGUIGroup()
        {
            if (m_Children.Count > 0)
            {
                for (var i = m_Children.Count - 1; i >= 0; i--)
                {
                    var child = m_Children[i];
                    if (child is UGUIPageGroup group)
                    {
                        return group.GetLastChildUGUIGroup();
                    }
                }
            }
            return this;
        }

        public override void OnJoinGroup(UIPageGroup group, object?[]? displayMessageArgs)
        {
            base.OnJoinGroup(group, displayMessageArgs);
            if(m_Parent is UGUIPageGroup uGUIPageGroup)
                m_ParentUGUI = uGUIPageGroup;
        }

        public override void OnLeaveGroup(UIPageGroup group)
        {
            base.OnLeaveGroup(group);
            m_ParentUGUI = null;
        }


        public override void Remove(UIPageBase page)
        {
            base.Remove(page);
            //从UI栈上移除之后，在UGUI这边还需要做更多操作
            if(page is UGUIPage uGuiPage)
            {
                //看看有没有遮罩
                if (uGuiPage.UseMask)
                {
                    //根据UI栈推断这个当前Page和这个
                }
            }
        }

        //------------内部方法们-----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 使用（显示）背景遮罩
        /// Group和UIKitCanvas里都有叫UseBackgroundMask的方法，通常而言应该是Group里的这个方法调UIKitCanvas里的方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="closeByMask"></param>
        /// <param name="maskColor"></param>
        protected virtual void UseBackgroundMask(ref UGUIPage page, bool closeByMask, Color? maskColor)
        {
            //给Page加上标记
            page.UseMask = true;
            page.CloseByMask = closeByMask;
            page.MaskColor = maskColor;

            //需要为当前页面真的动手执行遮罩操作吗？
            if (this.UseMask)
            {
                //本组已经有某个Mask使用了遮罩，所以如果给我们的目标Page不是最顶层Page的话，就不用管了
                var last_page = m_Children.LastOrDefault();
                if (last_page != page)
                    return; //扭头就走

                //剩下的情况就是需要遮罩的
            }

            //这里当然也是需要遮罩的情况
            SetUseMaskAndParents(true); //意思是当前组有至少一个子项使用了遮罩
            this.UGUICanvas?.UseBackgroundMask(page, closeByMask, maskColor);
        }

        /// <summary>
        /// 寻找Canvas
        /// </summary>
        /// <returns></returns>
        protected virtual UIKitUGUICanvas? FindUIKitUGUICanvas()
        {
            if (m_Parent == null)
            {
                if (m_Canvas != null)
                {
                    if (m_UGUICanvas == null)
                        m_UGUICanvas = m_Canvas as UIKitUGUICanvas;
                    return m_UGUICanvas;
                }
                else
                    return null;
            }
            else
                return (m_Parent as UGUIPageGroup)?.FindUIKitUGUICanvas();
        }

        /// <summary>
        /// 递归设置自己和所有父级的UseMask属性
        /// </summary>
        /// <param name="useMask"></param>
        /// <returns></returns>
        private void SetUseMaskAndParents(bool useMask)
        {
            this.UseMask = UseMask;

            if (m_Parent != null && m_ParentUGUI == null)
            {
                m_ParentUGUI = m_Parent as UGUIPageGroup;
            }

            if (m_ParentUGUI != null)
            {
                m_ParentUGUI?.SetUseMaskAndParents(useMask);
            }
        }
    }
#nullable restore
}

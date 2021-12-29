using System;
using System.Linq;
using TinaX.UIKit.Canvas;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.Page.BackgroundMask;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.Group
{
#nullable enable
    public class UGUIPageGroup : UIPageGroup, IUGUIGroup, IBackgroundMask
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
        public bool IsUseBackgroundMask { get; set; }

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
            m_UGUICanvas = null; //如果我们离开了一个组，说明我们以前进入过一个组，这就说明我们肯定不是Canvas的根级组，所以要把Canvas清空；
        }


        public override void Remove(UIPageBase child)
        {
            //从UI栈上移除之前，在UGUI这边还需要做更多操作
            //检查是否需要处理背景遮罩
            if (child is IBackgroundMask pageMask)
            {
                if (pageMask.IsUseBackgroundMask)
                {
                    //将要被移除的子项使用了背景遮罩，我们需要为其做处理吗？
                    ChildNotUseBackgroundMask(child);
                }
            }

            base.Remove(child); //然后这里会执行UI栈的实际移除
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
            page.IsUseBackgroundMask = true;
            page.CloseByBackgroundMask = closeByMask;
            page.BackgroundMaskColor = maskColor;

            //需要为当前页面真的动手执行遮罩操作吗？
            if (this.IsUseBackgroundMask)
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
        protected void SetUseMaskAndParents(bool useMask)
        {
            this.IsUseBackgroundMask = useMask;

            if (m_Parent != null && m_ParentUGUI == null)
            {
                m_ParentUGUI = m_Parent as UGUIPageGroup;
            }

            if (m_ParentUGUI != null)
            {
                m_ParentUGUI?.SetUseMaskAndParents(useMask);
            }
        }

        /// <summary>
        /// 当这个方法被调用，意味着我们的某个子项不再使用UI遮罩
        /// </summary>
        protected void ChildNotUseBackgroundMask(UIPageBase child)
        {
            /*
             * 当我们的某一个子项不再使用背景遮罩了
             * 1. 检查我们是否要为其处理遮罩问题，（如果有A、B、C三页使用了遮罩，移除的是B或者A的话，就不用处理）
             * 2. 检查我们的UI栈中还有没有其它子项使用了背景遮罩，
             *      - 如果有，为其设置遮罩
             *      - 如果没有了，告诉上级组
             *          - 如果我们是顶级组，那通知Canvas移除遮罩
             */

            bool remove_mask = false; //如果我们需要remove mask，则把这里设为true
            IBackgroundMask? last_useMask_child = null;

            for(int i= m_Children.Count-1; i>=0; i--) //直接倒着找就好
            {
                if(m_Children[i] is IBackgroundMask maskPage)
                {
                    if (maskPage.IsUseBackgroundMask)
                    {
                        if(maskPage == child && !remove_mask)
                        {
                            //我们倒序找到的第一个使用了背景遮罩的child就是我们要移除的child，所以
                            remove_mask = true;
                            continue; //然后我们不要断掉循环， 我们继续往下看看，当前UI栈还有没有其他使用了背景遮罩的
                        }

                        if (remove_mask) //这个设为true了，说明我们倒序查找到了child前面，那么，我们第一个要找的就是它
                        {
                            last_useMask_child = maskPage;
                            break; //现在我们可以断掉循环了
                        }

                    }
                }
            }

            if (!remove_mask)
                return; //不用管了

            if(last_useMask_child == null)
            {
                //我们整个组应该都没有使用遮罩的子项了，也就是说
                this.IsUseBackgroundMask = false; //我们组本身就不使用遮罩了
                if(m_ParentUGUI != null)
                {
                    //告诉上级组整个情况
                    m_ParentUGUI.ChildNotUseBackgroundMask(this);
                }
                else
                {
                    //我们自己就是顶级组，
                    m_UGUICanvas?.RemoveBackgroundMask(); //整个UI树都没有使用遮罩的了
                }
            }
            else
            {
                //我们当前UI栈下有另一位同学使用了遮罩，下面把话筒塞它嘴里
                if(last_useMask_child is IBackgroundMaskInfo maskPage)
                {
                    //这种情况最简单了
                    m_UGUICanvas?.UseBackgroundMask((UGUIPage)maskPage, maskPage.CloseByBackgroundMask, maskPage.BackgroundMaskColor);
                }
                else
                {
                    //这是个Group?
                    if(last_useMask_child is UGUIPageGroup maskGroup)
                    {
                        var last_page = maskGroup.GetLastUsedBackgroundMaskPage();
                        m_UGUICanvas?.UseBackgroundMask(last_page!, last_page!.CloseByBackgroundMask, last_page!.BackgroundMaskColor);
                    }
                    else
                    {
                        throw new Exception("Unexpected situation!"); //向开发者表示愤慨
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前UI栈中最顶层一个使用了背景遮罩的【UGUIPage】
        /// </summary>
        /// <returns></returns>
        protected UGUIPage? GetLastUsedBackgroundMaskPage()
        {
            for(int i = m_Children.Count - 1; i >= 0; i--)
            {
                if(m_Children[i] is IBackgroundMask maskChild)
                {
                    if (maskChild.IsUseBackgroundMask)
                    {
                        if (maskChild is UGUIPageGroup group)
                        {
                            //是个组啊，
                            return group.GetLastUsedBackgroundMaskPage();
                        }
                        else if (maskChild is UGUIPage page)
                            return page;
                        else
                            return null;
                    }
                }
            }

            return null;
        }
    }
#nullable restore
}

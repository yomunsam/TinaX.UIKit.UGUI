using TinaX.UIKit.Canvas;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.Group;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Canvas
{
#nullable enable
    public class UIKitUGUICanvas : UIKitCanvas
    {
        //public UIKitUGUICanvas() : base() { }
        public UIKitUGUICanvas(UGUIPageGroup rootGroup, Transform rootTransform) : base(rootGroup)
        {
            m_RootGroupUGUI = rootGroup;
            this.m_RootTransform = rootTransform;
        }
        public UIKitUGUICanvas(UGUIPageGroup rootGroup , Transform rootTransform, string name):base(rootGroup, name)
        {
            m_RootGroupUGUI = rootGroup;
            this.m_RootTransform = rootTransform;
        }

        protected UGUIPageGroup m_RootGroupUGUI;

        /// <summary>
        /// UIKitCanvas Root transform
        /// </summary>
        private readonly Transform m_RootTransform;

        /// <summary>
        /// 该UGUI Canvas的背景遮罩游戏对象
        /// </summary>
        protected GameObject? m_BackgroundMaskGameObject;
        protected UnityEngine.Canvas? m_BackgroundMaskCanvas;
        protected UnityEngine.UI.Image? m_BackgroundMaskImage;
        protected UnityEngine.UI.Button? m_BackgroundMaskButton;
        //protected UGUIPage? m_BackgroundMaskTargetPage;

        public UGUIPageGroup RootGroupUGUI
        {
            get
            {
                if (m_RootGroupUGUI == null)
                    m_RootGroupUGUI = (UGUIPageGroup)RootGroup;
                return m_RootGroupUGUI;
            }
        }

        public Transform RootTransform => m_RootTransform;

        public int SortingLayerId { get; set; } = 0;

        /// <summary>
        /// 使用（显示）背景遮罩
        /// </summary>
        /// <param name="closeByMask"></param>
        /// <param name="maskColor"></param>
        public virtual void UseBackgroundMask(UGUIPage page, bool closeByMask, Color? maskColor)
        {
            var group = page.ParentUGUI;
            //我们还是看一下需不需要处理
            var lastChildGroup = m_RootGroupUGUI!.GetLastChildUGUIGroup();
            if (lastChildGroup!.UseMask && group != lastChildGroup)
            {
                return; //不需要处理，因为当前最顶级的Group已经再使用Mask，并且给我们的Page不在最顶级的Group中
            }

            if (m_BackgroundMaskGameObject == null)
            {
                m_BackgroundMaskGameObject = this.m_RootTransform.gameObject.FindOrCreateGameObject("_mask_" + this.SortingLayerId, typeof(UnityEngine.Canvas))
                    .SetLayerRecursive(m_RootTransform.gameObject.layer);

                var rectTrans  = m_BackgroundMaskGameObject.GetComponent<RectTransform>(); //下面是让遮罩GameObject全屏铺开
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.one;
                rectTrans.localScale = Vector3.one;
                rectTrans.sizeDelta = Vector2.zero;
                rectTrans.localPosition = Vector3.zero;

                m_BackgroundMaskGameObject.GetComponentOrAdd<UnityEngine.UI.GraphicRaycaster>(); //这玩意作用是挡住点击射线，不让遮罩被戳穿
            }
            if(m_BackgroundMaskCanvas == null)
            {
                m_BackgroundMaskCanvas = m_BackgroundMaskGameObject.GetComponentOrAdd<UnityEngine.Canvas>();
                m_BackgroundMaskCanvas.overrideSorting = true;
                m_BackgroundMaskCanvas.sortingLayerID = this.SortingLayerId;
            }

            if(m_BackgroundMaskImage == null)
            {
                m_BackgroundMaskImage = m_BackgroundMaskGameObject.GetComponentOrAdd<UnityEngine.UI.Image>();
                m_BackgroundMaskImage.raycastTarget = true;
            }

            if(m_BackgroundMaskButton == null)
            {
                m_BackgroundMaskButton = m_BackgroundMaskGameObject.GetComponentOrAdd<UnityEngine.UI.Button>();
                m_BackgroundMaskButton.targetGraphic = m_BackgroundMaskImage;
                m_BackgroundMaskButton.transition = UnityEngine.UI.Selectable.Transition.None;
            }

            //------处理当次遮罩------------

            m_BackgroundMaskCanvas.sortingOrder = page.Order - 1; //设置SortingOrder是其目标Page的序号-1
            m_BackgroundMaskImage.color = maskColor ?? Color.black; //设置遮罩颜色 Todo: 默认色的功能还没有
            m_BackgroundMaskButton.onClick.RemoveAllListeners();
            if (closeByMask)
            {
                m_BackgroundMaskButton.onClick.AddListener(() =>
                {
                    Debug.Log("Todo:关闭UI操作");
                    page.ClosePage();
                });
            }
        }
    
        
    }
#nullable restore
}

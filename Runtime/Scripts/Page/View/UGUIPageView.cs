using TinaX.Services;
using TinaX.UIKit.Page.View;
using TinaX.UIKit.UIMessage;
using TinaX.XComponent;
using TinaX.XComponent.Warpper;
using UniRx;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.View
{
#nullable enable

    public class UGUIPageView : PageView
    {
        protected readonly GameObject m_uGuiPrefab;
        protected readonly UGUIPage m_uGuiPage;
        private readonly IAssetService m_AssetService;

        public UGUIPageView(string viewUri, GameObject uguiPrefab, UGUIPage page, IAssetService assetService) : base(viewUri, page)
        {
            this.m_uGuiPrefab = uguiPrefab;
            m_uGuiPage = page;
            this.m_AssetService = assetService;
        }

        protected GameObject? m_uGuiGameObject;
        protected UnityEngine.Canvas? m_UnityCanvas;


        public override void Display(object?[]? args)
        {
            if(m_uGuiGameObject == null)
            {
                if (m_uGuiPage.Parent == null)
                    Debug.LogError("The UI page does not specify a UI group, but is called to display view");
                m_uGuiGameObject = GameObject.Instantiate(m_uGuiPrefab, m_uGuiPage?.UGUICanvas?.RootTransform);
                if (m_uGuiGameObject.name.Length > 7)
                    m_uGuiGameObject.name = m_uGuiGameObject.name.Substring(0, m_uGuiGameObject.name.Length - 7);

                m_uGuiPage!.SetName(m_uGuiGameObject.name);
                m_uGuiPage!.SetTransform(m_uGuiGameObject.transform);

                //Sort Order
                m_UnityCanvas = m_uGuiGameObject.GetComponentOrAdd<UnityEngine.Canvas>();
                m_UnityCanvas.overrideSorting = true;
            }

            

            if (m_uGuiPage.Controller != null)
            {
                //挂载XComponent
                var xbehaviour = new XBehaviourWarpper(m_uGuiPage.Controller, m_uGuiPage.XBehaviourWrapperReflectionProvider);
                var xcomponent = m_uGuiGameObject.GetComponentOrAdd<TinaX.XComponent.XComponent>();
                xcomponent.AddBehaviour(xbehaviour);
            }


            //发送消息
            m_Page.SendUIDisplayMessage(args);
        }

        public override void Destroy(System.TimeSpan? delayTime = null)
        {
            if (m_Destroyed)
                return;

            if(m_uGuiGameObject != null)
            {
                if (delayTime != null)
                {
                    Observable.Timer(delayTime.Value)
                        .Subscribe(_ => { DestroyNow(); })
                        .AddTo(m_uGuiGameObject);
                }
                else
                {
                    DestroyNow();
                }
            }
            
        }

        private void DestroyNow()
        {
            if (m_Destroyed)
                return;
            //销毁GameObject
            if (m_uGuiGameObject != null)
            {
                m_uGuiGameObject.Destroy();
            }
            m_uGuiGameObject = null; //因为GameObject重载了 !=、==这些符号，所以你以为这儿是null的其实它可能不是null，所以手动把它设一遍null以防内存泄露（奇妙的设计）

            //释放Prefab
            if(m_uGuiPrefab != null)
            {
                m_AssetService.Release(m_uGuiPrefab);
            }
            m_UnityCanvas = null; //释放引用，其实照理说也没必要，强迫症
            m_Destroyed = true;
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


        #region UI Messages

        /// <summary>
        /// 当对UIController发送消息失败时，我们这里能不能进行更多的尝试呢？
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool SendUIDisplayMessage(object?[]? args)
        {
            //试试对挂在根组件上的任意继承自 MonoBehaviour 的组件发起调用。
            if (m_uGuiGameObject != null)
            {
                var components = m_uGuiGameObject.GetComponents<MonoBehaviour>();
                if(components != null && components.Length > 0)
                {
                    for(int i = 0; i < components.Length; i++)
                    {
                        if (components[i] is UGUIPageViewComponent)
                            continue;

                        if(components[i] is IUIDisplayMessage displayMsg)
                        {
                            displayMsg.OnDisplayed(args);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        #endregion
    }
#nullable restore
}

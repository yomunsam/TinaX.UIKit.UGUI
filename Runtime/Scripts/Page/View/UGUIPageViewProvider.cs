using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Services;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.View;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.View
{
#nullable enable

    public class UGUIPageViewProvider : IPageViewProvider<UGUIPageView, UGUIPage>
    {
        private readonly string m_ViewUri;
        private readonly IAssetService m_AssetService;

        public UGUIPageViewProvider(string viewLoadPath, IAssetService assetService)
        {
            this.m_ViewUri = viewLoadPath;
            this.m_AssetService = assetService;
        }

        public bool SupportAsynchronous => true; //支持异步方法

        public PageView GetPageView(UIPageBase page)
            => GetPageViewGeneric((UGUIPage)page);


        public async UniTask<PageView> GetPageViewAsync(UIPageBase page, CancellationToken cancellationToken = default)
        {
            var view = await GetPageViewGenericAsync((UGUIPage)page, cancellationToken);
            return view;
        }



        public UGUIPageView GetPageViewGeneric(UGUIPage page)
        {
            throw new NotImplementedException();
        }

        public async UniTask<UGUIPageView> GetPageViewGenericAsync(UGUIPage page, CancellationToken cancellationToken = default)
        {
            var ugui_prefab = await m_AssetService.LoadAsync<GameObject>(m_ViewUri, cancellationToken);
            var pageView = new UGUIPageView(m_ViewUri, ugui_prefab, page);
            return pageView;
        }
    }
#nullable disable
}

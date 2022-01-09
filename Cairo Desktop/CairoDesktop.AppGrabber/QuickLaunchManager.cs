﻿using ManagedShell.WindowsTasks;

namespace CairoDesktop.AppGrabber
{
    public class QuickLaunchManager
    {
        private readonly IAppGrabber _appGrabber;
        
        public QuickLaunchManager(IAppGrabber appGrabber)
        {
            _appGrabber = appGrabber;
        }
        
        public ApplicationInfo GetQuickLaunchApplicationInfo(ApplicationWindow window)
        {
            // it would be nice to cache this, but need to handle case of user adding/removing app via various means after first access
            foreach (ApplicationInfo ai in _appGrabber.QuickLaunch)
            {
                if (ai.Target.ToLower() == window.WinFileName.ToLower() || (window.IsUWP && ai.Target == window.AppUserModelID))
                {
                    return ai;
                }
                
                if (window.Title.ToLower().Contains(ai.Name.ToLower()))
                {
                    return ai;
                }
            }

            return null;
        }

        public void AddToQuickLaunch(bool isUWP, string path)
        {
            if (isUWP)
            {
                // store app, do special stuff
                _appGrabber.AddStoreApp(path, AppCategoryType.QuickLaunch);
            }
            else
            {
                _appGrabber.AddByPath(path, AppCategoryType.QuickLaunch);
            }
        }

        public void AddToQuickLaunch(ApplicationInfo app)
        {
            if (_appGrabber.QuickLaunch.Contains(app))
            {
                return;
            }

            ApplicationInfo appClone = app.Clone();
            appClone.Icon = null;

            _appGrabber.QuickLaunch.Add(appClone);

            _appGrabber.Save();
        }
    }
}

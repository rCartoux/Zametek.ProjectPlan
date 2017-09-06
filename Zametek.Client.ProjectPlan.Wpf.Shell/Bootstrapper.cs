﻿using Autofac;
using Prism.Autofac;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.IO;
using System.Windows;
using Zametek.Contract.ProjectPlan;
using Zametek.Engine.ProjectPlan;
using Zametek.Manager.ProjectPlan;
using Zametek.Access.ProjectPlan;

namespace Zametek.Client.ProjectPlan.Wpf.Shell
{
    public class Bootstrapper
        : AutofacBootstrapper, IDisposable
    {
        #region Fields

        private bool m_Disposed;

        #endregion

        #region Internal methods

        internal static void HandleException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            MessageBox.Show(ex.Message, "Exception");
            Environment.Exit(1);
        }

        #endregion

        #region AutofacBootstrapper Overrides

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<DateTimeCalculator>()
                .As<IDateTimeCalculator>();
            builder.RegisterType<FileDialogService>()
                .As<IFileDialogService>();

            builder.RegisterType<GraphProcessingEngine>()
                .As<IGraphProcessingEngine>().SingleInstance();
            builder.RegisterType<MetricAssessingEngine>()
                .As<IMetricAssessingEngine>().SingleInstance();
            builder.RegisterType<ProjectManager>()
                .As<IProjectManager>().SingleInstance();
            builder.RegisterType<SettingResourceAccess>()
                .As<ISettingResourceAccess>().SingleInstance();
            builder.RegisterType<SettingManager>()
                .As<ISettingManager>().SingleInstance();

            builder.RegisterType<MainViewModel>()
                .As<IMainViewModel>()
                .As<IActivitiesManagerViewModel>()
                .As<IArrowGraphManagerViewModel>()
                .As<IResourceChartsManagerViewModel>()
                .SingleInstance();

            base.ConfigureContainerBuilder(builder);
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(ClientWpfModule));
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            //mappings.RegisterMapping(typeof(Xceed.Wpf.AvalonDock.DockingManager), ServiceLocator.Current.GetInstance<DockingManagerRegionAdapter>());
            //mappings.RegisterMapping(typeof(Grid), ServiceLocator.Current.GetInstance<ContentControlRegionAdapter>());
            //mappings.RegisterMapping(typeof(TabControl), ServiceLocator.Current.GetInstance<TabControlRegionAdapter>());
            //mappings.RegisterMapping(typeof(Ribbon), ServiceLocator.Current.GetInstance<RibbonRegionAdapter>());
            return mappings;
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            IRegionBehaviorFactory factory = base.ConfigureDefaultRegionBehaviors();
            //factory.AddIfMissing(BehaviorName, typeof(BehaviorType));
            return factory;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            //base.InitializeShell();
            //Application.Current.MainWindow = (ShellView)Shell;
            Application.Current.MainWindow.Show();

            //check if started with filename param
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]) && Path.GetExtension(args[1]) == ".zpp")
            {
                var mainView = Container.Resolve<IMainViewModel>();
                mainView.OpenProjectPlanFileCommand.Execute(args[1]);
            }

            // Create any core application services here.

        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
            Application.Current.MainWindow.Activate();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }
            if (disposing)
            {
                // Free any other managed objects here. 
            }

            // Free any unmanaged objects here. 

            m_Disposed = true;
        }

        #endregion
    }
}

using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Project.Handler
{
    public static class CrashHandler
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Thread thread = new Thread(() => ShowCrashWindow((Exception)e.ExceptionObject));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        public static void ShowCrashWindow(Exception e)
        {
            var appBuilder = AppBuilder.Configure(() => new CrashApp(e))
                                       .UsePlatformDetect()
                                       .LogToTrace();
            appBuilder.StartWithClassicDesktopLifetime(new string[0]);
        }
    }

    public class CrashApp : Application
    {
        private readonly Exception _exception;

        public CrashApp(Exception exception)
        {
            _exception = exception;
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new CrashWindow(_exception);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }

    public class CrashWindow : Window
    {
        public CrashWindow(Exception e)
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            var textBlock = new TextBlock
            {
                Text = "An application error occurred. Please contact support.",
                Margin = new Thickness(0, 0, 0, 10)
            };
            var details = new TextBlock { Text = e.Message + "\n\n" + e.StackTrace, Margin = new Thickness(0, 0, 0, 10) };
            var closeButton = new Button { Content = "Close" };
            closeButton.Click += (_, __) => Close();

            panel.Children.Add(textBlock);
            panel.Children.Add(details);
            panel.Children.Add(closeButton);
            Content = panel;
        }
    }
}

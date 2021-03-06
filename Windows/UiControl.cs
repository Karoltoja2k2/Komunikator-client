﻿using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Windows
{
    class UiControl
    {
        public static System.Drawing.Point GetPosition(Window win)
        {
            return new System.Drawing.Point((int)win.Left, (int)win.Top);
        }

        public static void ChangeWindow(Window open, Window toOpen)
        {
            System.Drawing.Point loc = GetPosition(open);
            toOpen.Top = loc.Y;
            toOpen.Left = loc.X;
            App.Current.MainWindow = toOpen;
            toOpen.Show();
            open.Close();
        }

        public static void OpenWindow(Window open, Window toOpen)
        {
            System.Drawing.Point loc = GetPosition(open);
            toOpen.Top = loc.Y;
            toOpen.Left = loc.X;
            toOpen.Show();
        }
    }
}

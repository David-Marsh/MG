using System;

namespace MG.Shared.UI.Controls
{
    public interface IMouseControl
    {
        event EventHandler Clicked;
        event EventHandler MouseDown;
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        event EventHandler MouseUp;

        void OnClicked();
        void OnMouseDown();
        void OnMouseEnter();
        void OnMouseLeave();
        void OnMouseUp();
    }
}
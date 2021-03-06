﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Torch.UserInterface;

namespace SRPG.Scene.Battle
{
    public partial class QueuedCommandsDialog : WindowControl
    {
        private List<QueuedCommandControl> _controls;

        public Action ExecuteClicked = () => { };

        private void InitializeComponent()
        {
            EnableDragging = false;

            _controls = new List<QueuedCommandControl>();
            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(1.0f, 0.0f),
                new UniScalar(0.0f, 300.0f), new UniScalar(0.0f, 100.0f)
            );
            UpdateControls();
        }

        public void UpdateControls()
        {
            foreach(var control in _controls)
            {
                // should always contain it, but just to be safe...
                if (Children.Contains(control)) Children.Remove(control);
            }
            _controls.Clear();

            ButtonControl executeControl = new ButtonControl();
            executeControl.Text = "Execute!";
            executeControl.Bounds = new UniRectangle(
                new UniScalar(0.0f, 15.0f), new UniScalar(1.0f, -60.0f),
                new UniScalar(1.0f, -30.0f), new UniScalar(0.0f, 45.0f)
            );
            executeControl.Pressed += (s, a) => ExecuteClicked.Invoke();
            Children.Add(executeControl);

            for (var i = 0; i < Commands.Count; i++)
            {
                var command = Commands.ElementAt(i);
                QueuedCommandControl control = new QueuedCommandControl();
                control.Command = command;

                control.Bounds = new UniRectangle(
                    new UniScalar(0.0f, 15.0f), new UniScalar(0.0f, 15.0f + (49.0f * i)), 
                    new UniScalar(1.0f, -30.0f), new UniScalar(0.0f, 50.0f) 
                );

                _controls.Add(control);
                Children.Add(control);
            }

            Bounds.Size.Y.Offset = 95 + 49 * (Commands.Count);
            Bounds.Location.Y.Offset = 0 - Bounds.Size.Y.Offset;
        }
    }
}

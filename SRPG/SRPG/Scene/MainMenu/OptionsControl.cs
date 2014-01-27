﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Torch.Game;

namespace SRPG.Scene.MainMenu
{
    public partial class OptionsControl
    {
        public delegate void OptionsCanceledDelegate();
        // todo - this is such a bad idea. i need a better container for the options
        public delegate void OptionsSavedDelegate(bool fullScreen, int width, int height);

        public OptionsCanceledDelegate OptionsCanceled = () => { };
        public OptionsSavedDelegate OptionsSaved = (f, w, h) => { };

        public OptionsControl()
        {
            InitializeComponent();
            _fullScreenButton.Pressed +=
                (s, a) => _fullScreenButton.Text = _fullScreenButton.Text == "Yes" ? "No" : "Yes";
            _resolutionButton.Pressed += ShowResolutions;

            _cancelButton.Pressed += (s, a) => OptionsCanceled.Invoke();
            _saveButton.Pressed += (s, a) =>
                {
                    var resolution = _resolutionButton.Text.Split('x');
                    OptionsSaved.Invoke
                        (_fullScreenButton.Text == "Yes", int.Parse(resolution[0]), int.Parse(resolution[1]));
                };
        }

        public void UpdateSettings(Game game)
        {
            var graphics = (GraphicsDeviceManager)(game.Services.GetService(typeof (GraphicsDeviceManager)));
            _fullScreenButton.Text = graphics.IsFullScreen ? "Yes" : "No";
            _resolutionButton.Text = graphics.PreferredBackBufferWidth + "x" + graphics.PreferredBackBufferHeight;
        }

        private void ShowResolutions(object sender, EventArgs args)
        {
            
        }
    }
}
